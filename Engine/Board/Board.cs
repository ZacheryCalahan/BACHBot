namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public class Board {
        // Game board
        public int[] gameboard { get; private set; }

        // Gamestate stuff.
        public GameState CurrentGameState;
        public bool WhiteToMove { get; private set; }
        private Stack<GameState> gameStateHistory;
        public int MoveColor => WhiteToMove ? Piece.White : Piece.Black;
        public int OpponentColor => WhiteToMove ? Piece.Black : Piece.White;

        public Board() {
            gameboard = new int[64];
            gameStateHistory = new Stack<GameState>();
        }

        public void MakeMove(Move move) {
            // Useful values
            int pieceMoved = gameboard[move.StartSquare];
            int location = move.StartSquare;
            int colorMoved = Piece.GetPieceColor(pieceMoved);

            // Piece
            int pieceCaptured = 0;
            
            // Set En Passant Square to where a pawn moved to if move was double push pawn.
            int newEnPassantSquare = (move.MoveFlag == Move.PawnDoublePush) ? move.TargetSquare : -1;

            // Calculate 50 move counter.
            int newFiftyMoveCounter;
            if (Piece.GetPieceType(pieceMoved) == Piece.Pawn || move.IsPieceCaptured) {
                newFiftyMoveCounter = 1;
            } else {
                newFiftyMoveCounter = CurrentGameState.fiftyMoveCounter + 1;
            }

            // Move pieces
            if (move.IsEnpassantCapture) { // Enpassant
                pieceCaptured = gameboard[CurrentGameState.enPassantSquare];
                gameboard[CurrentGameState.enPassantSquare] = 0; // Remove enpassant pawn

                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;
            } else if (move.MoveFlag == Move.CastleFlag) { // Castle
                int rookOffset = 0;
                int rookLocation = 0;
                switch (move.TargetSquare) {
                    case 62: // black kingside
                        rookOffset += -2;
                        rookLocation = 63;
                        break;
                    case 52: // black queenside
                        rookOffset += 3;
                        rookLocation = 56;
                        break;
                    case 2: // white queenside
                        rookOffset += 3;
                        rookLocation = 0;
                        break;
                    case 6: // white kingside
                        rookOffset += -2;
                        rookLocation = 7;
                        break;
                    default:
                        throw new InvalidDataException();
                }

                // move king
                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;

                // move rook
                gameboard[rookOffset + rookLocation] = gameboard[rookLocation];
                gameboard[rookLocation] = 0;
            } else if (move.IsPromotion) { // Promotion
                int piecePromotedTo = 0;
                int colorToPromote = Piece.GetPieceColor(gameboard[move.StartSquare]);
                switch (move.PromotionPieceType) {
                    case (Piece.Queen):
                        piecePromotedTo = Piece.Queen | colorToPromote;
                        break;
                    case (Piece.Rook):
                        piecePromotedTo = Piece.Rook | colorToPromote;
                        break;
                    case (Piece.Knight):
                        piecePromotedTo = Piece.Knight | colorToPromote;
                        break;
                    case (Piece.Bishop):
                        piecePromotedTo = Piece.Bishop | colorToPromote;
                        break;
                    default:
                        break;
                }

                // Commit the promotion and move.
                gameboard[move.TargetSquare] = piecePromotedTo;
                gameboard[move.StartSquare] = 0;

            } else { // Normal moves
                pieceCaptured = gameboard[move.TargetSquare];
                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;
            }

            int newCastlingRights = CurrentGameState.castlingRights;
            // Calculate castle rights.
            if (Piece.GetPieceType(pieceMoved) == Piece.King) {
                if (colorMoved == Piece.White) {
                    newCastlingRights &= GameState.ClearWhiteKingsideMask;
                    newCastlingRights &= GameState.ClearWhiteQueensideMask;
                } else {
                    newCastlingRights &= GameState.ClearBlackKingsideMask;
                    newCastlingRights &= GameState.ClearBlackQueensideMask;
                }
            } else if (Piece.GetPieceType(pieceMoved) == Piece.Rook) {
                if (colorMoved == Piece.White) {
                    if (location == 0) {
                        newCastlingRights &= GameState.ClearWhiteQueensideMask;
                    } else if (location == 7) {
                        newCastlingRights &= GameState.ClearWhiteKingsideMask;
                    }
                } else {
                    if (location == 56) {
                        newCastlingRights &= GameState.ClearBlackQueensideMask;
                    } else if (location == 63) {
                        newCastlingRights &= GameState.ClearBlackKingsideMask;
                    }
                }
            }

            // Gamestate stuff
            WhiteToMove = !WhiteToMove; // pass off turn

            gameStateHistory.Push(CurrentGameState);
            CurrentGameState = new GameState(newEnPassantSquare, newCastlingRights, newFiftyMoveCounter, pieceCaptured);
        }

        public void UnMakeMove(Move move) {
            // Swap color to move
            WhiteToMove = !WhiteToMove;
            bool undoingWhiteMove = WhiteToMove;

            // Move info
            int movedFrom = move.StartSquare;
            int movedTo = move.TargetSquare;
            int moveFlag = move.MoveFlag;
            bool undoingEnPassant = moveFlag == Move.EnPassantCaptureFlag;
            bool undoingPromotion = move.IsPromotion;
            bool undoingCapture = CurrentGameState.capturedPieceType != Piece.None;

            int movedPiece = undoingPromotion ? Piece.MakePiece(Piece.Pawn, MoveColor) : gameboard[movedTo];
            int movedPieceType = Piece.GetPieceType(movedPiece);
            int capturedPieceType = CurrentGameState.capturedPieceType;

            if (undoingPromotion) {
                int promotedPiece = gameboard[movedTo];
                int pawnPiece = Piece.MakePiece(Piece.Pawn, MoveColor);
                gameboard[promotedPiece] = pawnPiece; // replace promotion with pawn
            }

            MovePiece(movedPiece, movedTo, movedFrom);

            if (undoingCapture) {
                int captureSquare = movedTo;
                int capturedPiece = Piece.MakePiece(capturedPieceType, OpponentColor);

                if (undoingEnPassant) {
                    captureSquare = movedTo + ((undoingWhiteMove) ? -8 : 8);
                }
                gameboard[captureSquare] = capturedPiece;
            }

            if (movedPieceType is Piece.King) {
                if (moveFlag is Move.CastleFlag) {
                    int rookPiece = Piece.MakePiece(Piece.Rook, MoveColor);
                    bool kingside = movedTo == 6 || movedTo == 62; // g1 and g8
                    int rookSquareBeforeCatling = kingside ? movedTo + 1 : movedTo - 2;
                    int rookSquareAfterCastling = kingside ? movedTo - 1 : movedTo + 1;

                    // Undo castle
                    gameboard[rookSquareAfterCastling] = Piece.None;
                    gameboard[rookSquareBeforeCatling] = rookPiece;
                }
            }

            // return gamestate
            CurrentGameState = gameStateHistory.Pop();
            //CurrentGameState = gameStateHistory.Peek();
        }

        void MovePiece(int piece, int startSquare, int targetSquare) {
            gameboard[startSquare] = Piece.None;
            gameboard[targetSquare] = piece;
        }

        /// <summary>
        /// Get the piece at the location on the board.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Integer representing the piece on the board.</returns>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
        public int GetPiece(int location) {
            if (location >= 0 && location < 64) {
                return gameboard[location];
            } else {
                throw new BoardException("Invalid board coordinate: " + location + ".", this);
            }
        }
        /// <summary>
        /// Sets a piece in a specific location on the board.
        /// </summary>
        /// <param name="location">The location on the board.</param>
        /// <param name="piece">The piece to set.</param>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
        public void SetPiece(int location, int piece) {
            if (location >= 0 && location < 64) {
                gameboard[location] = piece;
            } else {
                throw new InvalidDataException();
            }
        }

        public void SetGameStateFromFen(bool whiteToMove, bool whiteKingCastleRight, bool whiteQueenCastleRight,
            bool blackKingCastleRight, bool blackQueenCastleRight, int enPassantTargetSquare, int halfMoveCounter, int fullMoveCounter) {
            WhiteToMove = whiteToMove;
            int castleRights =
                (whiteKingCastleRight ? GameState.WhiteKingsideFlag : 0) |
                (whiteQueenCastleRight ? GameState.WhiteQueensideFlag : 0) |
                (blackKingCastleRight ? GameState.BlackKingsideFlag : 0) |
                (blackQueenCastleRight ? GameState.BlackQueensideFlag : 0);

            gameStateHistory = new Stack<GameState>(); // clear gamestate.
            CurrentGameState = new GameState(enPassantTargetSquare, castleRights, halfMoveCounter, 0);
            gameStateHistory.Push(CurrentGameState);
        }


        public override string ToString() {
            return BoardUtils.GetDiagram(this);
        }
        // Static helpers.
        /// <summary>
        /// Gets the name of a square from an integer.
        /// </summary>
        /// <param name="coord">The location on the board.</param>
        /// <returns>A <see cref="string"/> representing a board square.</returns>



    }
}

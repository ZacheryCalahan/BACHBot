namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public class Board {
        // Public
        public int WhiteIndex = 0;
        public int BlackIndex = 1;
        public int MoveColor => WhiteToMove ? Piece.White : Piece.Black;
        public int OpponentColor => WhiteToMove ? Piece.Black : Piece.White;
        public int MoveColorIndex => WhiteToMove ? WhiteIndex : BlackIndex;
        public int OpponentColorIndex => WhiteToMove ? BlackIndex : WhiteIndex;

        public int[] gameboard { get; private set; }
        public bool WhiteToMove { get; private set; }
        public GameState CurrentGameState;
        public PieceList[] Pawns;
        public PieceList[] Knights;
        public PieceList[] Bishops;
        public PieceList[] Rooks;
        public PieceList[] Queens;
        public int[] KingSquare;

        // Private
        private Stack<GameState> gameStateHistory;
        PieceList[] allPieceLists;

        public Board() {
            gameboard = new int[64];
            gameStateHistory = new Stack<GameState>();
        }

        public void MakeMove(Move move) {

            // Useful values
            int pieceMoved = gameboard[move.StartSquare];
            int location = move.StartSquare;
            int colorMoved = Piece.GetPieceColor(pieceMoved);
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
                int pieceColor = Piece.GetPieceColor(pieceMoved);
                int direction = move.StartSquare - move.TargetSquare;
                
                if (pieceColor == Piece.White) {
                    if (direction > 0) {
                        // white queenside
                        rookOffset += 3;
                        rookLocation = 0;
                    } else {
                        // white kingside
                        rookOffset += -2;
                        rookLocation = 7;
                    }
                } else {
                    if (direction > 0) {
                        // black queenside
                        rookOffset += 3;
                        rookLocation = 56;
                    } else {
                        // black kingside
                        rookOffset += -2;
                        rookLocation = 63;
                    }
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

        public void PassTurn() {
            WhiteToMove = !WhiteToMove;
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
                throw new BoardException("Invalid board coordinate: " + location + ".", this);
            }
        }


        public void LoadFenPosition(string fen) {
            FenUtils.PositionInfo positionInfo = FenUtils.SetupBoardFromFen(fen);
            LoadFenPosition(positionInfo);
        }

        public void LoadFenPosition(FenUtils.PositionInfo positionInfo) {
            Initialize();

            // Gameboard
            for (int i = 0; i < 64; i++) {
                int piece = positionInfo.squares[i];
                int colorIndex = Piece.IsWhite(piece) ? WhiteIndex : BlackIndex;
                int pieceType = Piece.GetPieceType(piece);

                gameboard[i] = piece;

                if (piece != Piece.None) {
                    if (pieceType == Piece.King) {
                        KingSquare[colorIndex] = i;
                    } else {
                        allPieceLists[piece].AddPieceAtSquare(i);
                    }
                }
            }

            // Gamestate
            WhiteToMove = positionInfo.whiteToMove;
            int whiteCastle = ((positionInfo.whiteCastleKingside) ? 1 << 0 : 0) | ((positionInfo.whiteCastleQueenside) ? 1 << 1 : 0);
            int blackCastle = ((positionInfo.blackCastleKingside) ? 1 << 2 : 0) | ((positionInfo.blackCastleQueenside) ? 1 << 3 : 0);
            int castlingRights = whiteCastle | blackCastle;

            // Plycount here, don't need it at the moment though.
            CurrentGameState = new GameState(positionInfo.enPassantSquare, castlingRights, positionInfo.fiftyMoveCounter, 0);
            gameStateHistory.Push(CurrentGameState);
        }

        public void Initialize() {
            KingSquare = new int[2];
            Array.Clear(gameboard);

            gameStateHistory = new Stack<GameState>(capacity: 64);
            CurrentGameState = new GameState();

            Knights = new PieceList[] { new PieceList(10), new PieceList(10) };
            Pawns = new PieceList[] { new PieceList(8), new PieceList(8) };
            Rooks = new PieceList[] { new PieceList(10), new PieceList(10) };
            Bishops = new PieceList[] { new PieceList(10), new PieceList(10) };
            Queens = new PieceList[] { new PieceList(9), new PieceList(9) };

            allPieceLists = new PieceList[Piece.MaxPieceIndex + 1];
            allPieceLists[Piece.WhitePawn] = Pawns[WhiteIndex];
            allPieceLists[Piece.WhiteKnight] = Knights[WhiteIndex];
            allPieceLists[Piece.WhiteBishop] = Bishops[WhiteIndex];
            allPieceLists[Piece.WhiteRook] = Rooks[WhiteIndex];
            allPieceLists[Piece.WhiteQueen] = Queens[WhiteIndex];
            allPieceLists[Piece.WhiteKing] = new PieceList(1);

            allPieceLists[Piece.BlackPawn] = Pawns[BlackIndex];
            allPieceLists[Piece.BlackKnight] = Knights[BlackIndex];
            allPieceLists[Piece.BlackBishop] = Bishops[BlackIndex];
            allPieceLists[Piece.BlackRook] = Rooks[BlackIndex];
            allPieceLists[Piece.BlackQueen] = Queens[BlackIndex];
            allPieceLists[Piece.BlackKing] = new PieceList(1);
        }

        public override string ToString() {
            return BoardUtils.GetDiagram(this);
        }

        // Static Helpers
        public static bool IsEqual(Board a, Board b) {
            return a.gameboard == b.gameboard &&
                   GameState.IsEqual(a.CurrentGameState, b.CurrentGameState) &&
                   a.WhiteToMove == b.WhiteToMove &&
                   a.gameStateHistory == b.gameStateHistory;
        }



    }
}

namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public sealed class Board {
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
        public PieceList[] allPieceLists;

        // Private
        private Stack<GameState> gameStateHistory;
        
        public Board() {
            gameboard = new int[64];
            gameStateHistory = new Stack<GameState>();
        }

        public void MakeMove(Move move) {
            // Info about the move
            int moveFrom = move.StartSquare;
            int moveTo = move.TargetSquare;
            int capturedPieceType = Piece.GetPieceType(gameboard[moveTo]);
            int movedPiece = gameboard[moveFrom];
            int movedPieceType = Piece.GetPieceType(movedPiece);
            bool isPromotion = move.IsPromotion;
            bool isEnPassant = move.MoveFlag == Move.EnPassantCaptureFlag;

            // Gamestate variables (set to default values first, overwrite them later if necessary)
            int newCastlingRights = CurrentGameState.castlingRights;
            int newEnPassantSquare = (move.MoveFlag == Move.PawnDoublePush) ? move.TargetSquare : -1;
            int newFiftyMoveCounter = 0;

            // Handle Captures
            if (capturedPieceType != 0 && !isEnPassant) {
                GetPieceList(capturedPieceType, OpponentColorIndex).RemovePieceAtSquare(moveTo);
            }

            // Move Pieces in PieceLists
            if (movedPieceType == Piece.King) {
                KingSquare[MoveColorIndex] = moveTo;
                newCastlingRights &= (WhiteToMove) ? GameState.ClearWhiteCastle : GameState.ClearBlackCastle;
            } else {
                GetPieceList(movedPieceType, MoveColorIndex).MovePiece(moveFrom, moveTo);
            }

            int pieceOnTargetSquare = movedPiece;

            // Handle Promotions
            if (isPromotion) {
                int promoteType = 0;
                switch (move.MoveFlag) {
                    case Move.PromoteToQueenFlag:
                        promoteType = Piece.Queen;
                        Queens[MoveColorIndex].AddPieceAtSquare(moveTo);
                        break;
                    case Move.PromoteToKnightFlag:
                        promoteType = Piece.Knight;
                        Knights[MoveColorIndex].AddPieceAtSquare(moveTo);
                        break;
                    case Move.PromoteToRookFlag:
                        promoteType = Piece.Rook;
                        Rooks[MoveColorIndex].AddPieceAtSquare(moveTo);
                        break;
                    case Move.PromoteToBishopFlag:
                        promoteType = Piece.Bishop;
                        Bishops[MoveColorIndex].AddPieceAtSquare(moveTo);
                        break;
                }
                pieceOnTargetSquare = promoteType | MoveColor;
                Pawns[MoveColorIndex].RemovePieceAtSquare(moveTo);
            } else {
                switch (move.MoveFlag) {
                    case Move.EnPassantCaptureFlag:
                        int enPassantSquare = CurrentGameState.enPassantSquare;
                        capturedPieceType = gameboard[enPassantSquare];
                        gameboard[enPassantSquare] = 0; // Clear enpassant pawn
                        Pawns[OpponentColorIndex].RemovePieceAtSquare(enPassantSquare);
                        break;
                    case Move.CastleFlag:
                        bool kingside = moveTo == 6 || moveTo == 62; // g1 and g8
                        int castlingRookFromIndex = (kingside) ? moveTo + 1 : moveTo - 2;
                        int castlingRookToIndex = (kingside) ? moveTo - 1 : moveTo + 1;
                        gameboard[castlingRookFromIndex] = Piece.None;
                        gameboard[castlingRookToIndex] = Piece.Rook | MoveColor;
                        Rooks[MoveColorIndex].MovePiece(castlingRookFromIndex, castlingRookToIndex);
                        break;
                }
            }

            // Move piece
            gameboard[moveTo] = pieceOnTargetSquare;
            gameboard[moveFrom] = 0;

            // Calculate castle rights in the case of Rook movement
            if (movedPieceType == Piece.Rook) {
                if (MoveColor == Piece.White) {
                    if (move.StartSquare == 0) {
                        newCastlingRights &= GameState.ClearWhiteQueensideMask;
                    } else if (move.StartSquare == 7) {
                        newCastlingRights &= GameState.ClearWhiteKingsideMask;
                    }
                } else {
                    if (move.StartSquare == 56) {
                        newCastlingRights &= GameState.ClearBlackQueensideMask;
                    } else if (move.StartSquare == 63) {
                        newCastlingRights &= GameState.ClearBlackKingsideMask;
                    }
                }
            }

            if (capturedPieceType != 0 || movedPieceType == Piece.Pawn) {
                newFiftyMoveCounter = 0;
            } else {
                if (MoveColor == Piece.Black) {
                    newFiftyMoveCounter += 1;
                }
            }

            WhiteToMove = !WhiteToMove;
            CurrentGameState = new GameState(newEnPassantSquare, newCastlingRights, newFiftyMoveCounter, capturedPieceType);
            gameStateHistory.Push(CurrentGameState);
        }

        public void UnMakeMove(Move move) { 
            // Swap color to move
            WhiteToMove = !WhiteToMove;
            bool undoingWhiteMove = WhiteToMove;

            // Move Info
            int movedFrom = move.StartSquare;
            int movedTo = move.TargetSquare;
            int moveFlag = move.MoveFlag;
            bool undoingEnPassant = moveFlag == Move.EnPassantCaptureFlag;
            bool undoingPromotion = move.IsPromotion;
            bool undoingCapture = CurrentGameState.capturedPieceType != Piece.None;
            int movedPiece = undoingPromotion ? Piece.MakePiece(Piece.Pawn, MoveColor) : gameboard[movedTo];
            int movedPieceType = Piece.GetPieceType(movedPiece);
            int lastCapturedPieceType = CurrentGameState.capturedPieceType;

            if (undoingPromotion) {
                int promotedPawn = gameboard[movedTo];
                int pawnPiece = Piece.MakePiece(Piece.Pawn, MoveColor);
                gameboard[movedTo] = pawnPiece;
                allPieceLists[promotedPawn].RemovePieceAtSquare(movedTo);
                allPieceLists[movedPiece].AddPieceAtSquare(movedTo);
            }

            MovePiece(movedPiece, movedTo, movedFrom);

            if (undoingCapture) {
                int captureSquare = movedTo;
                int capturedPiece = Piece.MakePiece(lastCapturedPieceType, OpponentColor);

                if (undoingEnPassant) {
                    captureSquare = movedTo + ((undoingWhiteMove) ? -8 : 8);
                }
                allPieceLists[capturedPiece].AddPieceAtSquare(captureSquare);
                gameboard[captureSquare] = capturedPiece;
            }

            if (movedPieceType is Piece.King) {
                KingSquare[MoveColorIndex] = movedFrom;

                if (moveFlag is Move.CastleFlag) {
                    int rookPiece = Piece.MakePiece(Piece.Rook, MoveColor);
                    bool kingside = movedTo == 6 || movedTo == 62; // g1 and g8
                    int rookSquareBeforeCastling = kingside ? movedTo + 1 : movedTo - 2;
                    int rookSquareAfterCastling = kingside ? movedTo - 1 : movedTo + 1;

                    // Return rook
                    gameboard[rookSquareAfterCastling] = Piece.None;
                    gameboard[rookSquareBeforeCastling] = rookPiece;
                    allPieceLists[rookPiece].MovePiece(rookSquareAfterCastling, rookSquareBeforeCastling);
                }
            }

            gameStateHistory.Pop();
            CurrentGameState = gameStateHistory.Peek();
        }

        PieceList GetPieceList (int pieceType, int colorIndex) {
            return allPieceLists[colorIndex * 8 + pieceType];
        }

        public void PassTurn() {
            WhiteToMove = !WhiteToMove;
        }

        void MovePiece(int piece, int startSquare, int targetSquare) {
            gameboard[startSquare] = Piece.None;
            gameboard[targetSquare] = piece;
            GetPieceList(Piece.GetPieceType(piece), MoveColorIndex).MovePiece(startSquare, targetSquare);
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

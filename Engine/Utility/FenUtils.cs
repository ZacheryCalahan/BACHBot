namespace caZsChessBot.Engine {
    public static class FenUtils { 

        public const string startPosFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        /// <summary>
        /// Manipulates the <see cref="Board"/> to set up the gamestate and position of any given FEN string.
        /// </summary>
        /// <param name="board">The board to manipulate.</param>
        /// <param name="fen">The FEN to set the board up to.</param>
        public static void SetupBoardFromFen(Board board, string fen) {
            string[] fenTokens = fen.Split(' ');
            string fenBoard = fenTokens[0];

            // Decode the position from the FEN
            int file = 0, rank = 7;

            // Clear board
            for (int i = 0; i < 8; i++) {
                for (int j = 7; j >= 0; j--) {
                    board.SetPiece(i * 8 + j, 0);
                }
            }

            // Write board position
            foreach (char symbol in fenBoard) {
                if (symbol == '/') {
                    file = 0;
                    rank--;
                } else {
                    if (char.IsDigit(symbol)) {
                        file += (int)char.GetNumericValue(symbol);
                    } else {
                        board.SetPiece(rank * 8 + file, Piece.GetPieceFromLetter(symbol));
                        file++;
                    }
                }
            }

            // Decode gamestate from FEN
            bool whiteToMove = (fenTokens[1] == "w");
            bool whiteKingCastleRight = fenTokens[2].Contains("K");
            bool whiteQueenCastleRight = fenTokens[2].Contains("Q");
            bool blackKingCastleRight = fenTokens[2].Contains("k");
            bool blackQueenCastleRight = fenTokens[2].Contains("q");
            int enPassantTargetSquare = fenTokens[3] != "-" ? BoardUtils.GetSquareCoordFromName(fenTokens[3]) : -1; // Returns -1 for no enpassant
            int halfMoveCounter = int.Parse(fenTokens[4]);
            int fullMoveCounter = int.Parse(fenTokens[5]);

            // Write gamestate
            board.SetGameStateFromFen(); // TODO
        }

        public readonly struct PositionInfo {

        }
    }
}

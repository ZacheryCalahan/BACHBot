namespace caZsChessBot.Engine {
    public static class BoardUtils {
        public static void PrintDiagram(Board board) {
            Console.WriteLine(". a . b . c . d . e . f . g . h .");
            for (int i = 7; i >= 0; i--) {
                Console.WriteLine("|---|---|---|---|---|---|---|---|-");
                for (int j = 0; j < 8; j++) { 
                    Console.Write("| " + Piece.GetPieceLetter(board.GetPiece((i * 8) + j)) + " ");
                }
                Console.WriteLine("| " + (i + 1));
            }

            Console.WriteLine("|---|---|---|---|---|---|---|---|-");
        }

        public static string GetDiagram(Board board) {
            string diagram = "";
            diagram += (". a . b . c . d . e . f . g . h .\n");
            for (int i = 7; i >= 0; i--) {
                diagram += ("|---|---|---|---|---|---|---|---|-\n");
                for (int j = 0; j < 8; j++) {
                    diagram += ("| " + Piece.GetPieceLetter(board.GetPiece((i * 8) + j)) + " ");
                }
                diagram += ("| " + (i + 1) + "\n");
            }

            diagram += ("|---|---|---|---|---|---|---|---|-\n");
            return diagram;
        }

        public static void PrintMoves(Board board) {
            List<Move> moves = MoveGeneration.GeneratePsuedoLegalMoves(board);
            if (moves.Count == 0) {
                Console.WriteLine("No moves found.");
                return;
            }
            
            // Print each move.
            foreach (Move move in moves) {
                // Handle promotion data
                string promotion;
                switch (move.MoveFlag) {
                    case Move.PromoteToBishopFlag:
                        promotion = "b";
                        break;
                    case Move.PromoteToKnightFlag:
                        promotion = "n";
                        break;
                    case Move.PromoteToRookFlag: 
                        promotion = "r";
                        break;
                    case Move.PromoteToQueenFlag: 
                        promotion = "q";
                        break;
                    case Move.CastleFlag: // yeah i know, doesn't really fit the names, give me a break and I'll fix it.
                        promotion = "castle";
                        break;
                    default:
                        promotion = "";
                        break;
                }


                    Console.WriteLine(BoardUtils.GetSquareNameFromCoord(move.StartSquare) +
                    BoardUtils.GetSquareNameFromCoord(move.TargetSquare) + promotion);
            }
            Console.WriteLine((board.WhiteToMove ? "White " : "Black ") + "to move.");
            Console.WriteLine("Numbers of moves found: " + moves.Count);
        }

        public static void PrintMove(Move move) {
            // Handle promotion data
            string promotion;
            switch (move.MoveFlag) {
                case Move.PromoteToBishopFlag:
                    promotion = "b";
                    break;
                case Move.PromoteToKnightFlag:
                    promotion = "n";
                    break;
                case Move.PromoteToRookFlag:
                    promotion = "r";
                    break;
                case Move.PromoteToQueenFlag:
                    promotion = "q";
                    break;
                case Move.CastleFlag: // yeah i know, doesn't really fit the names, give me a break and I'll fix it.
                    promotion = "castle";
                    break;
                default:
                    promotion = "";
                    break;
            }


            Console.WriteLine(BoardUtils.GetSquareNameFromCoord(move.StartSquare) +
            BoardUtils.GetSquareNameFromCoord(move.TargetSquare) + promotion);
        }
        /// <summary>
        /// Gets the name of a square by a coord.
        /// </summary>
        /// <param name="coord">The <see cref="int"/> location of the board.</param>
        /// <returns>A <see cref="string"/> representing the square name.</returns>
        public static string GetSquareNameFromCoord(int coord) {
            string coordName = "";
            // Get the file name
            switch (coord % 8) {
                case 0:
                    coordName = "a";
                    break;
                case 1:
                    coordName = "b";
                    break;
                case 2:
                    coordName = "c";
                    break;
                case 3:
                    coordName = "d";
                    break;
                case 4:
                    coordName = "e";
                    break;
                case 5:
                    coordName = "f";
                    break;
                case 6:
                    coordName = "g";
                    break;
                case 7:
                    coordName = "h";
                    break;
                default:
                    break;
            }

            // Get rank name
            coordName = coordName + ((coord / 8) + 1);
            return coordName;
        }
        /// <summary>
        /// Gets the coord of a square on the board by its name.
        /// </summary>
        /// <param name="name">The name (algebraic notation) of the coordinate.</param>
        /// <returns>An <see cref="int"/> representing the coordinate on the board.</returns>
        public static int GetSquareCoordFromName(string name) {
            int coord = 0;
            switch (name[0]) {
                case 'a':
                    coord = 0;
                    break;
                case 'b':
                    coord = 1;
                    break;
                case 'c':
                    coord = 2;
                    break;
                case 'd':
                    coord = 3;
                    break;
                case 'e':
                    coord = 4;
                    break;
                case 'f':
                    coord = 5;
                    break;
                case 'g':
                    coord = 6;
                    break;
                case 'h':
                    coord = 7;
                    break;
                default:
                    break;
            }
            return coord + (int.Parse("" + name[1]) - 1) * 8;
        }

        public static int RankIndex(int square) {
            return square >> 3;
        }

        public static int FileIndex(int square) {
            return square & 0b000111;
        }
    }
}

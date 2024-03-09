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


                    Console.WriteLine(Board.GetSquareNameFromCoord(move.StartSquare) +
                    Board.GetSquareNameFromCoord(move.TargetSquare) + promotion);
            }
            Console.WriteLine((board.WhiteToMove ? "White " : "Black ") + "to move.");
            Console.WriteLine("Numbers of moves found: " + moves.Count);
        }

        
    }
}

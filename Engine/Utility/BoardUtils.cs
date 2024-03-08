namespace caZsChessBot.Engine {
    public static class BoardUtils {
        public static void PrintDiagram(Board board) {
            for (int i = 7; i >= 0; i--) {
                Console.WriteLine("|---|---|---|---|---|---|---|---|");
                for (int j = 0; j < 8; j++) { 
                    Console.Write("| " + Piece.GetPieceLetter(board.GetPiece((i * 8) + j)) + " ");
                }
                Console.WriteLine("|");
            }

            Console.WriteLine("|---|---|---|---|---|---|---|---|");
        }

        public static void PrintMoves(Board board) {
            List<Move> moves = MoveGeneration.GeneratePsuedoLegalMoves(board);
            if (moves.Count == 0) {
                Console.WriteLine("No moves found.");
                return;
            }
            foreach (Move move in moves) {
                Console.WriteLine(Board.GetSquareNameFromCoord(move.StartSquare) + Board.GetSquareNameFromCoord(move.TargetSquare));
            }
            Console.WriteLine((board.WhiteToMove ? "White " : "Black ") + "to move.");
            Console.WriteLine("Numbers of moves found: " + moves.Count);
        }

        
    }
}

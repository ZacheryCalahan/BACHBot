namespace caZsChessBot.Engine {
    public static class PerftUtils {
        public static void GetPerftResults(Board board, int depthToSearch) {
            int numPositions = PerftTest(board, depthToSearch);
            Program.SendDebugInfo(numPositions + " positions found.");
        }
        
        private static int PerftTest(Board board, int depth) {
            if (depth == 0)
                return 1;
            List<Move> moves = MoveGeneration.GenerateLegalMoves(board);
            int numPositions = 0;
            foreach (Move move in moves) {
                board.MakeMove(move);
                numPositions += PerftTest(board, depth - 1);
                board.UnMakeMove(move);
            }
            return numPositions;
        }
    }
}

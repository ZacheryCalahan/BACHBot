namespace caZsChessBot.Engine {
    public class PerftUtils {
        public void GetPerftResults(Board board, int depthToSearch) {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            PerftTestDivide(board, depthToSearch);
            Program.SendDebugInfo("Time searched: " + stopwatch.ElapsedMilliseconds + "ms.");
            stopwatch.Stop();
        }
        
        private int PerftTest(Board board, int depth) {
            if (depth == 0) {
                return 1;
            }
                
            List<Move> moves = MoveGeneration.GenerateMoves(board);
            int numPositions = 0;
            foreach (Move move in moves) {
                board.MakeMove(move);
                numPositions += PerftTest(board, depth - 1);
                board.UnMakeMove(move);
            }
            return numPositions;
        }

        private void PerftTestDivide(Board board, int depth) {
            if (depth == 0) {
                Program.SendDebugInfo("Depth 0 returns 1 move.", true);
                return;
            }
            int totalMoves = 0;
            List<Move> moves = MoveGeneration.GenerateMoves(board);

            foreach (Move move in moves) {
                board.MakeMove(move);
                int movesCounted = PerftTest(board, depth - 1);
                Program.SendDebugInfo(move.ToString().Substring(0,4) + ": " + movesCounted);
                board.UnMakeMove(move);
                totalMoves += movesCounted;
            }
            Program.SendDebugInfo("Nodes searched: " + totalMoves.ToString());
        }
    }
}

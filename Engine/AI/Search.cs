namespace caZsChessBot.Engine {
    public static class Search {
        public static int bestEval;
        public static Move bestMove;
        public static int initDepth;

        public static Move GetBestMove(Board board, int depth) {
            initDepth = depth;
            SearchBestMove(board, depth, int.MinValue, int.MaxValue);
            return bestMove;
        }

        public static int GetBestEval() {
            return bestEval;
        }

        public static int SearchBestMove(Board board, int depth, int alpha, int beta) {
            if (depth == 0) {
                return Evaluate.GetEvaluation(board);
            }

            List<Move> moves = MoveGeneration.GenerateMoves(board);
            if (depth == initDepth) {
                // this seems weird, but it's my start game strat ig.
                Random random = new Random();
                bestMove = moves[random.Next(moves.Count - 1)];
            }

            
            if (moves.Count == 0) { // technically tries not to draw. Will be changed later, but this may depend on bitboards and zobrist hashing.
                return int.MinValue;
            }

            foreach (Move move in moves) {
                board.MakeMove(move);
                int eval = -SearchBestMove(board, depth - 1, -beta, -alpha);
                board.UnMakeMove(move);

                if (eval >= beta) {
                    return beta; // Sebastian Lague says: *Snip*
                }

                if (eval > alpha) { // Found best move!
                    bestMove = move;
                }
                alpha = Math.Max(alpha, eval);
            }

            return alpha;
        }
    }
}

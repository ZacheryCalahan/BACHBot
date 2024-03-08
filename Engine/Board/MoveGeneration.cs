namespace caZsChessBot.Engine {
    public static class MoveGeneration {
        // Offsets
        static readonly int[] orthogonalOffsets = { 8, -1, 1, -8 };
        static readonly int[] diagonalOffsets = { 7, 9, -9, -7 };
        static readonly int[] knightOffsets = { 15, 17, 6, 10, -10, -6, -17, -15 };

        // Ranks and Files for computation
        static readonly int[] rankA = { 0, 8, 16, 24, 32, 40, 48, 56 };
        static readonly int[] rankB = { 1, 9, 17, 25, 33, 41, 49, 57 };
        static readonly int[] rankG = { 6, 14, 22, 30, 38, 46, 54, 62 };
        static readonly int[] rankH = { 7, 15, 23, 31, 39, 47, 55, 63 };
        static readonly int[] file8 = { 56, 57, 58, 59, 60, 61, 62, 63 };
        static readonly int[] file7 = { 48, 49, 50, 51, 52, 53, 54, 55 };
        static readonly int[] file2 = { 8, 9, 10, 11, 12, 13, 14, 15 };
        static readonly int[] file1 = { 0, 1, 2, 3, 4, 5, 6, 7 };

        public static List<Move> GeneratePsuedoLegalMoves(Board board) {
            int colorToMove = board.WhiteToMove ? Piece.White : Piece.Black;
            List<Move> psuedoLegalMoves = new List<Move>();
         
            return psuedoLegalMoves;
        }

        private static List<Move> GenerateSlidingMoves(Board board, int location) {
            List<Move> slidingMoves = new List<Move>();
            int currentPiece = board.GetPiece(location);
            


            return slidingMoves;
        }

        private static List<Move> GenerateKingMoves(Board board, int piece) {
            List<Move> kingMoves = new List<Move>();



            return kingMoves;
        }

        private static List<Move> GeneratePawnMoves(Board board, int piece) {
            List<Move> pawnMoves = new List<Move>();



            return pawnMoves;
        }

        private static List<Move> GenerateKnightMoves(Board board, int piece) { 
            List<Move> knightMoves = new List<Move>();



            return knightMoves;
        }
    }
}

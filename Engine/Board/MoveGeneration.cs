using System.ComponentModel;

namespace caZsChessBot.Engine {
    public static class MoveGeneration {

        public static List<Move> GenerateMoves(Board board) {
            List<Move> legalMoves = new List<Move>();
            
            return legalMoves;
        }

        private static List<Move> GenerateSlidingMoves(Board board, int location) {
            List<Move> slidingMoves = new List<Move>();
            
            return slidingMoves;
        }

        private static List<Move> GenerateKingMoves(Board board, int location) {
            // Handle the psuedo move generation.
            List<Move> kingMoves = new List<Move>();

            return kingMoves;
        }

        private static List<Move> GenerateKingCastleMoves(Board board, int location, ulong attackedBitboard) {
            List<Move> castleMoves = new List<Move>();
            
            return castleMoves;
        }

        private static List<Move> GeneratePawnMoves(Board board, int location) {
            List<Move> pawnMoves = new List<Move>();

            return pawnMoves;
        }

        private static List<Move> GenerateKnightMoves(Board board, int location) {
            List<Move> knightMoves = new List<Move>();

            return knightMoves;
        }

        private static void VerifyBoardIntegrity(Board startBoard, Board endBoard) {
            if (Board.IsEqual(startBoard, endBoard)) {
                return;
            } else {
                Program.SendDebugInfo("Board malformed while generating moves. Original board:", true);
                BoardUtils.PrintDiagram(startBoard);
                BoardUtils.PrintMoves(startBoard);
                Program.SendDebugInfo("Board after move generation:", true);
                BoardUtils.PrintDiagram(endBoard);
                BoardUtils.PrintMoves(endBoard);
                throw new BoardException("Board malformity.", startBoard);
            }
        }
    }
}

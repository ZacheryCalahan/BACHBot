namespace caZsChessBot.Engine {
    public class MoveOrdering {

        const int capturedPieceValueMultiplier = 10;

        public static List<Move> OrderMoves(Board board, List<Move> moves) {
            List<Move> result = new List<Move>();
            int[] moveScores = new int[moves.Count];

            for (int i = 0; i < moves.Count; i++) {
                int score = 0;
                int movePieceType = Piece.GetPieceType(board.GetPiece(moves[i].StartSquare));
                int capturePieceType = Piece.GetPieceType(board.GetPiece(moves[i].TargetSquare));
                int flag = moves[i].MoveFlag;

                if (capturePieceType != Piece.None) {
                    score = capturedPieceValueMultiplier * GetPieceValue(capturePieceType) - GetPieceValue(movePieceType);
                }

                if (movePieceType == Piece.Pawn) {
                    if (flag == Move.PromoteToQueenFlag) {
                        score += Evaluate.queenValue;
                    } else if (flag == Move.PromoteToKnightFlag) {
                        score += Evaluate.knightValue;
                    } else if (flag == Move.PromoteToRookFlag) {
                        score += Evaluate.rookValue;
                    } else if (flag == Move.PromoteToBishopFlag) {
                        score += Evaluate.bishopValue;
                    }
                } // good place to check attacked squares.

                moveScores[i] = score;
            }
            return SortMoves(moves, moveScores);

        }

        static List<Move> SortMoves(List<Move> moves, int[] moveScores) {
            // Sort the moves list based on scores (I personally do not understand this yet, so I borrowed it from Seb Lague. yeah i know.)
            for (int i = 0; i < moves.Count - 1; i++) {
                for (int j = i + 1; j > 0; j--) {
                    int swapIndex = j - 1;
                    if (moveScores[swapIndex] < moveScores[j]) {
                        (moves[j], moves[swapIndex]) = (moves[swapIndex], moves[j]);
                        (moveScores[j], moveScores[swapIndex]) = (moveScores[swapIndex], moveScores[j]);
                    }
                }
            }
            return moves;
        }

        static int GetPieceValue(int pieceType) {
            switch (pieceType) {
                case Piece.Queen:
                    return Evaluate.queenValue;
                case Piece.Rook:
                    return Evaluate.rookValue;
                case Piece.Knight:
                    return Evaluate.knightValue;
                case Piece.Bishop:
                    return Evaluate.bishopValue;
                case Piece.Pawn:
                    return Evaluate.pawnValue;
                default:
                    return 0;
            }
        }

    }
}

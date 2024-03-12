using System.Security.Cryptography.X509Certificates;

namespace caZsChessBot.Engine {
    public static class MoveUtil {
        public static Move GetMoveFromUCIName(string moveName, Board board) {
            int startSquare = BoardUtils.GetSquareCoordFromName(moveName.Substring(0, 2));
            int targetSquare = BoardUtils.GetSquareCoordFromName(moveName.Substring(2, 2));

            int movedPieceType = Piece.GetPieceType(board.GetPiece(startSquare));

            // Determine move flag
            int moveFlag = Move.NoFlag;
            if (movedPieceType == Piece.Pawn) {
                
                if (moveName.Length > 4) {
                    // Handle promotions
                    moveFlag = moveName[4] switch {
                        'q' => Move.PromoteToQueenFlag,
                        'r' => Move.PromoteToRookFlag,
                        'n' => Move.PromoteToKnightFlag,
                        'b' => Move.PromoteToBishopFlag,
                        _ => Move.NoFlag
                    };
                } else if (Math.Abs(BoardUtils.RankIndex(targetSquare) - BoardUtils.RankIndex(startSquare)) == 2) {
                    // Handle double pawn pushes
                    moveFlag = Move.PawnDoublePush;
                } else if (BoardUtils.FileIndex(startSquare) != BoardUtils.FileIndex(targetSquare) && 
                           board.GetPiece(targetSquare) == Piece.None) {
                    // Handle enpassant captures
                    moveFlag = Move.EnPassantCaptureFlag;
                }
            } else if (movedPieceType == Piece.King) {
                if (Math.Abs((startSquare / 8) - (targetSquare / 8)) > 1) {
                    moveFlag = Move.CastleFlag;
                }
            }

            return new Move(startSquare, targetSquare, moveFlag);
        }

        public static bool IsMoveLegal(Move move, Board board) {
            List<Move> legalMoves = MoveGeneration.GenerateLegalMoves(board);
            return legalMoves.Contains(move);
        }

        public static bool IsMoveLegal(string move, Board board) {
            return IsMoveLegal(GetMoveFromUCIName(move, board), board);
        }
    }

}

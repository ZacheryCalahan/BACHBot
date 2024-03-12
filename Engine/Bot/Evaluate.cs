namespace caZsChessBot.Engine {
    public class Evaluate {
        const int pawnValue = 100;
        const int knightValue = 300;
        const int bishopValue = 310;
        const int rookValue = 500;
        const int queenValue = 900;

        public static int GetEvaluation(Board board) {
            int[] materials = CountMaterial(board);

            int eval = materials[0] - materials[1];
            return eval * (board.WhiteToMove ? 1 : -1);
        }

        static int[] CountMaterial(Board board) {
            int[] materials = { 0, 0 };
            int pieceColor;
            int pieceType;
            int colorToAdd;

            for (int i = 0; i < 64; i++) {
                pieceColor = Piece.GetPieceColor(board.GetPiece(i));
                pieceType = Piece.GetPieceType(board.GetPiece(i));

                if (pieceType != 0) { // verify that piece exists
                    colorToAdd = pieceColor == Piece.White ? 0 : 1;
                    switch (pieceType) {
                        case Piece.Pawn:
                            materials[colorToAdd] += pawnValue;
                            break;
                        case Piece.Knight:
                            materials[colorToAdd] += knightValue;
                            break;
                        case Piece.Bishop:
                            materials[colorToAdd] += bishopValue;
                            break;
                        case Piece.Rook:
                            materials[colorToAdd] += rookValue;
                            break;
                        case Piece.Queen:
                            materials[colorToAdd] += queenValue;
                            break;
                        case Piece.King:
                            break;
                        default:
                            Program.SendDebugInfo("Attempt to get piece value of Piece.None", true);
                            break;
                    }
                }
            }
            return materials;
        }

    }
}

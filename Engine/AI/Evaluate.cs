namespace caZsChessBot.Engine {
    public class Evaluate {
        public const int pawnValue = 100;
        public const int knightValue = 300;
        public const int bishopValue = 310;
        public const int rookValue = 500;
        public const int queenValue = 900;

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
                            materials[colorToAdd] += (pawnValue * WeightTables.GetWeight(Piece.Pawn, i, pieceColor));
                            break;
                        case Piece.Knight:
                            materials[colorToAdd] += knightValue * WeightTables.GetWeight(Piece.Knight, i, pieceColor);
                            break;
                        case Piece.Bishop:
                            materials[colorToAdd] += bishopValue * WeightTables.GetWeight(Piece.Bishop, i, pieceColor);
                            break;
                        case Piece.Rook:
                            materials[colorToAdd] += rookValue * WeightTables.GetWeight(Piece.Rook, i, pieceColor);
                            break;
                        case Piece.Queen:
                            materials[colorToAdd] += queenValue * WeightTables.GetWeight(Piece.Queen, i, pieceColor);
                            break;
                        case Piece.King: // no need to worry about this yet, but this will be important later.
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

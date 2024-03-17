namespace caZsChessBot.Engine { // Seb Lague implementation.
    public class PieceList {
        public int[] occupiedSquares;
        int[] map;
        int numPieces;

        public PieceList(int maxPieceCount = 16) {
            occupiedSquares = new int[maxPieceCount];
            map = new int[64];
            numPieces = 0;
        }

        public int Count {
            get {
                return numPieces;
            }
        }

        public void AddPieceAtSquare(int location) {
            occupiedSquares[numPieces] = location;
            map[location] = numPieces;
            numPieces++;
        }

        public void RemovePieceAtSquare(int location) {
            int pieceIndex = map[location];
            occupiedSquares[pieceIndex] = occupiedSquares[numPieces - 1];
            map[occupiedSquares[pieceIndex]] = pieceIndex;
            numPieces--;
        }

        public void MovePiece(int startSquare, int targetSquare) {
            int pieceIndex = map[startSquare];
            occupiedSquares[pieceIndex] = targetSquare;
            map[targetSquare] = pieceIndex;
        }

        public int this[int index] => occupiedSquares[index];
    }
}

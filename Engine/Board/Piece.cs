namespace caZsChessBot.Engine {

    /// <summary>Class <c>Piece</c> holds helper utilities and definitions of game pieces. See class <seealso cref="Board"/> for usage.</summary>
    public static class Piece {
        // Piece Type
        public const int Pawn = 1;      // Special movement
        public const int Knight = 2;    // Special movement
        public const int King = 3;      // Diag & Orth
        public const int Bishop = 4;    // Diagonal Slider
        public const int Rook = 5;      // Orthogonal Slider
        public const int Queen = 6;     // Diag & Orth Slider

        // Color
        public const int White = 8;
        public const int Black = 16;

        // Flags
        public const int Moved = 32; // Used for king and rooks in determining castling rights.

        // Bit Masks
        public const int PieceTypeMask = 7; // 0b00111
        public const int ColorMask = 24; // 0b11000

        // Methods
        /// <summary>
        /// Gets the piece, stripped of its color.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>Integer representing the piece type.</returns>
        public static int GetPieceType(int piece) { return piece & PieceTypeMask; }
        /// <summary>
        /// Checks if <c>piece</c> is of type <c>pieceType</c>.
        /// </summary>
        /// <param name="piece">Piece to compare</param>
        /// <param name="pieceType">Piece type to compare to</param>
        public static bool IsPieceType(int piece, int pieceType) { return (GetPieceType(piece) & pieceType) == pieceType ? true : false; }
        /// <summary>
        /// Get the color of <c>piece</c>.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>Integer representing the color of <c>piece</c>.</returns>
        public static int GetPieceColor(int piece) { return piece & ColorMask; } // Admitedly redundant, but may make code easier to read later.
        /// <summary>
        /// Check if <c>piece</c> is of color <see cref="White"/>.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsWhite(int piece) {  return (piece & White) == White ? true : false; }
        /// <summary>
        /// Check if <c>piece</c> is capable of moving more than one space at a time in any straight direction.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsSlidingPiece(int piece) { return GetPieceType(piece) >= 4 ? true : false; }
        /// <summary>
        /// Check if <c>piece</c> moves horizontally or vertically.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsOrthogonalPiece(int piece) { return (GetPieceType(piece) >= 5) || (IsPieceType(piece, King)) ? true : false; }
        /// <summary>
        /// Check if <c>piece</c> moves diagonally.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsDiagonalPiece(int piece) {
            piece = (GetPieceType(piece));
            if (piece == 3 || piece == 4 || piece == 6) {
                return true;
            }
            return false;
        }
    }

    
}

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
        /// <returns>An <see cref="int"/> representing the piece type.</returns>
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
        /// <returns>An <see cref="int"/> representing the color of <c>piece</c>.</returns>
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
        public static bool IsSlidingPiece(int piece) { return GetPieceType(piece) >= 4; }
        /// <summary>
        /// Check if <c>piece</c> moves horizontally or vertically.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsOrthogonalPiece(int piece) { return (GetPieceType(piece) >= 5) || IsPieceType(piece, King); }
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
        /// <summary>
        /// Get the letter corresponding to a piece.
        /// </summary>
        /// <param name="piece">The piece to decode the letter from.</param>
        /// <returns>A <see cref="Char"/> representing the given piece.</returns>
        public static char GetPieceLetter(int piece) {
            char pieceLetter;
            switch (GetPieceType(piece)) {
                case (Pawn):
                    pieceLetter = 'p';
                    break;
                case (Knight):
                    pieceLetter = 'n';
                    break;
                case (Bishop):
                    pieceLetter = 'b';
                    break;
                case (Rook):
                    pieceLetter = 'r';
                    break;
                case (Queen):
                    pieceLetter = 'q';
                    break;
                case (King):
                    pieceLetter = 'k';
                    break;
                default:
                    return ' '; // No piece :D
            }

            if (IsWhite(piece)) {
                return Char.ToUpper(pieceLetter);
            } else {
                return pieceLetter;
            }
        }
        /// <summary>
        /// Gets the piece corresponding to a letter.
        /// </summary>
        /// <param name="pieceLetter">The Char representing the given piece.</param>
        /// <returns>An <see cref="int"/> representation of a piece.</returns>
        public static int GetPieceFromLetter(char pieceLetter) {
            int piece = 0;

            if (Char.IsUpper(pieceLetter)) {
                piece |= White;
            } else {
                piece |= Black;
            }

            pieceLetter = Char.ToLower(pieceLetter);

            switch (pieceLetter) {
                case ('p'):
                    piece |= Pawn;
                    break;
                case ('n'):
                    piece |= Knight;
                    break;
                case ('b'):
                    piece |= Bishop;
                    break;
                case ('r'):
                    piece |= Rook;
                    break;
                case ('q'):
                    piece |= Queen;
                    break;
                case ('k'):
                    piece |= King;
                    break;
                default:
                    return 0; // No piece :D
            }

            return piece;
        }
        /// <summary>
        /// Check if two pieces are of the same color.
        /// </summary>
        /// <param name="piece1"></param>
        /// <param name="piece2"></param>
        /// <returns>A <see cref="bool"/> representing the piece color equality.</returns>
        public static bool IsColor(int piece1, int piece2) { return (GetPieceColor(piece1) == GetPieceColor(piece2)); }
        public static int GetOpponentColor(int piece) {
            if (GetPieceColor(piece) == White)
                return Black;
            else
                return White;
        }
        public static bool IsNull(int piece) { return piece == 0; }
    }

    
}

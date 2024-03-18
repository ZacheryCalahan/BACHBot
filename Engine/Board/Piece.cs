using caZsChessBot.Engine;
using System.Net.NetworkInformation;

namespace caZsChessBot.Engine {

    /// <summary>Class <c>Piece</c> holds helper utilities and definitions of game pieces. See class <seealso cref="Board"/> for usage.</summary>
    public static class Piece {
        // Piece Type
        public const int None = 0;
        public const int Pawn = 1;      // Special movement
        public const int Knight = 2;    // Special movement
        public const int King = 3;      // Diag & Orth
        public const int Bishop = 4;    // Diagonal Slider
        public const int Rook = 5;      // Orthogonal Slider
        public const int Queen = 6;     // Diag & Orth Slider

        // Color
        public const int White = 0;
        public const int Black = 8;

        // Pieces
        public const int WhitePawn = Pawn | White;      // 1
        public const int WhiteKnight = Knight | White;  // 2
        public const int WhiteKing = King | White;      // 3
        public const int WhiteBishop = Bishop | White;  // 4
        public const int WhiteRook = Rook | White;      // 5
        public const int WhiteQueen = Queen | White;    // 6
        
        public const int BlackPawn = Pawn | Black;      // 9
        public const int BlackKnight = Knight | Black;  // 10
        public const int BlackKing = King | Black;      // 11
        public const int BlackBishop = Bishop | Black;  // 12
        public const int BlackRook = Rook | Black;      // 13
        public const int BlackQueen = Queen | Black;    // 14

        public const int MaxPieceIndex = BlackQueen;

        public static readonly int[] PieceIndices = {
            WhitePawn, WhiteKnight, WhiteKing, WhiteBishop, WhiteRook, WhiteQueen,
            BlackPawn, BlackKnight, BlackKing, BlackBishop, BlackRook, BlackQueen
        };
        
        // Bit Masks
        public const int PieceTypeMask = 0b0111; // 0b00111
        public const int ColorMask = 0b1000; // 0b1000

        // Methods
        /// <summary>
        /// Gets the piece, stripped of its color.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>An <see cref="int"/> representing the piece type.</returns>
        public static int GetPieceType(int piece) => piece & PieceTypeMask;
        /// <summary>
        /// Checks if <c>piece</c> is of type <c>pieceType</c>.
        /// </summary>
        /// <param name="piece">Piece to compare</param>
        /// <param name="pieceType">Piece type to compare to</param>
        public static bool IsPieceType(int piece, int pieceType) => (GetPieceType(piece) & pieceType) == pieceType ? true : false;
        /// <summary>
        /// Get the color of <c>piece</c>.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>An <see cref="int"/> representing the color of <c>piece</c>.</returns>
        public static int GetPieceColor(int piece) => piece == 0 ? -1 : piece & ColorMask;
        /// <summary>
        /// Check if <c>piece</c> is of color <see cref="White"/>.
        /// </summary>
        /// <param name="piece"></param>
        public static bool IsWhite(int piece) => IsColor(piece, White);
        /// <summary>
        /// Check if two pieces are of the same color.
        /// </summary>
        /// <param name="piece">The piece</param>
        /// <param name="color">The color to check against</param>
        /// <returns>A <see cref="bool"/> representing the piece color equality.</returns>
        public static bool IsColor(int piece, int color) => (piece & ColorMask) == color && piece != 0;
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
        public static int GetOpponentColor(int piece) {
            if (piece == 0)
                return -1;
            if (GetPieceColor(piece) == White)
                return Black;
            else
                return White;
        }
        public static bool IsNull(int piece) { return piece == 0; }

        public static int MakePiece(int pieceType, int pieceColor) => pieceType | pieceColor;
    }

    
}

namespace caZsChessBot.Engine {
    /// <summary>
    /// Structure holding the state of a game.
    /// </summary>
    public readonly struct GameState {
        /// <summary>
        /// Location of the latest double pushed pawn. Set to -1 if no pawn was double pushed.
        /// </summary>
        public readonly int enPassantSquare;
        public readonly int castlingRights;
        public readonly int fiftyMoveCounter;
        public readonly int capturedPieceType;

        public const int WhiteKingsideFlag = 0b0001;
        public const int WhiteQueensideFlag = 0b0010;
        public const int BlackKingsideFlag = 0b0100;
        public const int BlackQueensideFlag = 0b1000;

        public const int ClearWhiteKingsideMask = 0b1110;
        public const int ClearWhiteQueensideMask = 0b1101;
        public const int ClearBlackKingsideMask = 0b1011;
        public const int ClearBlackQueensideMask = 0b0111;

        public const int ClearWhiteCastle = 0b1100;
        public const int ClearBlackCastle = 0b0011;

        public GameState(int enPassantSquare, int castlingRights, int fiftyMoveCounter, int capturedPieceType) {
            this.enPassantSquare = enPassantSquare;
            this.castlingRights = castlingRights;
            this.fiftyMoveCounter = fiftyMoveCounter;
            this.capturedPieceType = capturedPieceType;
        }

        /// <summary>
        /// Check if a color has kingside castling rights.
        /// </summary>
        /// <param name="white">Is the side playing white?</param>
        /// <returns>A <see cref="bool"/> representing if side has kingside castling rights.</returns>
        public bool HasKingsideCastleRight(bool white) {
            int mask = white ? 1 : 4;
            return (castlingRights & mask) != 0;
        }
        /// <summary>
        /// Check if a color has queenside castling rights.
        /// </summary>
        /// <param name="white">Is the side playing white?</param>
        /// <returns>A <see cref="bool"/> representing if side has queenside castling rights.</returns>
        public bool HasQueensideCastleRight(bool white) {
            int mask = white ? 2 : 8;
            return (castlingRights & mask) != 0;
        }

        override public string ToString() {
            return "enPassantSquare= " + enPassantSquare + ", " +
                   "castlingRights= " + castlingRights + ", " +
                   "fiftyMoveCounter= " + fiftyMoveCounter + ", " +
                   "capturedPieceType= " + capturedPieceType;
        }

        // Static helpers

        public static bool IsEqual(GameState a, GameState b) {
            return a.enPassantSquare == b.enPassantSquare &&
                   a.castlingRights == b.castlingRights &&
                   a.fiftyMoveCounter == b.fiftyMoveCounter &&
                   a.capturedPieceType == b.capturedPieceType;
        }

        
    }
}

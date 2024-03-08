namespace caZsChessBot.Engine {
    public readonly struct Move {
        readonly ushort moveValue;

        // Flags
        public const int NoFlag = 0;
        public const int EnPassantCaptureFlag = 1;
        public const int CastleFlag = 2;
        public const int PawnDoublePush = 3;

        public const int PromoteToQueenFlag = 4;
        public const int PromoteToRookFlag = 5;
        public const int PromoteToKnightFlag = 6;
        public const int PromoteToBishopFlag = 7;

        // this is a true bitflag, so one can capture and promote at one time.
        public const int PieceCapturedFlag = 8; 

        // Masks
        const ushort startSquareMask = 0b0000000000111111;
        const ushort targetSquareMask = 0b0000111111000000;
        const ushort flagMask = 0b1111000000000000;
        const ushort clearCaptureMask = 0b0111111111111111;

        public Move(ushort moveValue) {
            this.moveValue = moveValue;
        }

        public Move(int startSquare, int targetSquare) {
            moveValue = (ushort)(startSquare | targetSquare << 6);
        }

        public Move(int startSquare, int targetSquare, int flag) {
            moveValue = (ushort)(startSquare | targetSquare << 6 | flag << 12);
        }

        public ushort Value => moveValue;
        public bool IsNull => moveValue == 0;
        public int StartSquare => moveValue & startSquareMask;
        public int TargetSquare => (moveValue & targetSquareMask) >> 6;
        public bool IsPromotion => (MoveFlag & clearCaptureMask) >= PromoteToQueenFlag;
        public int MoveFlag => (moveValue >> 12) & clearCaptureMask;
        public bool IsPieceCaptured => (MoveFlag & 0x8000) == 0x8000; // check top bit.
        public bool IsEnpassantCapture => (MoveFlag & 0x8000) == EnPassantCaptureFlag;

        public int PromotionPieceType {
            get {
                switch (MoveFlag) {
                    case PromoteToQueenFlag: return Piece.Queen;
                    case PromoteToRookFlag: return Piece.Rook;
                    case PromoteToBishopFlag: return Piece.Bishop;
                    case PromoteToKnightFlag: return Piece.Knight;
                    default: return 0; // Returns null piece for non-promote.
                }
            }
        }
        public static Move NullMove => new Move(0);
        public static bool SameMove(Move a, Move b) => a.moveValue == b.moveValue;

    }
}

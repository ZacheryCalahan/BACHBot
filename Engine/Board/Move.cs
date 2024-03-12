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

        // this is a true bitflag, ex: one can capture and promote at one time.
        public const int PieceCapturedFlag = 8; 

        // Masks
        const ushort startSquareMask = 0b0000000000111111;
        const ushort targetSquareMask = 0b0000111111000000;
        const ushort flagMask = 0b1111000000000000;
        const ushort captureMask = 0b1000000000000000;
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
        public bool IsPromotion => (MoveFlag) is PromoteToBishopFlag or PromoteToKnightFlag or PromoteToQueenFlag or PromoteToRookFlag;
        public int MoveFlag => (moveValue >> 12) & clearCaptureMask;
        public bool IsPieceCaptured => moveValue >> 15 == 1; // check top bit.
        public bool IsEnpassantCapture => (MoveFlag & clearCaptureMask) == EnPassantCaptureFlag;

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

        override public string ToString() {
            string moveString = BoardUtils.GetSquareNameFromCoord(StartSquare) + " " +
                   BoardUtils.GetSquareNameFromCoord(TargetSquare);

            // if promotion
            string promotion;
            switch (MoveFlag) {
                case Move.PromoteToBishopFlag:
                    promotion = "b";
                    break;
                case Move.PromoteToKnightFlag:
                    promotion = "n";
                    break;
                case Move.PromoteToRookFlag:
                    promotion = "r";
                    break;
                case Move.PromoteToQueenFlag:
                    promotion = "q";
                    break;
                case Move.CastleFlag: // yeah i know, doesn't really fit the names, give me a break and I'll fix it.
                    promotion = "castle";
                    break;
                case Move.PawnDoublePush:
                    promotion = "enPassantPush";
                    break;
                default:
                    promotion = "";
                    break;
            }

            return moveString + "" + promotion + "" + (IsPieceCaptured ? " capture" : "");
        }

        

        // Static helpers
        public static Move NullMove => new Move(0);
        public static bool SameMove(Move a, Move b) => a.moveValue == b.moveValue;
        

        

    }
}

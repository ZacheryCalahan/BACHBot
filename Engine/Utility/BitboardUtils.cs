using System.Numerics;

namespace caZsChessBot.Engine {
    public class BitboardUtils {

        public const ulong FileA = 0x101010101010101;

        public const ulong Rank1 = 0b11111111;
        public const ulong Rank2 = Rank1 << 8;
        public const ulong Rank3 = Rank2 << 8;
        public const ulong Rank4 = Rank3 << 8;
        public const ulong Rank5 = Rank4 << 8;
        public const ulong Rank6 = Rank5 << 8;
        public const ulong Rank7 = Rank6 << 8;
        public const ulong Rank8 = Rank7 << 8;

        public const ulong notAFile = ~FileA;
        public const ulong notHFile = ~(FileA << 7);

        public static readonly ulong[] KnightAttacks;
        public static readonly ulong[] KingMoves;
        public static readonly ulong[] WhitePawnAttacks;
        public static readonly ulong[] BlackPawnAttacks;

        public static int PopLSB(ref ulong b) {
            int i = BitOperations.TrailingZeroCount(b);
            b &= (b - 1);
            return i;
        }
        
    }
}

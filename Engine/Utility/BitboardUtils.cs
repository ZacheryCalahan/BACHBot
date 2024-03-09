namespace caZsChessBot.Engine {
    public class BitboardUtils {

        public static ulong SetBit(ulong bitboard, int bit) {
            return bitboard |= 1ul << bit;
        }

        public static bool GetBit(ulong bitboard, int bit) {
            return (bitboard & (1ul << bit)) == (1ul << bit);
        }

        public static bool IsSquaresAttacked(ulong bitboard, ulong mask) {
            return (bitboard & mask) == 0;
        }
    }
}

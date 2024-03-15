namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public class Board {
        // Game board
        public int[] gameboard { get; private set; }

        // Color indexes
        public const int WhiteIndex = 0;
        public const int BlackIndex = 1;

        // White and Black king square
        public int[] KingSquare;

        // Bitboards
        public ulong[] PieceBitboards;

        public ulong[] ColorBitboards;
        public ulong AllPiecesBitboard;
        public ulong FriendlyOrthogonalSliders;
        public ulong FriendlyDiagonalSliders;
        public ulong EnemyOrthogonalSliders;
        public ulong EnemyDiagonalSliders;

        // PieceLists
        public PieceList[] Rooks;
        public PieceList[] Bishops;
        public PieceList[] Queens;
        public PieceList[] Knights;
        public PieceList[] Pawns;

        // Gamestate 
        public bool WhiteToMove { get; private set; }
        public int MoveColor => WhiteToMove ? Piece.White : Piece.Black;
        public int MoveColorIndex => WhiteToMove ? WhiteIndex : BlackIndex;
        public int OpponentColor => WhiteToMove ? Piece.Black : Piece.White;
        public int OpponentColorIndex => WhiteToMove ? BlackIndex : WhiteIndex;

        
        public GameState CurrentGameState;
        public List<Move> AllGameMoves;

        // Board only variables/objs
        private PieceList[] allPieceLists;
        private Stack<GameState> gameStateHistory;
        private FenUtils.PositionInfo startPosInfo;


        public Board() {
            gameboard = new int[64];
        }

        private void Init() {
            AllGameMoves = new List<Move>();
            KingSquare = new int[2];
            Array.Clear(gameboard);
            gameStateHistory = new Stack<GameState>();
            CurrentGameState = new GameState();

            Knights = new PieceList[] { new PieceList(10), new PieceList(10) };
            Pawns = new PieceList[] { new PieceList(8), new PieceList(8) };
            Rooks = new PieceList[] { new PieceList(10), new PieceList(10) };
            Bishops = new PieceList[] { new PieceList(10), new PieceList(10) };
            Queens = new PieceList[] { new PieceList(9), new PieceList(9) };

            allPieceLists = new PieceList[Piece.MaxPieceIndex + 1];
            // White Piecelists
            allPieceLists[Piece.GetPieceIndex(Piece.WhitePawn)] = Pawns[WhiteIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.WhiteKnight)] = Knights[WhiteIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.WhiteBishop)] = Bishops[WhiteIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.WhiteRook)] = Rooks[WhiteIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.WhiteQueen)] = Queens[WhiteIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.WhiteKing)] = new PieceList(1);

            // Black Piecelists
            allPieceLists[Piece.GetPieceIndex(Piece.BlackPawn)] = Pawns[BlackIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.BlackKnight)] = Knights[BlackIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.BlackBishop)] = Bishops[BlackIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.BlackRook)] = Rooks[BlackIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.BlackQueen)] = Queens[BlackIndex];
            allPieceLists[Piece.GetPieceIndex(Piece.BlackKing)] = new PieceList(1);

            // Init Bitboards
            PieceBitboards = new ulong[Piece.MaxPieceIndex + 1];
            ColorBitboards = new ulong[2];
            AllPiecesBitboard = 0;
        }

        public void MakeMove(Move move) {
            
        }

        public void UnMakeMove(Move move) {
            
        }

        public void PassTurn() {
            WhiteToMove = !WhiteToMove;
        }

        void MovePiece(int piece, int startSquare, int targetSquare) {
            
        }

        public int GetPiece(int location) {
            return 0;
        }

        public void SetPiece(int location, int piece) {
            
        }

        public void SetGameStateFromFen() {
            
        }

        public override string ToString() {
            return "";
        }

        // Static helpers.
        /// <summary>
        /// Gets the name of a square from an integer.
        /// </summary>
        /// <param name="coord">The location on the board.</param>
        /// <returns>A <see cref="string"/> representing a board square.</returns>
        
        public static bool IsEqual(Board a, Board b) {
            return false;
        }
    }
}

namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public class Board {
        // Game board
        public int[] gameboard { get; private set; }

        // Gamestate stuff.
        public GameState CurrentGameState;
        public bool WhiteToMove { get; private set; }
        private Stack<GameState> gameStateHistory;
        public int MoveColor => WhiteToMove ? Piece.White : Piece.Black;
        public int OpponentColor => WhiteToMove ? Piece.Black : Piece.White;

        // Tools for debugging
        Move lastMoveMade;

        public Board() {
            gameboard = new int[64];
            gameStateHistory = new Stack<GameState>();
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

        /// <summary>
        /// Get the piece at the location on the board.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Integer representing the piece on the board.</returns>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
        public int GetPiece(int location) {
            return 0;
        }

        /// <summary>
        /// Sets a piece in a specific location on the board.
        /// </summary>
        /// <param name="location">The location on the board.</param>
        /// <param name="piece">The piece to set.</param>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
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

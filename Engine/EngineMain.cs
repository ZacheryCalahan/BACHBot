namespace caZsChessBot.Engine {
    /// <summary>
    /// Container class that manages the game, the AI, and the board.
    /// </summary>
    public class EngineMain {
        private Board board;
        private Bot ai;

        public EngineMain() {
            board = new Board();
            ai = new Bot();
        }

        /// <summary>
        /// Initialize the engine. 
        /// </summary>
        public void InitEngine() {

        }

        /// <summary>
        /// Setup the gamestate for a new game.
        /// </summary>
        public void CreateNewGame() {

        }

        /// <summary>
        /// Setup the position from a FEN.
        /// </summary>
        /// <param name="fenString">The FEN to setup</param>
        public void SetupPosition(string fenString = FenUtils.startPosFen) {
            FenUtils.SetupBoardFromFen(board, fenString);
        }

        // Debug.
        public void PrintDiagram() {
            BoardUtils.PrintDiagram(board);
        }

        public void PrintMoves() {
            BoardUtils.PrintMoves(board);
        }

        public void EnterTester() {
            TestUtils.Test(board);
        }

    }
}

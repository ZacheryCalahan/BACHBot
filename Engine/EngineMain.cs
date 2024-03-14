namespace caZsChessBot.Engine {
    /// <summary>
    /// Container class that manages the game, the AI, and the board.
    /// </summary>
    public class EngineMain {
        public Board board { get; private set; }
        private Bot ai;

        public EngineMain() {
            
        }

        /// <summary>
        /// Initialize the engine. 
        /// </summary>
        public void InitEngine() {
            ai = new Bot();
            ai.OnMoveChosen += OnMoveChosen;
        }

        /// <summary>
        /// Setup the gamestate for a new game.
        /// </summary>
        public void CreateNewGame() {
            board = new Board();
        }

        public void AIMove() {
            ai.ThinkTimed(board);
        }
        public void OnMoveChosen(string move) {
            EngineUCI.RespondMove("bestmove " + move);
        }

        /// <summary>
        /// Setup the position from a FEN.
        /// </summary>
        /// <param name="fenString">The FEN to setup</param>
        public void SetupPosition(string fenString = FenUtils.startPosFen) {
            FenUtils.SetupBoardFromFen(board, fenString);
        }

        public void MakeMoveUCI(string move) { // TODO fix enpassant captures
            board.MakeMove(MoveUtil.GetMoveFromUCIName(move, board));
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

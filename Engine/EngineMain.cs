namespace caZsChessBot.Engine {
    /// <summary>
    /// Container class that manages the game, the AI, and the board.
    /// </summary>
    public class EngineMain {
        public Board board { get; private set; }
        private Bot ai;

        public EngineMain() {
            board = new Board();
            ai = new Bot();
            ai.OnMoveChosen += OnMoveChosen;
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
            board = new Board();
        }

        /// <summary>
        /// Perform search and make move.
        /// </summary>
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

        // Debug stuff, as the lowest call the nonstatic board has is here.
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

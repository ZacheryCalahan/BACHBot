namespace caZsChessBot.Engine {
    public class Bot {
        // Choose move
        public event Action<string>? OnMoveChosen;

        public Bot() {

        }

        public void ThinkTimed(Board board) {
            Move bestMove = Search.GetBestMove(board, 6);
            OnSearchComplete(bestMove);
        }

        void OnSearchComplete(Move move) {
            string moveString = BoardUtils.GetSquareNameFromCoord(move.StartSquare) + "" + 
                BoardUtils.GetSquareNameFromCoord(move.TargetSquare);
            OnMoveChosen?.Invoke(moveString);
        }
    }
}

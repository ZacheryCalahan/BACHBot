namespace caZsChessBot.Engine {
    public class Bot {
        // Choose move
        public event Action<string>? OnMoveChosen;

        public Bot() {

        }

        public void ThinkTimed(Board board) {
            List<Move> LegalMoves = MoveGeneration.GenerateLegalMoves(board);
            Program.SendDebugInfo("Thinking...");

            Random rand = new();
            int randomInt = rand.Next(LegalMoves.Count);
            OnSearchComplete(LegalMoves[randomInt]);
        }

        void OnSearchComplete(Move move) {
            string moveString = BoardUtils.GetSquareNameFromCoord(move.StartSquare) + "" + 
                BoardUtils.GetSquareNameFromCoord(move.TargetSquare);
            OnMoveChosen?.Invoke(moveString);
        }
    }
}

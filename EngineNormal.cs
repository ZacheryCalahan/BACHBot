using caZsChessBot.Engine;

namespace caZsChessBot {
    public class EngineNormal {

        private EngineMain engine;

        public EngineNormal() {
            // Create new engine to use.
            engine = new EngineMain();
            engine.InitEngine();
            engine.CreateNewGame();
        }

        public void RecieveCommand(string message) {

            message = message.Trim();
            string messageType = message.Split(' ')[0].ToLower();
            string[] messageTokens = message.Split(" ");

            switch (messageType) {
                case "position":
                    ProcessPositionCommand(message);
                    break;

                case "go":
                    ProcessGoCommand(message);
                    break;

                case "stop":
                    // TODO
                    break;

                case "print":
                    engine.PrintDiagram();
                    engine.PrintMoves();
                    break;

                case "perft":
                    PerftUtils perft = new PerftUtils();
                    bool parsed = int.TryParse(messageTokens[1], out int depth);
                    if (!parsed) {
                        Program.SendDebugInfo("Invalid value for perft.", true);
                        break;
                    }
                    perft.GetPerftResults(engine.board, depth);
                    break;

                case "printmoves":
                    engine.PrintMoves();
                    break;

                case "verifypiecelists":

                    break;

                default:
                    Program.SendDebugInfo("Unknown command: " + message, true);
                    break;
            }
        }

        private void Respond(string message) {
            Console.WriteLine(message);
        }

        public static void RespondMove(string message) {
            Console.WriteLine(message);
        }

        private void ProcessPositionCommand(string message) {
            if (message.ToLower().Contains("startpos")) {
                engine.SetupPosition();
            } else if (message.ToLower().Contains("fen")) {
                engine.SetupPosition(GetFullFen(message));
            }
            string movestring;
            bool parsed = GetMoves(message, out movestring);
            if (parsed) {
                string[] moves = movestring.Split(' ');
                foreach (string move in moves) {
                    engine.MakeMoveUCI(move);
                }
            } else if (movestring != "none") {
                Program.SendDebugInfo("Moves are invalid.");
            }
        }

        private void ProcessGoCommand(string message) {
            engine.AIMove(); // crude, but works for testing :D
        }

        private bool GetMoves(string message, out string moves) {
            message = message.Trim();
            if (message.IndexOf("moves") == -1) { // no moves detected.
                moves = "none";
                return false;
            }

            int valueStart = message.IndexOf("moves") + "moves".Length; // get the position of the end of moves command
            int valueEnd = message.Length;
            moves = message.Substring(valueStart, valueEnd - valueStart).Trim();
            return true;
        }

        private string GetFullFen(string message) {
            try {
                // Kinda dirty, but effective.
                string[] messageTokens = message.Split(" ");
                string fen = messageTokens[2]; // fen position
                fen += " " + messageTokens[3] + " " +
                    messageTokens[4] + " " +
                    messageTokens[5] + " " +
                    messageTokens[6] + " " +
                    messageTokens[7];
                return fen;
            } catch (IndexOutOfRangeException) {
                Program.SendDebugInfo("The given fen position was invalid. Setting to startpos.", true);
                return FenUtils.startPosFen;
            }

        }
    }
}


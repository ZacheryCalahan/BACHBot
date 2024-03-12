using caZsChessBot.Engine;
/// <summary>
/// Main class that handles input for UCI Commands.
/// </summary>
public class EngineUCI {

    private EngineMain engine = new EngineMain();
    

    public EngineUCI() {
        
    }
    /// <summary>
    /// Take in command, and parse the data to perform an action.
    /// </summary>
    /// <param name="message">UCI Command</param>
    public void RecieveCommand(string message) {

        message = message.Trim();
        string messageType = message.Split(' ')[0].ToLower();

        switch (messageType) {
            case "uci":
                Respond("id name caZ's Chess Bot");
                Respond("id author Zachery Calahan");
                Respond("uciok");
                break;

            case "isready":
                engine.InitEngine();
                Respond("readyok");
                break;

            case "ucinewgame":
                engine.CreateNewGame();
                break;

            case "position":
                ProcessPositionCommand(message);
                break;

            case "go":
                ProcessGoCommand(message); 
                break;

            case "stop":
                // TODO
                break;

            case "d":
                engine.PrintDiagram();
                break;

            case "test":
                // This just runs the Test() method from the Utility package.
                engine.EnterTester();                
                break;

            case "printmoves":
                engine.PrintMoves();
                break;

            default:
                Console.WriteLine("Unknown command: " + message);
                // Log point.
                break;
        }
    }

    private void Respond(string message) {
        Console.WriteLine(message);
        // Also good log point!!!
    }

    public static void RespondMove(string message) {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Handles the UCI command's arguments for the position command, and sets the bots <see cref="Board"/> as the command requests
    /// </summary>
    /// <param name="message">UCI Command</param>
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

    /// <summary>
    /// Handles the UCI command's arguments for the go command, and lets the <see cref="Bot"/> take action.
    /// </summary>
    /// <param name="message"></param>
    private void ProcessGoCommand(string message) {
        engine.AIMove(); // basic, but works :D
    }

    

    /// <summary>
    /// Method that extracts the moves from a position command
    /// </summary>
    /// <param name="message">The command sent to UCI</param>
    /// <param name="moves">The result containing a string of moves.</param>
    /// <returns>A <see cref="bool"/> if moves were found and parsed.</returns>
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
        // Kinda dirty, but effective.
        string[] messageTokens = message.Split(" ");
        string fen = messageTokens[2]; // fen position
        fen += " " + messageTokens[3] + " " +
            messageTokens[4] + " " +
            messageTokens[5] + " " +
            messageTokens[6] + " " +
            messageTokens[7];
        return fen;
    }
}


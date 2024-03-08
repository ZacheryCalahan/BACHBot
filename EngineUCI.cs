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
    }
    /// <summary>
    /// Handles the UCI command's arguments for the go command, and lets the <see cref="Bot"/> take action.
    /// </summary>
    /// <param name="message"></param>
    private void ProcessGoCommand(string message) {

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


/* Main class that handles input for UCI Commands. */

using caZsChessBot;
using caZsChessBot.Engine;
using System.Data;

public static class Program {
    public static bool doDebugText = true;

    public static void Main(string[] args) {
        string command = "";
        while (command != "quit") {
            if (command == "") {

            } else if (command == "uci") {
                SendDebugInfo("UCI Mode");
                UCIMode(command); // requires a reread of uci, so pass the same command.
            } else if (command == "cli") {
                SendDebugInfo("BACHBot CLI >> Use readme for commands.");
                NormalMode();
            } else {
                SendDebugInfo("Invalid command.", true);
            }

            SendColorText("caZ's Chess Bot (BACHBot)", ConsoleColor.Cyan);
            SendColorText("Available commands:\n\tuci\n\tcli", ConsoleColor.Cyan);
            command = Console.ReadLine();
        }
    }

    public static void UCIMode(string command) {
        EngineUCI engineUci = new EngineUCI();
        while (command != "quit") {
            engineUci.RecieveCommand(command);
            command = Console.ReadLine();
        }
    }

    public static void NormalMode() {
        EngineNormal engineNormal = new EngineNormal();
        string command = Console.ReadLine();
        while (command != "quit") {
            engineNormal.RecieveCommand(command);
            command = Console.ReadLine();
        }
    }

    public static void SendDebugInfo(string message, bool isError = false) {
        if (doDebugText || isError) {
            ConsoleColor newColor = isError ? ConsoleColor.Red : ConsoleColor.Yellow;
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            Console.WriteLine("Debug >> " + message);
            Console.ForegroundColor = prevColor;
        }
    }

    public static void SendColorText(string message, ConsoleColor foregroundColor) {
        ConsoleColor prevColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(message);
        Console.ForegroundColor = prevColor;
    }
}
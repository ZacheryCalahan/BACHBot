/* Main class that handles input for UCI Commands. */

using caZsChessBot.Engine;

public static class Program {
    public static bool doDebugText = true;

    public static void Main(string[] args) {

        EngineUCI engine = new EngineUCI();
        
        // Handle input
        string command = Console.ReadLine();
        while (command != "quit") {
            if (command == "uci") {
                SendDebugInfo("UCI Mode");
                while (command != "quit") {
                    engine.RecieveCommand(command);
                    command = Console.ReadLine();
                }
                break;
            } else if (command == "debuginfo") {
                doDebugText = !doDebugText;
                Console.WriteLine("Debug info set to " + doDebugText);
            }

            command = Console.ReadLine();
        }
    }

    public static void SendDebugInfo(string message) {
        if (doDebugText) {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Debug >> " + message);
            Console.ForegroundColor = prevColor;
        }
    }

    public static void SendDebugInfo(string message, bool isError) {
        if (doDebugText || isError) {
            ConsoleColor newColor = isError ? ConsoleColor.Red : ConsoleColor.Yellow;
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            Console.WriteLine("Debug >> " + message);
            Console.ForegroundColor = prevColor;
        }
    }
}
/* Main class that handles input for UCI Commands. */

using caZsChessBot.Engine;

public static class Program {
    public static void Main(string[] args) {

        EngineUCI engine = new EngineUCI();
        string command = String.Empty;

        while (command != "quit") {
            command = Console.ReadLine();
            engine.RecieveCommand(command);
        }
    } 
}
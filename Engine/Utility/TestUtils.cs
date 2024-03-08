using caZsChessBot.Engine;

public static class TestUtils {
    
    public static void Test(Board board) {
        string message = "";
        Console.WriteLine("TEST MENU\nAvailable commands:\n\tgetpiece {0-63}");

        while (message != "exit") {
            message = Console.ReadLine();
            string[] messageTokens = message.Split(" ");
            switch (messageTokens[0]) {
                case "exit":
                    break;
                case "getpiece":
                    GetPieceOnBoard(board, messageTokens[1]); 
                    break;
                default:
                    Console.WriteLine("Invalid command: " + messageTokens[0]);
                    break;
            }
        }
    }

    private static void GetPieceOnBoard(Board board, string index) {
        int value;
        bool parsed = int.TryParse(index, out value);
        if (!parsed && value! >= 0 && value! < 64) {
            Console.WriteLine("Invalid number. {0-63}");
            return;
        } else {
            Console.WriteLine(Piece.GetPieceLetter(board.GetPiece(value)));
        }
    }
}
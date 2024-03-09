using caZsChessBot.Engine;

public static class TestUtils {
    
    public static void Test(Board board) {
        string message = "";
        Console.WriteLine("TEST MENU\nAvailable commands:\n\tgetpiece {0-63}\n\tmovepiece startsquare targetsquare {0-63}");

        while (message != "exit") {
            message = Console.ReadLine();
            string[] messageTokens = message.Split(" ");
            switch (messageTokens[0]) {
                case "exit":
                    break;
                case "getpiece":
                    GetPieceOnBoard(board, messageTokens[1]); 
                    break;
                case "movepiece":
                    MovePieceCastle(board, messageTokens[1], messageTokens[2]);
                    break;
                default:
                    Console.WriteLine("Invalid command: " + messageTokens[0]);
                    break;
            }
        }
    }
    private static void MovePieceCastle(Board board, string startSquare, string targetSquare) {
        int startSquareInt, targetSquareInt;
        bool parsed = int.TryParse(startSquare, out startSquareInt);
        parsed &= int.TryParse(targetSquare, out targetSquareInt);
        if (!parsed && startSquareInt !>= 0 && startSquareInt !< 64 && targetSquareInt !>= 0 && targetSquareInt !< 64) {
            Console.WriteLine("Invalid number. {0-63}");
        } else {
            board.MakeMove(new Move(startSquareInt, targetSquareInt, Move.CastleFlag));
            Console.WriteLine("Sucessfully made move.");
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
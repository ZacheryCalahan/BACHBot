﻿using caZsChessBot.Engine;

public static class TestUtils {
    
    public static void Test(Board board) {
        string message = "";
        Console.WriteLine("TEST MENU\nAvailable commands:" +
                          "\n\tgetpiece {0-63}" +
                          "\n\tmovepiece startsquare targetsquare {0-63}" +
                          "\n\tverifymoves move {uci format}" +
                          "\n\texit");

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
                case "verifymoves":
                    string move = messageTokens[1];
                    VerifyMakeMove(board, MoveUtil.GetMoveFromUCIName(move, board));
                    break;
                case "perft":
                    bool parsed;
                    int depth = 0;
                    try {
                        parsed = int.TryParse(messageTokens[1], out depth);
                    } catch (IndexOutOfRangeException e) {
                        Program.SendDebugInfo("No depth provided.", true);
                        parsed = false;
                    }
                    
                    if (!parsed) {
                        Program.SendDebugInfo("Error, depth is invalid. ", true);
                    } else {
                        PerftUtils.GetPerftResults(board, depth);
                    }
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

    private static void VerifyMakeMove(Board board, Move move) {
        int[] prevBoard = board.gameboard;
        Board prevBoardObj = board;
        GameState prevGameState = board.CurrentGameState;
        board.MakeMove(move);
        board.UnMakeMove(move);
        if (prevBoard == board.gameboard) {
            if (GameState.IsEqual(board.CurrentGameState, prevGameState)) { // Equal board and gamestate
                Program.SendDebugInfo("Make/Unmake move is successful.");
            } else { // Equal board and unequal gamestate
                Program.SendDebugInfo("Board is positionally equal, but gamestate is different.", true);
                Program.SendDebugInfo("old: " + prevGameState.ToString(), true);
                Program.SendDebugInfo("new: " + board.CurrentGameState.ToString(), true);
                Program.SendDebugInfo("Move info: " + move.ToString(), true);
            }
        } else { // Unequal board and untested gamestate.
            Program.SendDebugInfo("Board is not positionally equal.", true);
            Program.SendDebugInfo("old: ", true);
            BoardUtils.PrintDiagram(prevBoardObj);
            Program.SendDebugInfo("new: ", true);
            BoardUtils.PrintDiagram(board);
        }
        
    }
}
using System.Drawing;

namespace caZsChessBot.Engine {
    /// <summary>
    /// This Class holds the internal representation of the chess board.
    /// </summary>
    public class Board {
        // Game board
        private int[] gameboard;

        // Gamestate stuff.
        public GameState CurrentGameState;
        public bool WhiteToMove { get; private set; }

        public Board() {
            gameboard = new int[64];
        }

        public void MakeMove(Move move) {
            // Useful values
            int pieceMoved = gameboard[move.StartSquare];
            int location = move.StartSquare;
            int colorMoved = Piece.GetPieceColor(pieceMoved);

            // Set En Passant Square to where a pawn moved to if move was double push pawn.
            int newEnPassantSquare = (move.MoveFlag == Move.PawnDoublePush) ? move.TargetSquare : -1;

            int newCastlingRights = CurrentGameState.castlingRights;
            // Calculate castle rights.
            if (pieceMoved == Piece.King) {
                if (colorMoved == Piece.White) {
                    newCastlingRights &= GameState.ClearWhiteKingsideMask;
                    newCastlingRights &= GameState.ClearWhiteQueensideMask;
                } else {
                    newCastlingRights &= GameState.ClearBlackKingsideMask;
                    newCastlingRights &= GameState.ClearBlackQueensideMask;
                }
            } else if (pieceMoved == Piece.Rook) {
                if (colorMoved == Piece.White) {
                    if (location == 0) {
                        newCastlingRights &= GameState.ClearWhiteQueensideMask;
                    } else if (location == 7) {
                        newCastlingRights &= GameState.ClearWhiteKingsideMask;
                    }
                } else {
                    if (location == 56) {
                        newCastlingRights &= GameState.ClearBlackQueensideMask;
                    } else if (location == 63) {
                        newCastlingRights &= GameState.ClearBlackKingsideMask;
                    }
                }
            }

            // Calculate 50 move counter.
            int newFiftyMoveCounter;
            if (Piece.GetPieceType(pieceMoved) == Piece.Pawn || move.IsPieceCaptured) {
                newFiftyMoveCounter = 1;
            } else {
                newFiftyMoveCounter = CurrentGameState.fiftyMoveCounter + 1;
            }

            // Move pieces
            if (move.IsEnpassantCapture) {
                gameboard[CurrentGameState.enPassantSquare] = 0; // Remove enpassant pawn

                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;
            } else if (move.MoveFlag == Move.CastleFlag) {
                int rookOffset = 0;
                int rookLocation = 0;
                switch (move.TargetSquare) {
                    case 62: // black kingside
                        rookOffset += -2;
                        rookLocation = 63;
                        break;
                    case 52: // black queenside
                        rookOffset += 3;
                        rookLocation = 56;
                        break;
                    case 2: // white queenside
                        rookOffset += 3;
                        rookLocation = 0;
                        break;
                    case 6: // white kingside
                        rookOffset += -2;
                        rookLocation = 7;
                        break;
                    default:
                        throw new InvalidDataException();
                }

                // move king
                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;

                // move rook
                gameboard[rookOffset + rookLocation] = gameboard[rookLocation];
                gameboard[rookLocation] = 0;
            } else { // traditional move
                gameboard[move.TargetSquare] = gameboard[move.StartSquare];
                gameboard[move.StartSquare] = 0;
            }

            // Commit the move
            

            // also, if enpassant capture flag move, remove the pawn @ gamestate.enpassantsquare.
            // Make the move


            CurrentGameState = new GameState(newEnPassantSquare, newCastlingRights, newFiftyMoveCounter);
        }

        public void UnMakeMove(Move move) {

        }

        /// <summary>
        /// Get the piece at the location on the board.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Integer representing the piece on the board.</returns>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
        public int GetPiece(int location) {
            if (location >= 0 && location < 64) {
                return gameboard[location];
            } else {
                throw new InvalidDataException();
            }
        }
        /// <summary>
        /// Sets a piece in a specific location on the board.
        /// </summary>
        /// <param name="location">The location on the board.</param>
        /// <param name="piece">The piece to set.</param>
        /// <exception cref="InvalidDataException">Throws if location is not within the board.</exception>
        public void SetPiece(int location, int piece) {
            if (location >= 0 && location < 64) {
                gameboard[location] = piece;
            } else {
                throw new InvalidDataException();
            }
        }

        public void SetGameStateFromFen(bool whiteToMove, bool whiteKingCastleRight, bool whiteQueenCastleRight,
            bool blackKingCastleRight, bool blackQueenCastleRight, int enPassantTargetSquare, int halfMoveCounter, int fullMoveCounter) {
            WhiteToMove = whiteToMove;
            int castleRights =
                (whiteKingCastleRight ? GameState.WhiteKingsideFlag : 0) |
                (whiteQueenCastleRight ? GameState.WhiteQueensideFlag : 0) |
                (blackKingCastleRight ? GameState.BlackKingsideFlag : 0) |
                (blackQueenCastleRight ? GameState.BlackQueensideFlag : 0);

            CurrentGameState = new GameState(enPassantTargetSquare, castleRights, halfMoveCounter);
        }

        // Static helpers.
        /// <summary>
        /// Gets the name of a square from an integer.
        /// </summary>
        /// <param name="coord">The location on the board.</param>
        /// <returns>A <see cref="string"/> representing a board square.</returns>
        public static string GetSquareNameFromCoord(int coord) {
            string coordName = "";
            // Get the file name
            switch (coord % 8) {
                case 0:
                    coordName = "a";
                    break;
                case 1:
                    coordName = "b";
                    break;
                case 2:
                    coordName = "c";
                    break;
                case 3:
                    coordName = "d";
                    break;
                case 4:
                    coordName = "e";
                    break;
                case 5:
                    coordName = "f";
                    break;
                case 6:
                    coordName = "g";
                    break;
                case 7:
                    coordName = "h";
                    break;
                default:
                    break;
            }

            // Get rank name
            coordName = coordName + ((coord / 8) + 1);
            return coordName;
        }
        /// <summary>
        /// Gets the coord of a square on the board by its name.
        /// </summary>
        /// <param name="name">The name (algebraic notation) of the coordinate.</param>
        /// <returns>An <see cref="int"/> representing the coordinate on the board.</returns>
        public static int GetSquareCoordFromName(string name) {
            int coord = 0;
            switch (name[0]) {
                case 'a':
                    coord = 0;
                    break;
                case 'b':
                    coord = 1;
                    break;
                case 'c':
                    coord = 2;
                    break;
                case 'd':
                    coord = 3;
                    break;
                case 'e':
                    coord = 4;
                    break;
                case 'f':
                    coord = 5;
                    break;
                case 'g':
                    coord = 6;
                    break;
                case 'h':
                    coord = 7;
                    break;
                default:
                    break;
            }
            return coord + (int.Parse("" + name[1]) - 1) * 8;
        }

        
    }
}

﻿namespace caZsChessBot.Engine {
    public static class MoveGeneration {
        // Ranks and Files for computation (double ply around board for sake of knights, single ply for everything else.)
        static readonly int[] rankA = { 0, 8, 16, 24, 32, 40, 48, 56 };
        static readonly int[] rankB = { 1, 9, 17, 25, 33, 41, 49, 57 };
        static readonly int[] rankG = { 6, 14, 22, 30, 38, 46, 54, 62 };
        static readonly int[] rankH = { 7, 15, 23, 31, 39, 47, 55, 63 };
        static readonly int[] file8 = { 56, 57, 58, 59, 60, 61, 62, 63 };
        static readonly int[] file7 = { 48, 49, 50, 51, 52, 53, 54, 55 };
        static readonly int[] file2 = { 8, 9, 10, 11, 12, 13, 14, 15 };
        static readonly int[] file1 = { 0, 1, 2, 3, 4, 5, 6, 7 };

        public static List<Move> GenerateLegalMoves(Board board) {
            List<Move> processedMoves = GeneratePsuedoLegalMoves(board);

            foreach (Move move in processedMoves) {
                // requires a move to be made on the board.
            }

            return processedMoves;
        }

        public static List<Move> GeneratePsuedoLegalMoves(Board board) {
            List<Move> psuedoLegalMoves = new List<Move>();
            
            int colorToMove = board.WhiteToMove ? Piece.White : Piece.Black;
            int currentPiece = 0;

            for (int i = 0; i < 64; i++) { // Loop through each piece on the board.
                currentPiece = board.GetPiece(i);
                
                // Piece must be color to move.
                if (currentPiece != 0 && Piece.GetPieceColor(currentPiece) == colorToMove) {
                    int pieceType = Piece.GetPieceType(currentPiece);

                    if (Piece.IsSlidingPiece(currentPiece)) {
                        psuedoLegalMoves.AddRange(GenerateSlidingMoves(board, i));
                    } else if (pieceType == Piece.Knight) {
                        psuedoLegalMoves.AddRange(GenerateKnightMoves(board, i));
                    } else if (pieceType == Piece.Pawn) {
                        psuedoLegalMoves.AddRange(GeneratePawnMoves(board, i));
                    } else {
                        psuedoLegalMoves.AddRange(GenerateKingMoves(board, i));
                    }
                }
            }
            return psuedoLegalMoves;
        }

        private static List<Move> GenerateSlidingMoves(Board board, int location) {
            List<Move> slidingMoves = new List<Move>();
            int currentPiece = board.GetPiece(location);
            int offsetLocation = location;

            if (Piece.IsOrthogonalPiece(currentPiece)) {
                // left
                while (!rankA.Contains(offsetLocation) && !rankA.Contains(location)) {
                    offsetLocation += -1;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;

                // right
                while (!rankH.Contains(offsetLocation) && !rankH.Contains(location)) {
                    offsetLocation += 1;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;

                // up
                while (!file8.Contains(offsetLocation) && !file8.Contains(location)) {
                    offsetLocation += 8;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;
                // down
                while (!file1.Contains(offsetLocation) && !file1.Contains(location)) {
                    offsetLocation += -8;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
            } 
            
            if (Piece.IsDiagonalPiece(currentPiece)) {
                // up right
                while (!rankH.Contains(offsetLocation) && !rankH.Contains(location) &&
                       !file8.Contains(offsetLocation) && !file8.Contains(location)) {
                    offsetLocation += 9;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;
                // down right
                while (!rankH.Contains(offsetLocation) && !rankH.Contains(location) &&
                       !file1.Contains(offsetLocation) && !file1.Contains(location)) {
                    offsetLocation += -7;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;
                // up left
                while (!rankA.Contains(offsetLocation) && !rankA.Contains(location) &&
                       !file8.Contains(offsetLocation) && !file8.Contains(location)) {
                    offsetLocation += 7;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
                offsetLocation = location;
                // down left
                while (!rankA.Contains(offsetLocation) && !rankA.Contains(location) &&
                       !file1.Contains(offsetLocation) && !file1.Contains(location)) {
                    offsetLocation += 9;
                    int pieceAtLocation = board.GetPiece(offsetLocation);
                    // Only add piece if target square is empty or contains opponent piece.
                    if (pieceAtLocation == 0) {
                        slidingMoves.Add(new Move(location, offsetLocation));
                    } else if (Piece.GetPieceColor(pieceAtLocation) == Piece.GetOpponentColor(currentPiece)) {
                        slidingMoves.Add(new Move(location, offsetLocation, Move.PieceCapturedFlag));
                        break;
                    } else {
                        break;
                    }
                }
            }

            return slidingMoves;
        }

        private static List<Move> GenerateKingMoves(Board board, int location) {
            // TODO CASTLING!!!
            
            // Handle the psuedo move generation (does not account for checks, we'll do all that in the search/eval).
            List<Move> kingMoves = new List<Move>();
            int currentPiece = board.GetPiece(location);
            int offsetLocation = location;
            int pieceAtLocation;

            List<int> offsets = new List<int>(); // store available offsets

            // Handle left / right movement. (no exeptions for corner positions)
            if (!rankA.Contains(location)) {
                int[] temp = { 7, -1, -9 };
                offsets.AddRange(temp);
            } if (!rankH.Contains(location)) {
                int[] temp = { 9, 1, -7 };
                offsets.AddRange(temp);
            } 
            // Handle up / down movement. (handle the corner exeptions here.)
            if (!file8.Contains(location)) {
                offsets.Add(8);
            } else { // Trim corner exceptions.
                offsets.Remove(7);
                offsets.Remove(9);
            } if (!file1.Contains(location)) {
                offsets.Add(-8);
            } else { // Trim corner exceptions.
                offsets.Remove(-7);
                offsets.Remove(-9);
            }

            foreach (int offset in offsets) {
                offsetLocation = location + offset;
                pieceAtLocation = board.GetPiece(offsetLocation);
                if (Piece.GetPieceColor(board.GetPiece(offsetLocation)) != Piece.GetPieceColor(currentPiece)) {
                    kingMoves.Add(new Move(location, offsetLocation));
                }
            }
            return kingMoves;
        }

        private static List<Move> GeneratePawnMoves(Board board, int location) {
            List<Move> pawnMoves = new List<Move>();
            int currentPiece = board.GetPiece(location);
            bool isWhite = Piece.IsWhite(currentPiece);
            int offset = isWhite ? 8 : -8;
            int offsetDirection = offset / 8;
            int targetSquare = location + offset;

            // Single push
            if (board.GetPiece(targetSquare) == 0) {
                if (file1.Contains(targetSquare) || file8.Contains(targetSquare)) {
                    // Promote! (we assume white can't move backwards. if you're here wondering why white promoted
                    // on file 1, that an issue somewhere else in the move generation.
                    pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToBishopFlag));
                    pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToKnightFlag));
                    pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToRookFlag));
                    pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToQueenFlag));
                } else {
                    pawnMoves.Add(new Move(location, targetSquare));
                }
                
            }

            // Double push
            targetSquare += offset;
            int[] noMoveLocations = isWhite ? file2 : file7;
            if ((noMoveLocations.Contains(location)) && (board.GetPiece(targetSquare) == 0)) {
                // Run if on the correct file for your color, and target is empty.
                pawnMoves.Add(new Move(location, targetSquare, Move.PawnDoublePush));
            }

            // Attacking
            List<int> offsets = new List<int>(); // store available offsets
            // Determine the directions that don't wrap the board. (not elegant but very readable, and simple)
            if (!rankA.Contains(location)) {
                offsets.Add(7);
                offsets.Add(-9);
            } else if (!rankH.Contains(location)) {
                offsets.Add(9);
                offsets.Add(-7);
            }
            // Prune offsets depending on forward direction.
            if (isWhite) {
                offsets.Remove(-9);
                offsets.Remove(-7);
            } else {
                offsets.Remove(9);
                offsets.Remove(7);
            }
            
            foreach (int i in offsets) {
                targetSquare = location + i;
                if (Piece.GetOpponentColor(currentPiece) == Piece.GetPieceColor(board.GetPiece(targetSquare))) {
                    // Disallow taking of anything but an opponent piece.
                    if (file1.Contains(targetSquare) || file8.Contains(targetSquare)) {
                        // Promote! (we assume white can't move backwards. if you're here wondering why white promoted
                        // on file 1, that an issue somewhere else in the move generation. Also this technically
                        // prevents you from moving off the board on the top/bottom, due to pawns not being able to
                        // exist on the top/bottom of the board. So that's neat!
                        pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToBishopFlag | Move.PieceCapturedFlag));
                        pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToKnightFlag | Move.PieceCapturedFlag));
                        pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToRookFlag | Move.PieceCapturedFlag));
                        pawnMoves.Add(new Move(location, targetSquare, Move.PromoteToQueenFlag | Move.PieceCapturedFlag));
                    } else {
                        pawnMoves.Add(new Move(location, targetSquare, Move.PieceCapturedFlag));
                    }
                }
            }

            // Enpassant
            offsets = new List<int>();
            if (!rankA.Contains(location)) {
                offsets.Add(-1);
            } if (!rankH.Contains(location)) {
                offsets.Add(1);
            }
            
            foreach (int i in offsets) {
                int attackingSquare = location + i;
                if (board.CurrentGameState.enPassantSquare == attackingSquare && 
                    board.CurrentGameState.enPassantSquare != -1) {
                    // piece can only be double pushed pawn of opposing color.
                    Console.WriteLine("Checking for enpassant capture on: " + attackingSquare);
                    targetSquare = attackingSquare + (isWhite ? 8 : -8);
                    pawnMoves.Add(new Move(location, targetSquare, Move.EnPassantCaptureFlag));
                }
            }

            return pawnMoves;
        }

        private static List<Move> GenerateKnightMoves(Board board, int location) { 
            List<Move> knightMoves = new List<Move>();
            int currentPiece = board.GetPiece(location);
            List<int> offsets = new List<int>();
            int [] knightOffsets = { 15, 17, 6, 10, -10, -6, -17, -15 };
            offsets.AddRange(knightOffsets);

            // Get and remove offsets that wrap the board.
            if (rankA.Contains(location)) {
                int[] temp = { 15, 6, -10, -17 };
                offsets.Remove(15);
                offsets.Remove(6);
                offsets.Remove(-10);
                offsets.Remove(-17);
            } else if (rankB.Contains(location)) {
                offsets.Remove(6);
                offsets.Remove(-10);
            }

            if (rankH.Contains(location)) {
                offsets.Remove(17);
                offsets.Remove(10);
                offsets.Remove(-6);
                offsets.Remove(-15);
            } else if (rankG.Contains(location)) {
                offsets.Remove(10);
                offsets.Remove(-6);
            }

            int targetSquare;

            foreach (int i in offsets) {
                targetSquare = location + i;
                if (targetSquare >= 0 && targetSquare < 64) {
                    int attackingPiece = board.GetPiece(targetSquare);
                    if (Piece.GetPieceColor(attackingPiece) == Piece.GetOpponentColor(currentPiece)) {
                        // Taking opponent piece.
                        knightMoves.Add(new Move(location, targetSquare, Move.PieceCapturedFlag));
                    } else if (attackingPiece == 0) {
                        // Empty square
                        knightMoves.Add(new Move(location, targetSquare));
                    }
                }
            }
            return knightMoves;
        }
    }
}

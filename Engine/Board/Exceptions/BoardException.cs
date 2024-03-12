using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caZsChessBot.Engine {
    public class BoardException: Exception {
        public BoardException(string message, Board board): base(message) {
            Program.SendDebugInfo("Error caught. " + message, true);
            BoardUtils.PrintDiagram(board);
            
        }
    }
}

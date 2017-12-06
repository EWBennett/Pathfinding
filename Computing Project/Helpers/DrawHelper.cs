using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Computing_Project
{
    public class DrawHelper
    {
        private Grid _grid { get; set; }
        private System.Windows.Forms.Timer _timer { get; set; }

        public DrawHelper(Grid grid, System.Windows.Forms.Timer timer)
        {
            _grid = grid;
            _timer = timer;            
        }

        public void DrawGridPath(Stack<Node> stack)
        {
            ClearGrid();
            _timer.Start();
            //while (stack.Any())
            //{
            //    var current = (Node)stack.Pop();
            //    if(current == _grid.GetEndNode() || current == _grid.GetStartNode()) continue;
            //    current.FormBox.BackColor = Color.Red;
            //}            
        }

        public void ClearGrid()
        {
            foreach (Node node in _grid.ListOfNodes)
            {
                node.FormBox.BackColor = node == _grid.GetStartNode()
                    ? Color.LightGreen
                    : node == _grid.GetEndNode()
                        ? Color.IndianRed
                        : node.State == NodeState.Wall
                            ? Color.DarkSlateGray
                        : Color.White;
            }
        }
    }
}

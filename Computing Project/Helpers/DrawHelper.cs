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

        //The drawing timer is started and any previous routes are cleared
        public void DrawGridPath()
        {
            ClearGrid();
            _timer.Start();         
        }

        //Sets each panel to be the colour it should be before any algorithm is run.
        //Walls are set to grey, start nodes are green, end nodes are red, and any other node is white
        public void ClearGrid()
        {
            foreach (Node node in _grid.ListOfNodes)
            {
                node.FormBox.BackColor = node == _grid.StartNode
                    ? Color.LightGreen
                    : node == _grid.EndNode
                        ? Color.IndianRed
                        : node.State == NodeState.Wall
                            ? Color.DarkSlateGray
                            : node.TravelCost == 2
                                ? Color.RosyBrown
                        : Color.White;
            }
        }
    }
}

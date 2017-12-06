using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
    class BreadthFirst
    {
		private Grid _grid { get; set; }		
        public BreadthFirst(Grid grid)
        {
			_grid = grid;
        }

        public void StartAlgorithm(Grid _grid)
        {
            var unexplored = new Queue<Node>();
            var path = new Stack<Node>();
            var current = _grid.GetStartNode();
            //Add the start to the open queue
            unexplored.Enqueue(_grid.GetStartNode());
            //While there are unexplored nodes
            while (unexplored.Any())
            {                
                current = (Node)(unexplored.Dequeue());
                //Break if the end is found
                current.State = NodeState.Inactive;
                path.Push(current);
                if (current == _grid.GetEndNode())
                {
                    break;
                }                
                //Loop through unexplored neighbors add them to the open queue so they will be explored
                foreach (Node node in _grid.GetActiveNeighbors(current))
                {
                    unexplored.Enqueue(node);
                }
            }
            //insert drawing
            path.Reverse();
            _grid.Drawer.DrawGridPath(path);
        }
    }
}

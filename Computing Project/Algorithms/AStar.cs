using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
	public class AStar
	{
		private Grid _grid { get; set; }
		private int _gValue { get; set; }

		public AStar(Grid grid)
		{
			//Create a new grid and reset start and end node. Reset g value
			_grid = grid;
			_gValue = 0;            
        }

		public void StartAlgorithm()
		{
            var path = new Stack<Node>();
            var start = _grid.GetStartNode();
			var end = _grid.GetEndNode();
            path.Push(start);
			ActivateNeighbors(start);

			//Main loop; goes through every active node
			while (_grid.GetAllActive().Count() > 0)
			{
				//Loops through all active nodes to find the node with the lowest F value.
				var node = _grid.GetAllActive().Aggregate((closest, next) 
					=> (closest == null || FindFScore(next, end) < FindFScore(closest, end) ? next : closest));

                path.Push(node);
				//If the end is found the loop is broken out of
				if(node == _grid.GetEndNode()) break;
				//Otherwise examined node is set to be inactive
				node.State = NodeState.Inactive;
				//Next node's neighbors are activated
				ActivateNeighbors(node);
				_gValue++;
			}
            //insert drawing            
            path.Reverse();
            _grid.Drawer.DrawGridPath(path);
            return;
        }

		//Takes list of adjacent nodes and sets them to be active
		public void ActivateNeighbors(Node current)
		{
			foreach (var neighbor in _grid.GetActiveNeighbors(current))
			{
				neighbor.State = NodeState.Active;
			}
		}

		//Function to calculate the F value of a node (distance to end node (H) + distance travelled (G))
		public int FindFScore(Node current, Node end)
		{
			return (end.X - current.X) + (end.Y - current.Y) + _gValue;
		}

        //Returns distance from a node to the end
        public int FindDistance(Node node)
        {
            return (_grid.GetEndNode().X - node.X) + (_grid.GetEndNode().Y - node.Y);
        }

    }
}


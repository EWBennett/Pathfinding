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

		public AStar(Grid grid)
		{
			_grid = grid;

			//Each node is set to be no state, other than walls. Start node has a distance of 0 while all others have an initial distance of 'infinity'
			foreach (var node in _grid.ListOfNodes)
			{
				if (node.State == NodeState.Wall) continue;
				node.Distance = node == grid.StartNode ? 0 : int.MaxValue;
				node.State = NodeState.None;
			}
		}

		public Tuple<Stack<Node>, Stack<Node>> FindShortestPath()
		{
			var prev = new Dictionary<Guid, Node>();
			var path = new Stack<Node>();
			_grid.StartNode.State = NodeState.Active;
			//Loops through every active node
			while (_grid.GetAllActive().Any())
			{
				//Currently examined node is the node with the lowest f score. F score is the distance to the end added to the distance travelled so far
				var current = _grid.GetAllActive().OrderBy(x => FindFScore(x)).FirstOrDefault();
				current.State = NodeState.Inactive;

				if (current == _grid.EndNode)
				{
					while (prev.ContainsKey(current.Id))
					{
						path.Push(current);
						current = prev[current.Id];
					}
					break;
				}

				//As neighbors are discovered they are set to be active and their distance is assigned to a
				//more accurate value. If a shorter route to a node is found the distance is changed
				foreach (var neighbor in _grid.GetActiveNeighbors(current))
				{
					neighbor.State = NodeState.Active;
					var alt = current.Distance + neighbor.TravelCost;
					if (alt < neighbor.Distance)
					{
						neighbor.Distance = alt;
						prev[neighbor.Id] = current;
					}
				}

				if (current.Distance == int.MaxValue) break;
			}

			var used = new Stack<Node>(prev.Select(x => x.Value).Reverse());
			return new Tuple<Stack<Node>, Stack<Node>>(path, used);
		}
		//Function that returns the F Score of a node. This value is distance to the end
		//using the Manhattan heuristic added to the distance travelled so far
		private int FindFScore(Node node)
		{
			return node.Distance + (Math.Abs(node.X - _grid.EndNode.X) + Math.Abs(node.Y - _grid.EndNode.Y));
		}
	}
}

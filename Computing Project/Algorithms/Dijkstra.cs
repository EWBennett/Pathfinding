using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
	public class Dijkstra
	{
		private Grid _grid { get; set; }
		private readonly Node _startNode;
		private readonly Node _endNode;

		public Dijkstra(Grid grid)
		{
			_grid = grid;
			_startNode = grid.StartNode;
			_endNode = grid.EndNode;

			foreach(var node in _grid.ListOfNodes)
			{
				//Start node distance is 0, all others are 'infinity'. Every node is activated except for walls.
				if (node.State == NodeState.Wall) continue;				
				node.Distance = node == _startNode ? 0 : int.MaxValue;
				node.State = NodeState.Active;
			}
		}

		public Tuple<Stack<Node>, Stack<Node>> FindShortestPath()
		{
			var prev = new Dictionary<Node, Node>();
			var path = new Stack<Node>();

			//Loops through the list of activated nodes
			var allActive = _grid.GetAllActive().ToList();
			while (allActive.Any())
			{
				//The currently examined node is the node closest to the start point
				var smallest = allActive.OrderBy(x => x.Distance).FirstOrDefault();
				allActive.Remove(smallest);

				//When the end is found 
				if(smallest == _endNode)
				{
					while (prev.ContainsKey(smallest))
					{
						path.Push(smallest);
						smallest = prev[smallest];
					}
				}

				if (smallest.Distance == int.MaxValue) break;

				//Assigns the distance of each node as it is discovered. If a shorter route to a
				//node is found the distance is changed to the shorter route
				foreach (var neighbor in _grid.GetActiveNeighbors(smallest))
				{
					var alt = smallest.Distance + neighbor.TravelCost;
					if (alt < neighbor.Distance)
					{
						neighbor.Distance = alt;
						prev[neighbor] = smallest;
					}
				}
			}

			var used = new Stack<Node>(prev.Select(x => x.Value).Reverse());
			return new Tuple<Stack<Node>, Stack<Node>>(path, used);
		}
	}
}

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
			_startNode = grid.GetStartNode();
			_endNode = grid.GetEndNode();

			foreach(var node in _grid.ListOfNodes)
			{
				if (node.State == NodeState.Wall) continue;
				node.Distance = node == _startNode ? 0 : int.MaxValue;
				node.State = NodeState.Active;
			}
		}

		public Stack<Node> FindShortestPath()
		{
			var prev = new Dictionary<Node, Node>();
			var path = new Stack<Node>();

			var allActive = _grid.GetAllActive().ToList();
			while (allActive.Any())
			{
				var smallest = allActive.OrderBy(x => x.Distance).FirstOrDefault();
				allActive.Remove(smallest);

				if(smallest == _endNode)
				{
					while (prev.ContainsKey(smallest))
					{
						path.Push(smallest);
						smallest = prev[smallest];
					}
				}

				if (smallest.Distance == int.MaxValue) break;

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
			return path;
		}
	}
}

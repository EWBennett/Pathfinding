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
			foreach (var node in _grid.ListOfNodes)
			{
                if (node.State == NodeState.Wall) continue;
				node.State = node == _grid.StartNode ? NodeState.Active : NodeState.None;
			}
		}

		public Tuple<Stack<Node>, Stack<Node>> FindShortestPath()
		{
			var prev = new Dictionary<Guid, Node>();
			var path = new Stack<Node>();
			var open = new Queue<Node>();

			open.Enqueue(_grid.StartNode);

			while (open.Any())
			{
				var current = open.Dequeue();
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

				foreach (var neighbor in _grid.GetActiveNeighbors(current))
				{
					if (neighbor.State == NodeState.None)
					{
						neighbor.State = NodeState.Active;
						open.Enqueue(neighbor);
						prev[neighbor.Id] = current;
					}
				}
			}

			var used = new Stack<Node>(prev.Select(x => x.Value).Reverse());
			return new Tuple<Stack<Node>, Stack<Node>>(path, used);
		}
	}
}

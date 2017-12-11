using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
    class BestFirst
    {
		private Grid _grid { get; set; }
		public BestFirst(Grid grid)
        {
			_grid = grid;
			foreach(var node in _grid.ListOfNodes)
			{
                if (node.State == NodeState.Wall) continue;
                node.State = node == _grid.StartNode ? NodeState.Active : NodeState.None;
			}
        }
        public Tuple<Stack<Node>, Stack<Node>> FindShortestPath()
        {
			var prev = new Dictionary<Guid, Node>();
			var path = new Stack<Node>();
			var priorityList = new List<Node>();
            priorityList.Add(_grid.StartNode);

			while(priorityList.Any())
			{
				var current = priorityList.OrderBy(x => x.Distance).FirstOrDefault();
				priorityList.Remove(current);

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
					neighbor.State = NodeState.Active;
					neighbor.Distance = FindDistance(neighbor);
					priorityList.Add(neighbor);
                    prev[neighbor.Id] = current;
                }
				current.State = NodeState.Inactive;
			}
			var used = new Stack<Node>(prev.Select(x => x.Value).Reverse());
			return new Tuple<Stack<Node>, Stack<Node>>(path, used);
		}

        //Returns distance from a node to the end
        public int FindDistance(Node node)
        {
            return (Math.Abs(node.X - _grid.EndNode.X) + Math.Abs(node.Y - _grid.EndNode.Y));
        }
    }
}

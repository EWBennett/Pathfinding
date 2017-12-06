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
        }
        public void StartAlgorithm()
        {
            var path = new Stack<Node>();
            foreach (Node node in _grid.ListOfNodes)
			{
				node.State = NodeState.Inactive;
			}
            var current = _grid.GetStartNode();
            current.State = NodeState.Active;
            //While there are unexplored nodes
            while (_grid.GetAllActive().Any())
            {
                path.Push(current);
				current.State = NodeState.Inactive;
                //Break if the end is found
                if (current == _grid.GetEndNode())
                {
                    break;
                }
                //Loop through the neighbors of the current node, finding the one with the lowest distance from the end. The node with the shortest distance is the next node to be explored
                var NextNode = _grid.GetStartNode();
                foreach(Node node in _grid.GetActiveNeighbors(current))
                {
                    node.State = NodeState.Active;                    
                    if (FindDistance(node) < FindDistance(NextNode))
                    {
                        NextNode = node;
                    }
                }
                current = NextNode;
            }
			//Insert drawing
			//Stack<Node> TempStack = null;
			//current = _grid.GetStartNode();
			//Node Next;
			//while (current != _grid.GetEndNode())
			//{
			//	TempStack.Push(current);
			//	Next = current;
			//	foreach (Node node in _grid.GetAllNeighbors(current))
			//	{
			//		if (FindDistance(node) < FindDistance(Next))
			//		{
			//			Next = node;
			//		}
			//	}
			//	current = Next;
			//}
			path.Reverse();
			_grid.Drawer.DrawGridPath(path);
			return;
		}

        //Returns distance from a node to the end
        public int FindDistance(Node node)
        {
            return (_grid.GetEndNode().X - node.X) + (_grid.GetEndNode().Y - node.Y);
        }
    }
}

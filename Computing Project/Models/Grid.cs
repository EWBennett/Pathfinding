using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
	public class Grid
	{
		public bool Started { get; set; }
		public Node StartNode { get; private set; }
		public Node EndNode { get; private set; }
		public List<Node> ListOfNodes { get; set; }
		public DrawHelper Drawer { get; set; }
		public System.Windows.Forms.CheckBox Diagonal { get; set; }

        public Grid(int width, int height, System.Windows.Forms.CheckBox diagonal, System.Windows.Forms.Timer timer)
		{
			ListOfNodes = new List<Node>();
			Started = false;
			Diagonal = diagonal;
			//For loop to initialise list of nodes and assign states
			for (var x = 1; x <= width; x++)
			{
				for (var y = 1; y <= height; y++)
				{
					ListOfNodes.Add(new Node
					{
						Id = Guid.NewGuid(),
						X = x,
						Y = y,
						State = NodeState.None,
					});
				}
			}

			Drawer = new DrawHelper(this, timer);
		}

		public void SelectEnd(int x, int y)
		{
			EndNode = ListOfNodes.FirstOrDefault(node => node.X == x && node.Y == y);
		}

		public void SelectStart(int x, int y)
		{
			StartNode = ListOfNodes.FirstOrDefault(node => node.X == x && node.Y == y);
		}

		public bool ResetGrid()
		{
			//Set points that aren't start or end to be none
			foreach (var node in ListOfNodes
				.Where(n => n != StartNode && n != EndNode))
			{
				node.State = NodeState.None;
			}

			//Check for start point and end point
			return StartNode != null && EndNode != null;
		}

		//Finds adjacent active nodes to the node passed in
		public IEnumerable<Node> GetActiveNeighbors(Node current)
		{
			if (Diagonal.Checked)
			{
				return ListOfNodes.Where(node =>
									((node.X == current.X + 1 ||
									 node.X == current.X ||
									 node.X == current.X - 1) &&
									(node.Y == current.Y + 1 ||
									 node.Y == current.Y ||
									 node.Y == current.Y - 1))
									 && node.State != NodeState.Inactive
									 && node.State != NodeState.Wall
									 && node != current);				
			}
			else
			{
				return ListOfNodes.Where(node =>
						(((node.X == current.X + 1 || node.X == current.X - 1) && node.Y == current.Y)
						|| ((node.Y == current.Y + 1 || node.Y == current.Y - 1) && node.X == current.X))
						&& node.State != NodeState.Inactive
						&& node.State != NodeState.Wall
						&& node != current);
			}

		}

		public IEnumerable<Node> GetAllNeighbors(Node current)
		{
			return ListOfNodes.Where(node =>
					((node.X == current.X + 1 ||
					 node.X == current.X ||
					 node.X == current.X - 1) &&
					(node.Y == current.Y + 1 ||
					 node.Y == current.Y ||
					 node.Y == current.Y - 1))
					 && node.State != NodeState.Wall
					 && node != current);
		}

		//Returns all active nodes
		public IEnumerable<Node> GetAllActive()
		{
			return ListOfNodes.Where(node => node.State == NodeState.Active);
		}
	}
}

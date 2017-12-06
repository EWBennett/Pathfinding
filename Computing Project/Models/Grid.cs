using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
	public class Grid
	{
		public bool Ed { get; set; }
		public bool Started { get; set; }
		public List<Node> ListOfNodes { get; set; }
		public DrawHelper Drawer { get; set; }
		public System.Windows.Forms.CheckBox Diagonal { get; set; }

		public Grid(int width, int height, System.Windows.Forms.CheckBox diagonal, System.Windows.Forms.Timer timer)
		{
			this.Ed = true;
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

		public Node GetStartNode()
		{
			return ListOfNodes.FirstOrDefault(node => node.IsStart == true);
		}

		public Node GetEndNode()
		{
			return ListOfNodes.FirstOrDefault(node => node.IsEnd == true);
		}

		public void SelectStart(int x, int y)
		{
			var oldNode = GetStartNode();
			//set old start to none and set passed in x and y to be new start
			if (oldNode != null)
			{
				oldNode.IsStart = false;
			}
			var newNode = ListOfNodes.FirstOrDefault(node => node.X.Equals(x) && node.Y.Equals(y));
			newNode.IsStart = true;
		}

		public void SelectEnd(int x, int y)
		{
			var oldNode = GetEndNode();
			//set old end to none and set passed in x and y to be new end
			if (oldNode != null)
			{
				oldNode.IsEnd = false;               
			}
			var newNode = ListOfNodes.FirstOrDefault(node => node.X.Equals(x) && node.Y.Equals(y));
			newNode.IsEnd = true;
		}

		public bool ResetGrid()
		{
			//Set points that aren't start or end to be none
			foreach (var node in ListOfNodes
				.Where(n => n != GetStartNode() && n != GetEndNode()))
			{
				node.State = NodeState.None;
				node.IsEnd = false;
				node.IsStart = false;
			}

			//Check for start point and end point
			return ListOfNodes.Any(node => node.IsStart)
				&& ListOfNodes.Any(node => node.IsEnd);
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
						((node.X == current.X + 1 || node.X == current.X - 1) && node.Y == current.Y)
						|| ((node.Y == current.Y + 1 || node.Y == current.Y - 1) && node.X == current.X)
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Computing_Project
{
	public partial class Form1 : Form
	{
		private Grid _grid;
		private DrawHelper _drawer;
		private Stack<Node> _path;
		private Stack<Node> _remainder;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//Sets the grid on start with a default start of the top left and a default end of the bottom right
			_grid = new Grid(10, 10, AllowDiagonalBox, DrawTimer);
			_grid.SelectStart(1, 1);
			_grid.SelectEnd(10, 10);
			//Assigns each node to a panel on the form
			foreach (Node node in _grid.ListOfNodes)
			{
				node.FormBox = Controls.Find("panel" + node.X + "_" + node.Y, true).FirstOrDefault() as Panel;
			}
			//Sets an event that happens when each panel is clicked
			foreach (var control in Controls)
			{
				if (control.GetType() != typeof(Panel)) continue;
				var box = (Panel)control;
				box.MouseClick += OnPanelClick;
			}
			_drawer = new DrawHelper(_grid, DrawTimer);
			_drawer.ClearGrid();
		}

		private void AStarButton_Click(object sender, EventArgs e)
		{
            NoPathLabel.Visible = false;
            //Begins A* algorithm when button is clicked
            var search = new AStar(_grid);
			var result = search.FindShortestPath();
			_path = result.Item1;
			_remainder = result.Item2;

			NoPathLabel.Visible = !_path.Any(x => x == _grid.EndNode);
			_drawer.DrawGridPath();
		}

		private void DijkstraButton_Click(object sender, EventArgs e)
		{
            NoPathLabel.Visible = false;
            //Begins Dijkstra's algorithm when button is clicked
            var search = new Dijkstra(_grid);
			var result = search.FindShortestPath();
			_path = result.Item1;
			_remainder = result.Item2;

			NoPathLabel.Visible = !_path.Any(x => x == _grid.EndNode);
			_drawer.DrawGridPath();
		}

		private void BestFirstButton_Click(object sender, EventArgs e)
		{
            NoPathLabel.Visible = false;
            //Begins Best First Search algorithm when button is clicked
            var search = new BestFirst(_grid);
			var result = search.FindShortestPath();
            _path = result.Item1;
            _remainder = result.Item2;

            NoPathLabel.Visible = !_path.Any(x => x == _grid.EndNode);
            _drawer.DrawGridPath();
        }

		private void BreadthFirstButton_Click(object sender, EventArgs e)
		{
            NoPathLabel.Visible = false;
            //Begins Breadth First Search algorithm when button is clicked
            var search = new BreadthFirst(_grid);
			var result = search.FindShortestPath();
			_path = result.Item1;
			_remainder = result.Item2;

			NoPathLabel.Visible = !_path.Any(x => x == _grid.EndNode);
			_drawer.DrawGridPath();			
		}

		private void panel1_1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void panel1_2_Paint(object sender, PaintEventArgs e)
		{

		}

		//When a panel is click it's state changes to a wall or not and switches colour
		private void OnPanelClick(object sender, EventArgs e)
		{
			Panel clickedPanel = sender as Panel;
			foreach (Node node in _grid.ListOfNodes)
			{
				if (node.FormBox != clickedPanel || node == _grid.EndNode || node == _grid.StartNode) continue;

				//If the select start or select end buttons have been clicked then the
				//node is changed to be a start or end point instead of a wall
				if (SelectStartButton.Checked)
				{
					_grid.StartNode.FormBox.BackColor = Color.White;
					_grid.SelectStart(node.X, node.Y);
					node.FormBox.BackColor = Color.LightGreen;
					SelectStartButton.Checked = false;
					continue;
				}
				else if (SelectEndButton.Checked)
				{
					_grid.EndNode.FormBox.BackColor = Color.White;
					_grid.SelectEnd(node.X, node.Y);
					node.FormBox.BackColor = Color.IndianRed;
					SelectEndButton.Checked = false;
					continue;
				}
				//node.State = node.State == NodeState.Wall ? NodeState.None : NodeState.Wall;
				if (node.FormBox.BackColor == Color.White)
				{
					node.State = NodeState.Wall;
					node.FormBox.BackColor = Color.DarkSlateGray;
				}
				else if (node.FormBox.BackColor == Color.DarkSlateGray)
				{
					node.State = NodeState.None;
					node.TravelCost = 2;
					node.FormBox.BackColor = Color.RosyBrown;
				}
				else if (node.FormBox.BackColor == Color.RosyBrown)
				{
					node.TravelCost = 1;
					node.FormBox.BackColor = Color.White;
				}
                NoPathLabel.Visible = false;
            }
		}

		//Resets the start and end nodes to the corners and clears the grid of walls and any previous path
		private void ResetGridButton_Click(object sender, EventArgs e)
		{
			_grid.SelectStart(1, 1);
			_grid.SelectEnd(10, 10);
			foreach (Node node in _grid.ListOfNodes)
			{
				if (node == _grid.EndNode || node == _grid.StartNode) continue;
				node.State = NodeState.None;
				node.TravelCost = 1;
			}
			_grid.Drawer.ClearGrid();
            NoPathLabel.Visible = false;
        }

		//When an algorithm is finished this timer is started
		private void DrawTimer_Tick(object sender, EventArgs e)
		{
			if (_remainder != null && _remainder.Any())
			{
				PaintNodes(_remainder, Color.LightSkyBlue);
				return;
			}

			if (_path.Any())
			{
				PaintNodes(_path, Color.CornflowerBlue);
				return;
			}

			//When the stack returns null and the stack is empty the timer stops
			DrawTimer.Stop();
		}

		private void PaintNodes(Stack<Node> nodes, Color colour)
		{
			//Takes the next member of the stack returned by an algorithm
			var current = nodes != null && nodes.Any() ? nodes.Pop() : null;
			if (current == _grid.EndNode || current == _grid.StartNode) return;
			//If the node is not a start or end node it changes the colour
			if (current != null)
			{
				current.FormBox.BackColor = colour;
				return;
			}
		}

		//Clears any previous path without clearing walls or resetting start or end positions
		private void ClearPathButton_Click(object sender, EventArgs e)
		{
			_grid.Drawer.ClearGrid();
		}

		private void SelectStartButton_CheckedChanged(object sender, EventArgs e)
		{

		}
	}

}

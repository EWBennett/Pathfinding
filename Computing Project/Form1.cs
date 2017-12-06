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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Sets the grid on start
            _grid = new Grid(10, 10, AllowDiagonalBox, DrawTimer);
            _grid.SelectStart(1, 1);
            _grid.SelectEnd(10, 10);
            //Assigns each node to a panel on the form
            foreach (Node node in _grid.ListOfNodes)
            {
                node.FormBox = Controls.Find("panel" + node.X + "_" + node.Y, true).FirstOrDefault() as Panel;
            }
            //Sets what happens when each panel is clicked
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
            //Begins A* algorithm when button is clicked
            var Search = new AStar(_grid);
            Search.StartAlgorithm();
        }

        private void DijkstraButton_Click(object sender, EventArgs e)
        {
            var Search = new Dijkstra(_grid);
            _path = Search.FindShortestPath();
            _drawer.DrawGridPath(_path);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            //_grid.Drawer.DrawGridPath();
            //Insert drawing
            //Each function returns a stack for this function to construct into a path(?)

            /*System.Drawing.Graphics graphics = this.CreateGraphics();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(50, 50, 150, 150);
            graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
            graphics.DrawRectangle(System.Drawing.Pens.Red, rectangle);
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.CornflowerBlue);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawLine(myPen, 0, 0, 200, 200);
            myPen.Dispose();
            formGraphics.Dispose();*/
        }

        private void BestFirstButton_Click(object sender, EventArgs e)
        {
            var Search = new BestFirst(_grid);
            Search.StartAlgorithm();
        }

        private void BreadthFirstButton_Click(object sender, EventArgs e)
        {
            var Search = new BreadthFirst(_grid);
            Search.StartAlgorithm(_grid);
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
                if (node.FormBox != clickedPanel || node == _grid.GetEndNode() || node == _grid.GetStartNode()) continue;
                if (SelectStartButton.Checked)
                {
                    _grid.GetStartNode().FormBox.BackColor = Color.White;
                    _grid.SelectStart(node.X, node.Y);
                    node.FormBox.BackColor = Color.LightGreen;
                    SelectStartButton.Checked = false;
                    continue;
                }
                else if (SelectEndButton.Checked)
                {
                    _grid.GetEndNode().FormBox.BackColor = Color.White;
                    _grid.SelectEnd(node.X, node.Y);
                    node.FormBox.BackColor = Color.IndianRed;
                    SelectEndButton.Checked = false;
                    continue;
                }
                node.State = node.State == NodeState.Wall ? NodeState.None : NodeState.Wall;
                if (node.FormBox.BackColor == Color.White)
                {
                    node.FormBox.BackColor = Color.DarkSlateGray;
                }
                else if (node.FormBox.BackColor == Color.DarkSlateGray)
                {
                    node.FormBox.BackColor = Color.White;
                }
            }
        }

        private void ResetGridButton_Click(object sender, EventArgs e)
        {
            _grid.SelectStart(1, 1);
            _grid.SelectEnd(10, 10);
            foreach (Node node in _grid.ListOfNodes)
            {
                if (node == _grid.GetEndNode() || node == _grid.GetStartNode()) continue;
                node.State = NodeState.None;
            }
            _grid.Drawer.ClearGrid();
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
			var current = (_path.Any()) ? _path.Pop() : null;

			if (current != null
				&& current != _grid.GetEndNode()
				&& current != _grid.GetStartNode())
			{
				current.FormBox.BackColor = Color.Red;
			}
			else
			{
				DrawTimer.Stop();
			}
		}

        private void ClearPathButton_Click(object sender, EventArgs e)
        {
            _grid.Drawer.ClearGrid();
        }

        private void SelectStartButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

}

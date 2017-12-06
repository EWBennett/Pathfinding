using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing_Project
{
    public class Node
    {
		public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance;
        public int TravelCost = 1;
		public NodeState State { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public System.Windows.Forms.Panel FormBox { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShardNodes.Model
{
    public class Connector
    {
        public Node PareNode { get; set; }
        public Connection AttatchedConnection { get; set; }
        public Point Hotspot { get; set; }
    }
}

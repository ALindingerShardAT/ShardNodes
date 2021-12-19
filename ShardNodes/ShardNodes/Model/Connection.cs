using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShardNodes.Model
{
    public class Connection
    {
        public Connector SourceConnector { get; set; }
        public Connector DestinationConnector { get; set; }
        public Point SourceConnectorHotspot { get; set; }
        public Point DestinationConnectorHotspot { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShardNodes.Annotations;

namespace ShardNodes.Model
{
    public class Network : INotifyPropertyChanged
    {
        private List<Node> _nodes = new();
        private List<Connection> _connections = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }

            set
            {
                if (value != _nodes)
                {
                    _nodes = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Connection> Connections
        {
            get
            {
                return _connections;
            }

            set
            {
                if (value != _connections)
                {
                    _connections = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

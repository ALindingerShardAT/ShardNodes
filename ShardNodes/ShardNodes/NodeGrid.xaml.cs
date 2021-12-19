using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShardNodes.Model;

namespace ShardNodes
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NodeGrid : INotifyPropertyChanged
    {
        Point _mousePosition;
        Node _currentNode;
        
        Network _Network = new();

        public Network Network
        {
            get
            {
                return _Network;
            }

            set
            {
                if (value != _Network)
                {
                    _Network = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NodeGrid()
        {
            InitializeComponent();
        }

        void NetworkChanged()
        {
            CanvasNodes.Children.Clear();
            foreach (var node in Network.Nodes)
            {
                CanvasNodes.Children.Add(node);
            }
        }

        void AddNewNode(Node n)
        {
            Network.Nodes.Add(n);
            NetworkChanged();
            Canvas.SetLeft(n, _mousePosition.X);
            Canvas.SetTop(n, _mousePosition.Y);
        }

        private void NewNode_Click(object sender, RoutedEventArgs e)
        {
            Node n = new Node();
            n.Header.MouseMove += Node_MouseMove;
            n.MouseEnter += Node_MouseEnter;
            n.MouseLeftButtonDown += Node_MouseLeftButtonDown;
            n.Title.Content = "Empty Node";
            n.HasInfo = false;

            Connector c = new Connector(SnapType.Input);
            n.Input.Children.Add(c);

            AddNewNode(n);
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var node in Network.Nodes)
            {
                node.BorderSelected.Background = (Brush)new BrushConverter().ConvertFromString("#00FFFFFF");
                node.BorderSelected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#00FFFFFF");
            }

            _currentNode.BorderSelected.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFA500");
            _currentNode.BorderSelected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFFFA500");

            NetworkChanged();
        }

        private void Node_MouseEnter(object sender, MouseEventArgs e)
        {
            _currentNode = sender as Node;
        }

        private void InfoNode_Click(object sender, RoutedEventArgs e)
        {
            Node n = new Node();
            n.Header.MouseMove += Node_MouseMove;
            n.MouseEnter += Node_MouseEnter;
            n.MouseLeftButtonDown += Node_MouseLeftButtonDown;
            n.Title.Content = "Info Node";
            n.Info.Content = "Logic Error";
            n.HasInfo = true;

            Connector c = new Connector(SnapType.Output);
            n.Output.Children.Add(c);

            AddNewNode(n);
        }

        private void GridNodes_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mousePosition = Mouse.GetPosition(CanvasNodes);
        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

        private void GridNodes_Drop(object sender, DragEventArgs e)
        {
            Point P = e.GetPosition(CanvasNodes);
            if (_currentNode != null)
            {
                Canvas.SetLeft(_currentNode, P.X - _currentNode.MousePosition.X);
                Canvas.SetTop(_currentNode, P.Y - _currentNode.MousePosition.Y);

                foreach (var connector in _currentNode.Input.Children)
                {
                    if(connector is Connector)
                        ((Connector)connector).GetCords();
                }
            }
        }

        private void CanvasNodes_MouseMove(object sender, MouseEventArgs e)
        {
            Point P = Mouse.GetPosition(CanvasNodes);
            HintX.Text = "X: " + P.X;
            Hinty.Text = "Y: " + P.Y;
        }
    }
}
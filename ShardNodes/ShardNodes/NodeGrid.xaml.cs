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
    public partial class NodeGrid
    {
        System.Windows.Point _mousePosition;
        public Node ActiveNode;

        public Network ActiveNetwork = new();
        public Connector ActiveConnectorDown;
        public Connector ActiveConnectorUp;
        
        public static NodeGrid nodeGrid;
        public NodeGrid()
        {
            InitializeComponent();
            nodeGrid = this;
        }

        public void CheckLine()
        {
            if (ActiveConnectorDown != null && ActiveConnectorUp != null)
            {
                Connection connection = new Connection();
                connection.SourceConnector = ActiveConnectorDown;
                connection.DestinationConnector = ActiveConnectorUp;

                ActiveNetwork.Connections.Add(connection);

                ActiveConnectorDown = null;
                ActiveConnectorUp = null;
                NetworkChanged();
            }
        }

        public void NetworkChanged()
        {
            CanvasNodes.Children.Clear();
            foreach (var node in ActiveNetwork.Nodes)
            {
                CanvasNodes.Children.Add(node);
            }

            foreach (var connection in ActiveNetwork.Connections)
            {
                //Line line = new Line();
                //line.SnapsToDevicePixels = true;
                //line.StrokeThickness = 2;
                //line.Stroke = (Brush) new BrushConverter().ConvertFromString("#FFFFFFFF");
                //line.X1 = connection.SourceConnector.X;
                //line.Y1 = connection.SourceConnector.Y;
                //line.X2 = connection.DestinationConnector.X;
                //line.Y2 = connection.DestinationConnector.Y;

                Point[] points = new[] {
                    new Point(0, 200),
                    new Point(100, 200),
                    new Point(100, 0),
                    new Point(400, 0)
                };
                var b = GetBezierApproximation(points, 256);
                PathFigure pf = new PathFigure(b.Points[0], new[] { b }, false);
                PathFigureCollection pfc = new PathFigureCollection();
                pfc.Add(pf);
                var pge = new PathGeometry();
                pge.Figures = pfc;
                Path p = new Path();
                p.Data = pge;
                p.Stroke = (Brush)new BrushConverter().ConvertFromString("#FFFFFFFF");

                CanvasNodes.Children.Add(p);

            }

            NodesC.Text = "Nodes: " + ActiveNetwork.Nodes.Count;
            StrokeC.Text = "Strokes: " + ActiveNetwork.Connections.Count;

            PolyLineSegment GetBezierApproximation(Point[] controlPoints, int outputSegmentCount)
            {
                Point[] points = new Point[outputSegmentCount + 1];
                for (int i = 0; i <= outputSegmentCount; i++)
                {
                    double t = (double)i / outputSegmentCount;
                    points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
                }
                return new PolyLineSegment(points, true);
            }

            Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
            {
                if (count == 1)
                    return controlPoints[index];
                var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
                var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
                return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
            }
        }

        void AddNewNode(Node n)
        {
            ActiveNetwork.Nodes.Add(n);
            NetworkChanged();
            Canvas.SetLeft(n, _mousePosition.X);
            Canvas.SetTop(n, _mousePosition.Y);
        }

        private void NewNode_Click(object sender, RoutedEventArgs e)
        {
            Node n = new Node();
            n.Title.Content = "Empty Node";
            n.HasInfo = false;

            Connector c = new Connector(SnapType.Input);
            c.ParentNode = n;
            n.Input.Children.Add(c);

            AddNewNode(n);
        }

        private void InfoNode_Click(object sender, RoutedEventArgs e)
        {
            Node n = new Node();
            n.Title.Content = "Info Node";
            n.Info.Content = "Logic Error";
            n.HasInfo = true;

            Connector c = new Connector(SnapType.Output);
            c.ParentNode = n;
            n.Output.Children.Add(c);

            AddNewNode(n);
        }

        private void GridNodes_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mousePosition = Mouse.GetPosition(CanvasNodes);
        }

        private void GridNodes_Drop(object sender, DragEventArgs e)
        {
            System.Windows.Point P = e.GetPosition(CanvasNodes);
            if (ActiveNode != null)
            {
                Canvas.SetLeft(ActiveNode, P.X - ActiveNode.MousePosition.X);
                Canvas.SetTop(ActiveNode, P.Y - ActiveNode.MousePosition.Y);

                foreach (var connector in ActiveNode.Input.Children)
                {
                    if (connector is Connector)
                        ((Connector) connector).UpdateSnap();
                }
                foreach (var connector in ActiveNode.Output.Children)
                {
                    if(connector is Connector)
                        ((Connector)connector).UpdateSnap();
                }
            }
        }

        private void CanvasNodes_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point P = Mouse.GetPosition(CanvasNodes);
            HintX.Text = "X: " + P.X;
            Hinty.Text = "Y: " + P.Y;
        }
    }
}
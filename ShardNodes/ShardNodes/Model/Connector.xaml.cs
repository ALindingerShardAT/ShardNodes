using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShardNodes.Model
{
    public enum SnapType
    {
        Output,
        Input
    }

    /// <summary>
    /// Interaktionslogik für Connector.xaml
    /// </summary>
    public partial class Connector : UserControl
    {
        public Node ParentNode { get; set; }
        public SnapType SnapType;
        public double X { get; set; }
        public double Y { get; set; }

        public Connector(SnapType SnapType)
        {
            InitializeComponent();
            this.SnapType = SnapType;

            if (SnapType == SnapType.Input)
            {
                Out.Width = new GridLength(0);
            }
            else
            {
                In.Width = new GridLength(0);
            }
        }

        public void UpdateSnap()
        {
            Vector offsetNode = VisualTreeHelper.GetOffset(ParentNode);
            UIElement container = VisualTreeHelper.GetParent(ParentNode) as UIElement;

            if (SnapType == SnapType.Input)
            {
                //Vector offsetSnap = VisualTreeHelper.GetOffset(SnapIn);
                //X = offsetSnap.X + offsetNode.X + 50;
                //Y = offsetSnap.Y + offsetNode.Y + 50;
                
                Point relativeLocation = SnapIn.TranslatePoint(new Point(0, 0), container);
                X = relativeLocation.X + (SnapIn.Height / 2);
                Y = relativeLocation.Y + (SnapIn.Width / 2);
            }
            else
            {
                //Vector offsetSnap = VisualTreeHelper.GetOffset(SnapOut);
                //X = offsetSnap.X + offsetNode.X;
                //Y = offsetSnap.Y + offsetNode.Y;

                Point relativeLocation = SnapOut.TranslatePoint(new Point(0, 0), container);
                X = relativeLocation.X + (SnapIn.Height / 2);
                Y = relativeLocation.Y + (SnapIn.Width / 2);
            }
        }

        private void SnapIn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NodeGrid.nodeGrid.ActiveConnectorDown = this;
        }

        private void SnapIn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NodeGrid.nodeGrid.ActiveConnectorUp = this;
            if (NodeGrid.nodeGrid.ActiveConnectorDown == NodeGrid.nodeGrid.ActiveConnectorUp)
            {
                NodeGrid.nodeGrid.ActiveConnectorDown = null;
                NodeGrid.nodeGrid.ActiveConnectorUp = null;
            }
            else
                NodeGrid.nodeGrid.CheckLine();
        }
    }
}

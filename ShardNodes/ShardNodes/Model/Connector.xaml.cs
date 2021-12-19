using System;
using System.Collections.Generic;
using System.Linq;
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
        public SnapType SnapType;
        public Node PareNode { get; set; }
        public Connection AttatchedConnection { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Connector(SnapType SnapType)
        {
            InitializeComponent();
            this.SnapType = SnapType;

            GetCords();

            if (SnapType == SnapType.Input)
            {
                Out.Width = new GridLength(0);
            }
            else
            {
                In.Width = new GridLength(0);
            }
        }

        public void GetCords()
        {
            if (SnapType == SnapType.Input)
            {
                X = Canvas.GetLeft(SnapIn);
                Y = Canvas.GetTop(SnapIn);
            }
            else
            {
                X = Canvas.GetLeft(SnapOut);
                Y = Canvas.GetTop(SnapOut);
            }
        }
    }
}

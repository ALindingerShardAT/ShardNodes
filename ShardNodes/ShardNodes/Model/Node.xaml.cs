using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ShardNodes.Annotations;

namespace ShardNodes.Model
{
    /// <summary>
    /// Interaktionslogik für Node.xaml
    /// </summary>
    public partial class Node : UserControl, INotifyPropertyChanged
    {

        public Point MousePosition { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public List<Connector> Connectors { get; set; }
        public List<Connection> Connections { get; set; }
        bool IsSelected { get; set; }

        private bool _hasInfo;

        public bool HasInfo
        {
            get
            {
                return _hasInfo;
            }
            set
            {
                _hasInfo = value;
                if (_hasInfo)
                {
                    ContainerInfo.Background = (Brush)new BrushConverter().ConvertFromString("#FF8B0000");
                    ContainerInfo.Height = 26;
                }
                else
                {
                    ContainerInfo.Background = (Brush)new BrushConverter().ConvertFromString("#FF0E0E0E");
                    ContainerInfo.Height = 10;
                }
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Node()
        {
            InitializeComponent();

            BorderSelected.Background = (Brush)new BrushConverter().ConvertFromString("#00FFFFFF");
            BorderSelected.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#00FFFFFF");
        }

        private void Header_MouseEnter(object sender, MouseEventArgs e)
        {
            Header.Cursor = Cursors.Hand;
        }

        private void Header_MouseLeave(object sender, MouseEventArgs e)
        {
            Header.Cursor = Cursors.Arrow;
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MousePosition = e.GetPosition(Header);
        }
    }
}

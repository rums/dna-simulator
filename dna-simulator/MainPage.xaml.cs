using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace dna_simulator
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TileEdge_LeftClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	var contextMenu = new ContextMenu();
			var splitEdge = new MenuItem {Header = "Split edge"};
            var addColor = new MenuItem {Header = "Change label"};
            var addGlue = new MenuItem {Header = "Add glue"};
			contextMenu.Items.Add(splitEdge);
            contextMenu.Items.Add(addColor);
            contextMenu.Items.Add(addGlue);
            contextMenu.HorizontalOffset = e.GetPosition(LayoutRoot).X;
            contextMenu.VerticalOffset = e.GetPosition(LayoutRoot).Y;
            contextMenu.IsOpen = true;
        }

        private void TileItem_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
        	
        }
    }
}

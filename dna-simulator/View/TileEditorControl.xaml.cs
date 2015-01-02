using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace dna_simulator.View
{
    public partial class TileEditorControl : UserControl
    {
        DispatcherTimer popupTimer = new DispatcherTimer();

        public TileEditorControl()
        {
            // Required to initialize variables
            InitializeComponent();
            popupTimer.Interval = TimeSpan.FromMilliseconds(2000);
            popupTimer.Tick += popupTimer_Tick;
        }

        void popupTimer_Tick(object sender, EventArgs e)
        {
            popupTimer.Stop();
            EdgeMenu.IsOpen = false;
        }

        private void OnEdgeMenuMouseEnter(object sender, MouseEventArgs e)
        {
            popupTimer.Stop();
            EdgeMenu.IsOpen = true;
        }

        private void OnEdgeMenuMouseLeave(object sender, MouseEventArgs e)
        {
            popupTimer.Start();
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace dna_simulator.Behaviors
{
    public class OpenCloseChildWindowBehavior : Behavior<UserControl>
    {
        private ChildWindow _windowInstance;

        public Type WindowType { get { return (Type)GetValue(WindowTypeProperty); } set { SetValue(WindowTypeProperty, value); } }

        public static readonly DependencyProperty WindowTypeProperty = DependencyProperty.Register("WindowType", typeof(Type), typeof(OpenCloseChildWindowBehavior), new PropertyMetadata(null));

        public bool Open { get { return (bool)GetValue(OpenProperty); } set { SetValue(OpenProperty, value); } }

        public static readonly DependencyProperty OpenProperty = DependencyProperty.Register("Open", typeof(bool), typeof(OpenCloseChildWindowBehavior), new PropertyMetadata(false, OnOpenChanged));

        /// <summary>
        /// Opens or closes a window of type 'WindowType'.
        /// </summary>
        private static void OnOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (OpenCloseChildWindowBehavior)d;
            if ((bool)e.NewValue)
            {
                var instance = Activator.CreateInstance(me.WindowType);
                var window = instance as ChildWindow;
                if (window != null)
                {
                    window.Closing += (s, ev) =>
                    {
                        if (!me.Open) return;
                        // prevents repeated Close call
                        me._windowInstance = null;
                        // set to false, so next time Open is set to true, OnOpenChanged is triggered again
                        me.Open = false;
                    };
                    window.Show();
                    me._windowInstance = window;
                }
                else
                {
                    // could check this already in PropertyChangedCallback of WindowType - but doesn't matter until someone actually tries to open it.
                    throw new ArgumentException(string.Format("Type '{0}' does not derive from System.Windows.Window.", me.WindowType));
                }
            }
            else
            {
                if (me._windowInstance != null)
                    // closed by ViewModel
                    me._windowInstance.Close();
            }
        }
    }
}
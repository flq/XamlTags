using System;
using System.Windows;

namespace XamlAppForTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool BlowUpOnConstruction;

        public MainWindow()
        {
            if (BlowUpOnConstruction)
              throw new InvalidOperationException();
            InitializeComponent();
        }
    }
}

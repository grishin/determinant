using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Determinant.Controls
{
    public sealed partial class AvailableNumbers : UserControl
    {
        public AvailableNumbers()
        {
            this.InitializeComponent();
        }

        public void Init()
        {
            foreach (var border in AvailableNumbersGrid.Children.Cast<Border>())
            {
                border.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private Border GetAvailableNumbersGridElement(int value)
        {
            return AvailableNumbersGrid.Children.Cast<Border>()
                .First(x => ((TextBlock)x.Child).Text == value.ToString());
        }
    }
}

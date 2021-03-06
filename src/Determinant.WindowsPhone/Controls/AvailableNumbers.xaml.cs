﻿using System;
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

namespace Determinant.Controls
{
    public sealed partial class AvailableNumbers : UserControl
    {
        public AvailableNumbers()
        {
            this.InitializeComponent();
        }

        public bool AllowSelect { get; set; }
                      
        public int? SelectedValue { get; private set; }

        public event EventHandler OnCellSelected;

        public void Init()
        {
            foreach (var border in AvailableNumbersGrid.Children.Cast<Border>())
            {
                border.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private Border GetCell(int value)
        {
            return AvailableNumbersGrid.Children.Cast<Border>()
                .First(x => ((TextBlock)x.Child).Text == value.ToString());
        }

        public void HideValue(int value)
        {
            var cell = GetCell(value);
            cell.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AvailableNumbersGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedBorder = (Border)sender;
            var selecteTextBlock = (TextBlock)selectedBorder.Child;

            if (!AllowSelect) return;

            selectedBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SelectedValue = Convert.ToInt32(selecteTextBlock.Text);

            OnCellSelected(this, new EventArgs());
        }
    }
}

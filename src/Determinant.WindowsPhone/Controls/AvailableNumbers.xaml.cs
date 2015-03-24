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

        private Border GetAvailableNumbersGridElement(int value)
        {
            return AvailableNumbersGrid.Children.Cast<Border>()
                .First(x => ((TextBlock)x.Child).Text == value.ToString());
        }

        private void AvailableNumbersGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedBorder = (Border)sender;
            var selecteTextBlock = (TextBlock)selectedBorder.Child;

            if (!AllowSelect) return;

            selectedBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            OnCellSelected(this, new EventArgs());

         /*
            if (_selectedGameFieldGridText == null
                || _selectedGameFieldGridCell == null
                || _selectedGameFieldGridColumn == null
                || _selectedGameFieldGridRow == null
                ) return;

            _selectedGameFieldGridText.Text = selectedAvailableNumbersGridTextBlock.Text;
            _selectedGameFieldGridText.Foreground = this.Foreground;           
            _selectedGameFieldGridCell.Background = this.Background;

            var computerPlayerTurnResult = _game.MakeTurn(_selectedGameFieldGridColumn.Value, _selectedGameFieldGridRow.Value, Convert.ToInt32(selectedAvailableNumbersGridTextBlock.Text));

            _selectedGameFieldGridCell = null;
            _selectedGameFieldGridText = null;
            _selectedGameFieldGridColumn = null;
            _selectedGameFieldGridRow = null;
           
            if (computerPlayerTurnResult != null)
            {
                var availableNumbersGridCell = GetAvailableNumbersGridElement(computerPlayerTurnResult.Value);
                availableNumbersGridCell.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                /*    var gameFieldGridCell = GetGameFieldGridElement(computerPlayerTurnResult.Cell);
                    var gameFieldGridText = (TextBlock)gameFieldGridCell.Child;
                    gameFieldGridText.Text = computerPlayerTurnResult.Value.ToString();
                    gameFieldGridText.Foreground = new SolidColorBrush(Colors.Purple);
                    gameFieldGridText.Visibility = Windows.UI.Xaml.Visibility.Visible;  
            }

            if (_game.IsCompleted) { OnGameCompleted(); }    */
        }
    }
}

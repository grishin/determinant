using Determinant.Domain.Models.Matrix;
using Determinant.Helpers;
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
    public sealed partial class GameField : UserControl
    {
        private Theme _theme;
        private const string EmptyCellText = "?";

        public GameField()
        {
            this.InitializeComponent();
        }

        public void Init(Theme theme)
        {
            _theme = theme;

            ResetAllCells();
            ActiveCell = null;
        }

        public IEnumerable<MainPageGameFieldCell> GetAllCells()
        {
            return GameFieldGrid.Children.Cast<Border>().Select(x => new MainPageGameFieldCell
            {
                Border = x,
                TextBlock = (TextBlock)x.Child
            });
        }

        public MainPageGameFieldCell ActiveCell { get; set; }

        private void ResetAllCells()
        {
            foreach (var cell in this.GetAllCells())
            {
                cell.Border.Background = _theme.BackgroundBrush;
                cell.TextBlock.Text = EmptyCellText;
                cell.TextBlock.Foreground = _theme.BackgroundBrush;
            }
        }

        public Border GetGameFieldGridElement(MatrixCell cell)
        {
            return GameFieldGrid.Children.Cast<Border>()
                .First(x => Grid.GetRow(x) == cell.Row && Grid.GetColumn(x) == cell.Column);
        }

        private void GameFieldGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedGameFieldGridCell = (Border)sender;
            var selectedGameFieldGridText = (TextBlock)selectedGameFieldGridCell.Child;

            if (selectedGameFieldGridText.Text != "?") return;

         /*   _selectedGameFieldGridCell = selectedGameFieldGridCell;
            _selectedGameFieldGridText = selectedGameFieldGridText;

            _selectedGameFieldGridColumn = Grid.GetColumn(_selectedGameFieldGridCell);
            _selectedGameFieldGridRow = Grid.GetRow(_selectedGameFieldGridCell);   */

            // resetting color for all free cells
            /*  foreach (var border in GameFieldGrid.Children.Cast<Border>())
              {
                  var textBlock = (TextBlock)border.Child;
                  if (textBlock.Text == "?")
                  {
                      textBlock.Foreground = this.Background;
                      border.Background = this.Background;
                  }
              }   */

            // setting selected style for tapped cell
        //    _selectedGameFieldGridCell.Background = new SolidColorBrush(Colors.PaleGoldenrod);
        //    _selectedGameFieldGridText.Foreground = new SolidColorBrush(Colors.PaleGoldenrod);
        }
    }

    public class MainPageGameFieldCell
    {
        public Border Border { get; set; }
        public TextBlock TextBlock { get; set; }

        public int? GetValue()
        {
            int value;
            if (Int32.TryParse(TextBlock.Text, out value))
            {
                return value;
            };

            return null;
        }
    }
}

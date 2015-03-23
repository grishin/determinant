using Determinant.Domain.Models.Matrix;
using Determinant.Helpers;
using Determinant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

        private Border _selectedBorder;
        private TextBlock _selectedTextBlock;
        
        public int? SelectedColumn { get; private set;}
        public int? SelectedRow {get; private set;}

        public event EventHandler OnCellSelected;
        public event EventHandler OnCellDeselected();

        public GameField()
        {
            this.InitializeComponent();
        }

        public void Init(Theme theme)
        {
            _theme = theme;

            ResetAllCells();
            ResetSelected();
        }

        private IEnumerable<GameFieldCell> GetAllCells()
        {
            return GameFieldGrid.Children.Cast<Border>().Select(x => new GameFieldCell
            {
                Border = x,
                TextBlock = (TextBlock)x.Child
            });
        }

        private void ResetAllCells()
        {
            foreach (var cell in GetAllCells())
            {
                ResetCell(cell);
            }
        }

        private void ResetCell(GameFieldCell cell)
        {
            cell.Border.Background = _theme.BackgroundBrush;
            cell.TextBlock.Text = EmptyCellText;
            cell.TextBlock.Foreground = _theme.BackgroundBrush;
        }

        private void ResetSelected()
        {
            _selectedBorder = null;
            _selectedTextBlock = null;
            
            SelectedRow = null;
            SelectedColumn = null;

            OnCellDeselected(this, new EventArgs());
        }

        public Border GetGameFieldGridElement(MatrixCell cell)
        {
            return GameFieldGrid.Children.Cast<Border>()
                .First(x => Grid.GetRow(x) == cell.Row && Grid.GetColumn(x) == cell.Column);
        }

        private void GameFieldGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedBorder = (Border)sender;
            var selectedTextBlock = (TextBlock)selectedBorder.Child;

            // validating tapped cell
            if (selectedTextBlock.Text != EmptyCellText) return;

            // set selected
            _selectedBorder = selectedBorder;
            _selectedTextBlock = selectedTextBlock;
            SelectedColumn = Grid.GetColumn(_selectedBorder);
            SelectedRow = Grid.GetRow(_selectedBorder);

            ResetEmptyCells();

            // setting selected style for tapped cell
            _selectedBorder.Background = new SolidColorBrush(Colors.PaleGoldenrod);
            _selectedTextBlock.Foreground = new SolidColorBrush(Colors.PaleGoldenrod);

            OnCellSelected(this, new EventArgs());
        }

        private void ResetEmptyCells()
        {
            foreach (var cell in GetAllCells().Where(x => x.IsEmpty))
            {
                ResetCell(cell);
            }
        }
    }
}

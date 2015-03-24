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

        private GameFieldCell _selectedCell;

        public int? SelectedColumn { get; private set; }
        public int? SelectedRow { get; private set; }

        public event EventHandler OnCellSelected;
        public event EventHandler OnCellDeselected;

        public GameField()
        {
            this.InitializeComponent();
        }

        public void Init(Theme theme)
        {
            _theme = theme;

            ResetAllCellsVisual();
            ResetSelected();
        }

        public void SetValue(MatrixCell cell, int value)
        {
            var gameFieldCell = GetCell(cell);
            gameFieldCell.TextBlock.Text = value.ToString();
        }

        public void SetValue(int value)
        {
            _selectedCell.TextBlock.Text = value.ToString();

            ResetSelected();
            ResetCellVisual(_selectedCell);
        }

        private IEnumerable<GameFieldCell> GetAllCells()
        {
            return GameFieldGrid.Children.Cast<Border>().Select(x => new GameFieldCell
            {
                Border = x,
                TextBlock = (TextBlock)x.Child
            });
        }

        private GameFieldCell GetCell(MatrixCell cell)
        {
            return GameFieldGrid
                .Children
                .Cast<Border>()
                .Where(x => Grid.GetColumn(x) == cell.Column && Grid.GetRow(x) == cell.Row)
                .Select(x => new GameFieldCell
                {
                    Border = x,
                    TextBlock = (TextBlock)x.Child
                }).First();
        }

        private void ResetAllCellsVisual()
        {
            foreach (var cell in GetAllCells())
            {
                ResetCellVisual(cell);
            }
        }

        private void ResetCellVisual(GameFieldCell cell)
        {
            cell.Border.Background = _theme.BackgroundBrush;
            cell.TextBlock.Text = EmptyCellText;
            cell.TextBlock.Foreground = _theme.BackgroundBrush;
        }

        private void ResetSelected()
        {
            _selectedCell = null;

            SelectedRow = null;
            SelectedColumn = null;

            OnCellDeselected(this, new EventArgs());
        }

        private void GameFieldGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedBorder = (Border)sender;
            var selectedTextBlock = (TextBlock)selectedBorder.Child;

            // validating tapped cell
            if (selectedTextBlock.Text != EmptyCellText) return;

            // set selected
            _selectedCell = new GameFieldCell
            {
                Border = selectedBorder,
                TextBlock = selectedTextBlock
            };

            SelectedColumn = Grid.GetColumn(_selectedCell.Border);
            SelectedRow = Grid.GetRow(_selectedCell.Border);

            ResetEmptyCells();

            // setting selected style for tapped cell
            _selectedCell.Border.Background = new SolidColorBrush(Colors.PaleGoldenrod);
            _selectedCell.TextBlock.Foreground = new SolidColorBrush(Colors.PaleGoldenrod);

            OnCellSelected(this, new EventArgs());
        }

        private void ResetEmptyCells()
        {
            foreach (var cell in GetAllCells().Where(x => x.IsEmpty))
            {
                ResetCellVisual(cell);
            }
        }
    }
}

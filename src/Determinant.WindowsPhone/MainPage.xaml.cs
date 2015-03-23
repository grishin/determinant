using Determinant.Domain.Models;
using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Services.Game;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Determinant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        class Theme
        {
            public Theme(Brush foregroundBrush, Brush backgroundBrush)
            {
                ForegroundBrush = foregroundBrush;
                BackgroundBrush = backgroundBrush;
            }

            public Brush ForegroundBrush { get; private set; }
            public Brush BackgroundBrush { get; private set; }
        }


        class MainPageGameField
        {
            private Grid _grid;
            private Theme _theme;

            const string EmptyCellText = "?";

            public MainPageGameField(Grid grid, Theme theme)
            {
                _grid = grid;
                _theme = theme;
            }

            public IEnumerable<MainPageGameFieldCell> GetAllCells()
            {
                return _grid.Children.Cast<Border>().Select(x => new MainPageGameFieldCell
                {
                    Border = x,
                    TextBlock = (TextBlock)x.Child
                });
            }

            public MainPageGameFieldCell ActiveCell { get; set; }

            public void ResetAllCells()
            {
                foreach (var cell in this.GetAllCells())
                {
                    cell.Border.Background = _theme.BackgroundBrush;
                    cell.TextBlock.Text = EmptyCellText;
                    cell.TextBlock.Foreground = _theme.BackgroundBrush;
                }
            }
        }

        class MainPageGameFieldCell
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

        private MainPageGameField GameField { get; set; }


        private Border _selectedGameFieldGridCell;
        private TextBlock _selectedGameFieldGridText;
        private int? _selectedGameFieldGridColumn;
        private int? _selectedGameFieldGridRow;

        private Game _game;
        private Theme _theme;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void InitVisualControls()
        {
            _theme = new Theme(this.Background, this.Foreground);

            // create new game data model
            _game = new SinglePlayerGameBuilder().CreateGame();

            //--
            GameField = new MainPageGameField(GameFieldGrid, _theme);
            GameField.ActiveCell = null;

            // setting player names
            PositivePlayerName.Text = _game.PositivePlayer.Name;
            NegativePlayerName.Text = _game.NegativePlayer.Name;

            // hiding game completed controls
            Winner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // reset gamefield grid values
            GameField.ResetAllCells();

            // set visible to availablenumbers grid buttons 
            foreach (var border in AvailableNumbersGrid.Children.Cast<Border>())
            {
                border.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitVisualControls();
        }

        private Border GetGameFieldGridElement(MatrixCell cell)
        {
            return GameFieldGrid.Children.Cast<Border>()
                .First(x => Grid.GetRow(x) == cell.Row && Grid.GetColumn(x) == cell.Column);
        }

        private Border GetAvailableNumbersGridElement(int value)
        {
            return AvailableNumbersGrid.Children.Cast<Border>()
                .First(x => ((TextBlock)x.Child).Text == value.ToString());
        }

        private void GameFieldGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedGameFieldGridCell = (Border)sender;
            var selectedGameFieldGridText = (TextBlock)selectedGameFieldGridCell.Child;

            if (selectedGameFieldGridText.Text != "?") return;

            _selectedGameFieldGridCell = selectedGameFieldGridCell;
            _selectedGameFieldGridText = selectedGameFieldGridText;

            _selectedGameFieldGridColumn = Grid.GetColumn(_selectedGameFieldGridCell);
            _selectedGameFieldGridRow = Grid.GetRow(_selectedGameFieldGridCell);

            // resetting color for all free cells
            foreach (var border in GameFieldGrid.Children.Cast<Border>())
            {
                var textBlock = (TextBlock)border.Child;
                if (textBlock.Text == "?")
                {
                    textBlock.Foreground = this.Background;
                    border.Background = this.Background;
                }
            }

            // setting selected style for tapped cell
            _selectedGameFieldGridCell.Background = new SolidColorBrush(Colors.PaleGoldenrod);
            _selectedGameFieldGridText.Foreground = new SolidColorBrush(Colors.PaleGoldenrod);
        }


        private void AvailableNumbersGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedAvailableNumbersGridCell = (Border)sender;
            var selectedAvailableNumbersGridTextBlock = (TextBlock)selectedAvailableNumbersGridCell.Child;

            if (_selectedGameFieldGridText == null
                || _selectedGameFieldGridCell == null
                || _selectedGameFieldGridColumn == null
                || _selectedGameFieldGridRow == null
                ) return;

            _selectedGameFieldGridText.Text = selectedAvailableNumbersGridTextBlock.Text;
            _selectedGameFieldGridText.Foreground = this.Foreground;

            selectedAvailableNumbersGridCell.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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

                var gameFieldGridCell = GetGameFieldGridElement(computerPlayerTurnResult.Cell);
                var gameFieldGridText = (TextBlock)gameFieldGridCell.Child;
                gameFieldGridText.Text = computerPlayerTurnResult.Value.ToString();
                gameFieldGridText.Foreground = new SolidColorBrush(Colors.Purple);
                gameFieldGridText.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            if (_game.IsCompleted) { OnGameCompleted(); }
        }

        private void OnGameCompleted()
        {
            if (_game.Winner != null)
            {
                Winner.Text = "Winner is " + _game.Winner.Name;
            }
            else
            {
                Winner.Text = "DRAW";
            }

            Winner.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void Restart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            InitVisualControls();
        }
    }
}

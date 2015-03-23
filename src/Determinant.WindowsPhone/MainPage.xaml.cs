using Determinant.Domain.Models;
using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Services.Game;
using Determinant.Helpers;
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

namespace Determinant
{
    public sealed partial class MainPage : Page
    {
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

        private void Init()
        {
            _theme = new Theme(this.Background, this.Foreground);

            // create new game data model
            _game = new SinglePlayerGameBuilder().CreateGame();

            GameField.Init(_theme);
            PlayersInfo.Init(_game);

            // hiding game completed controls
            Winner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // set visible to availablenumbers grid buttons 
            AvailableNumbers.Init();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
        }

        private Border GetAvailableNumbersGridElement(int value)
        {
            return AvailableNumbersGrid.Children.Cast<Border>()
                .First(x => ((TextBlock)x.Child).Text == value.ToString());
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

            /*    var gameFieldGridCell = GetGameFieldGridElement(computerPlayerTurnResult.Cell);
                var gameFieldGridText = (TextBlock)gameFieldGridCell.Child;
                gameFieldGridText.Text = computerPlayerTurnResult.Value.ToString();
                gameFieldGridText.Foreground = new SolidColorBrush(Colors.Purple);
                gameFieldGridText.Visibility = Windows.UI.Xaml.Visibility.Visible;  */
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
            Init();
        }
    }
}

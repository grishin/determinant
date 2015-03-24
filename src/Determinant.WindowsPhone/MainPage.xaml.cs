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
        private Game _game;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            GameField.OnCellSelected += OnGameFieldCellSelected;
            GameField.OnCellDeselected += OnGameFieldCellDeselected;

            AvailableNumbers.OnCellSelected += OnAvailableNumbersCellSelected;
        }

        private void Init()
        {
            _game = new SinglePlayerGameBuilder().CreateGame();
            _game.OnCompleted += OnGameCompleted;

            // init game elements in user controls
            GameField.Init(new Theme(this.Foreground, this.Background));
            PlayersInfo.Init(_game.PositivePlayer.Name, _game.NegativePlayer.Name);
            AvailableNumbers.Init();

            // hiding game completed controls
            Winner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            PlayersInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;
            AvailableNumbers.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
        }

        private void OnGameFieldCellSelected(object sender, EventArgs e)
        {
            AvailableNumbers.AllowSelect = true;
        }

        private void OnGameFieldCellDeselected(object sender, EventArgs e)
        {
            AvailableNumbers.AllowSelect = false;
        }

        private void OnAvailableNumbersCellSelected(object sender, EventArgs e)
        {
            if (GameField.SelectedColumn == null || GameField.SelectedRow == null || AvailableNumbers.SelectedValue == null) return;

            MakeHumanPlayerTurn();

            if (_game.Mode == GameMode.SinglePlayer && !_game.IsCompleted)
            {
                MakeComputerPlayerTurn();
            }
        }

        private void MakeComputerPlayerTurn()
        {
            var turnResult = _game.MakeComputerPlayerTurn();
            GameField.SetValue(turnResult.Cell, turnResult.Value);
            AvailableNumbers.HideValue(turnResult.Value);
        }

        private void MakeHumanPlayerTurn()
        {
            int column = GameField.SelectedColumn.Value;
            int row = GameField.SelectedRow.Value;
            int value = AvailableNumbers.SelectedValue.Value;

            _game.MakeHumanPlayerTurn(new MatrixCell { Row = row, Column = column }, value);
            GameField.SetValue(value);
            AvailableNumbers.HideValue(value);
        }

        private void OnGameCompleted(object sender, EventArgs e)
        {
            Winner.Text = _game.Winner != null ? "Winner is " + _game.Winner.Name : "DRAW";
            Winner.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Visible;
            PlayersInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AvailableNumbers.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Restart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Init();
        }
    }
}

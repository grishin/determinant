using Determinant.Domain.Models;
using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Services.Game;
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

namespace Determinant
{
    public sealed partial class MainPage : Page
    {
        private Game _game;
        private IGameBuilder _gameBuilder;

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
            _game = _gameBuilder.CreateGame();
            _game.OnCompleted += OnGameCompleted;
            _game.OnComputerPlayerTurn += OnComputerPlayerTurn;
            _game.OnHumanPlayerTurn += OnHumanPlayerTurn;
            _game.OnPlayerChanged += OnPlayerChanged;

            // init game elements in user controls
            GameField.Init(new Theme(this.Foreground, this.Background));
            PlayersInfo.Init(_game.PositivePlayer.Name, _game.NegativePlayer.Name);
            AvailableNumbers.Init();

            // hiding game completed controls
            Winner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            QuitToMenu.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            PlayersInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;
            AvailableNumbers.Visibility = Windows.UI.Xaml.Visibility.Visible;

            // if computer player goes first
            _game.TryMakeInitialComputerTurn();

            PlayersInfo.SetActivePlayer(_game.CurrentPlayer);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _gameBuilder = (IGameBuilder)e.Parameter;
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

            int column = GameField.SelectedColumn.Value;
            int row = GameField.SelectedRow.Value;
            int value = AvailableNumbers.SelectedValue.Value;

            _game.MakeTurn(new MatrixCell { Row = row, Column = column }, value);
        }

        private void OnComputerPlayerTurn(object sender, TurnEventArgs e)
        {
            GameField.SetValue(e.Cell, e.Value, _game.CurrentPlayer);
            AvailableNumbers.HideValue(e.Value);
        }

        private void OnHumanPlayerTurn(object sender, TurnEventArgs e)
        {
            GameField.SetValue(e.Cell, e.Value, _game.CurrentPlayer);
            AvailableNumbers.HideValue(e.Value);
        }

        private void OnPlayerChanged(object sender, EventArgs e)
        {
            PlayersInfo.SetActivePlayer(_game.CurrentPlayer);
        }

        private void OnGameCompleted(object sender, GameCompletedEventArgs e)
        {
            Winner.Text = e.WinnerPlayer != null ? String.Format("Winner is {0} ({1})", e.WinnerPlayer.Name, e.Determinant) : "DRAW";
            
            Winner.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Restart.Visibility = Windows.UI.Xaml.Visibility.Visible;
            QuitToMenu.Visibility = Windows.UI.Xaml.Visibility.Visible;
            PlayersInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AvailableNumbers.Visibility = Windows.UI.Xaml.Visibility.Collapsed;            
        }

        private void Restart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Init();
        }

        private void QuitToMenu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _game = null;
            Frame.Navigate(typeof(StartPage));
        }
    }
}

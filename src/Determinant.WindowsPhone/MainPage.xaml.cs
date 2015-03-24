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
        private Theme _theme;

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
            _theme = new Theme(this.Foreground, this.Background);
            _game = new SinglePlayerGameBuilder().CreateGame();

            // init game elements in user controls
            GameField.Init(_theme);
            PlayersInfo.Init(_game.PositivePlayer.Name, _game.NegativePlayer.Name);
            AvailableNumbers.Init();

            // hiding game completed controls
            WinnerBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
            var computerPlayerTurnResult = _game.MakeTurn(GameField.SelectedColumn.Value, GameField.SelectedRow.Value, AvailableNumbers.SelectedValue.Value);
            //GameField.Res

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

            WinnerBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void Restart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Init();
        }
    }
}

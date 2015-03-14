using Determinant.Domain.Models;
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
        private Border _selectedGameFieldGridCell;
        private TextBlock _selectedGameFieldGridText;
        private int _selectedGameFieldGridPosX;
        private int _selectedGameFieldGridPosY;

        private Game _game;


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _game = new SinglePlayerGameBuilder().CreateGame();

            foreach (var border in GameFieldGrid.Children.Cast<Border>())
            {
                var textBlock = (TextBlock)border.Child;
                textBlock.Text = "?";
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }


            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private FrameworkElement GetGameFieldGridElement(int posx, int posy)
        {
            return GameFieldGrid.Children.Cast<FrameworkElement>()
                .First(x => Grid.GetRow(x) == posx && Grid.GetColumn(x) == posy);
        }

        private void GameFieldGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _selectedGameFieldGridCell = (Border)sender;
            _selectedGameFieldGridText = (TextBlock)_selectedGameFieldGridCell.Child;

            _selectedGameFieldGridPosX = Grid.GetRow(_selectedGameFieldGridCell);
            _selectedGameFieldGridPosY = Grid.GetColumn(_selectedGameFieldGridCell);

           // if (_selectedGameFieldGridText.Text != "?") return;

            // resetting color and thickness for all cells
            foreach(var border in GameFieldGrid.Children.Cast<Border>())
            {
                border.Background = new SolidColorBrush(Colors.Black);
            }

            // setting selected style for tapped cell

            _selectedGameFieldGridCell.Background = new SolidColorBrush(Colors.PaleGoldenrod);


            //tappedTextBlock.Text = posx + ":" + posy;
        }


        private void AvailableNumbersGridCell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedAvailableNumbersGridCell = (Border)sender;
            var selectedAvailableNumbersGridTextBlock = (TextBlock)selectedAvailableNumbersGridCell.Child;

            _selectedGameFieldGridText.Text = selectedAvailableNumbersGridTextBlock.Text;
            _selectedGameFieldGridText.Foreground = new SolidColorBrush(Colors.White);

            selectedAvailableNumbersGridCell.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            _game.MakeTurn(_selectedGameFieldGridPosX, _selectedGameFieldGridPosY, Convert.ToInt32(selectedAvailableNumbersGridTextBlock.Text));

            if (_game.IsCompleted)
            {
                switch(_game.Winner)
                {
                    case Domain.Models.Winner.Positive:
                         Winner.Text = "Winner is POSITIVE";
                        break;
                    case Domain.Models.Winner.Negative:
                         Winner.Text = "Winner is NEGATIVE";
                        break;
                    case Domain.Models.Winner.Draw:
                         Winner.Text = "DRAW";
                        break;
                }                
            }

        }
    }
}

using Determinant.Domain.Models;
using Determinant.Domain.Models.Player;
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
    public sealed partial class PlayersInfo : UserControl
    {
        public PlayersInfo()
        {
            this.InitializeComponent();
        }

        public void Init(string positivePlayerName, string negativePlayerName)
        {
            PositivePlayerName.Text = positivePlayerName;
            NegativePlayerName.Text = negativePlayerName;
        }

        public void SetActivePlayer(IPlayer player)
        {
            if (player.Goal == PlayerGoal.Positive)
            {
                PositivePlayerName.Foreground = new SolidColorBrush(Colors.Red);
                NegativePlayerName.Foreground = this.Foreground;
            }
            else // (player.Goal == PlayerGoal.Negative)
            {
                PositivePlayerName.Foreground = this.Foreground;
                NegativePlayerName.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}

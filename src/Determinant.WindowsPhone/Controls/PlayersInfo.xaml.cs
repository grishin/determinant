using Determinant.Domain.Models;
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
    }
}

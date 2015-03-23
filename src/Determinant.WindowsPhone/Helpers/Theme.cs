using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Determinant.Helpers
{
    public class Theme
    {
        public Theme(Brush foregroundBrush, Brush backgroundBrush)
        {
            ForegroundBrush = foregroundBrush;
            BackgroundBrush = backgroundBrush;
        }

        public Brush ForegroundBrush { get; private set; }
        public Brush BackgroundBrush { get; private set; }
    }
}

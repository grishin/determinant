using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Determinant.Models
{
    class GameFieldCell
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

        public bool IsEmpty
        {
            get { return TextBlock.Text == "?"; }
        }
    }
}

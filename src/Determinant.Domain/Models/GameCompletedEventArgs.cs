using Determinant.Domain.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models
{
    public class GameCompletedEventArgs : EventArgs
    {
        public IPlayer WinnerPlayer { get; set; }
        public int Determinant { get; set; }
    }
}

using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models
{
    public class Game
    {
        private Matrix3x3 _matrix;
        private IEnumerable<IPlayer> _players;

        public Game(IEnumerable<IPlayer> players)
        {
            _matrix = new Matrix3x3();
            _players = players;
        }

        public void Run()
        {

        }

    }
}

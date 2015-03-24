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
        public bool IsCompleted { get; private set; }
        public IPlayer Winner { get; private set; }
        public IPlayer CurrentPlayer { get; private set; }

        private Matrix3x3 _matrix;
        private IEnumerable<IPlayer> _players;

        private const int MaxPlayers = 2;

        public IPlayer PositivePlayer { get; private set;}
        public IPlayer NegativePlayer { get; private set;}

        public event EventHandler OnCreated;
        public event EventHandler OnCompleted;
        public event EventHandler OnHumanPlayerTurn;
        public event EventHandler OnComputerPlayerTurn;


        public Game(IPlayer positivePlayer, IPlayer negativePlayer)
        {
            PositivePlayer = positivePlayer;
            NegativePlayer = negativePlayer;

            _matrix = new Matrix3x3();
            _players = new [] {
             PositivePlayer, NegativePlayer   
            };

            IsCompleted = false;
            Winner = null;

            OnCreated(this, new EventArgs());
        }

        public void MakeHumanPlayerTurn(MatrixCell cell, int value)
        {
            _matrix.SetValue(cell, value);
            
            CheckIfCompleted();
        }

        public TurnResult MakeComputerPlayerTurn()
        {
            var player = _players.FirstOrDefault(x => x is ComputerPlayer);
            if (player == null) { return null; }

            var turnResult = (player as ComputerPlayer).MakeTurn(_matrix);
            _matrix.SetValue(turnResult.Cell, turnResult.Value);

            CheckIfCompleted();

            return turnResult;
        }

        private void CheckIfCompleted()
        {
            if (_matrix.IsFull())
            {
                int determinant = _matrix.GetDeterminant();

                if (determinant > 0)
                {
                    Winner = PositivePlayer;
                }
                else if (determinant < 0)
                {
                    Winner = NegativePlayer;
                }
                else
                {
                    Winner = null;
                }

                IsCompleted = true;

                OnCompleted(this, new EventArgs());
            }
        }
    }
}

using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Models.Player;
using Determinant.Models;
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
        private bool _isCompleted;

        public IPlayer CurrentPlayer { get; private set; }
        public IPlayer PositivePlayer { get { return _players.First(x => x.Goal == PlayerGoal.Positive); } }
        public IPlayer NegativePlayer { get { return _players.First(x => x.Goal == PlayerGoal.Negative); } }
        
        private ComputerPlayer ComputerPlayer { get { return (ComputerPlayer)_players.FirstOrDefault(x => x is ComputerPlayer); } }

        public event EventHandler<GameCompletedEventArgs> OnCompleted;
        public event EventHandler<TurnEventArgs> OnHumanPlayerTurn;
        public event EventHandler<TurnEventArgs> OnComputerPlayerTurn;

        public Game(IEnumerable<IPlayer> players)
        {
            _players = players;
            _matrix = new Matrix3x3();
            _isCompleted = false;
        }

        public void MakeTurn(MatrixCell cell, int value)
        {
            ChangePlayer();
            MakeHumanPlayerTurn(cell, value);

            if (ComputerPlayer != null && !_isCompleted)
            {
                ChangePlayer();
                MakeComputerPlayerTurn();
            }
        }

        private void ChangePlayer()
        {
            CurrentPlayer = _players.First(x => x != CurrentPlayer);
        }

        private void MakeHumanPlayerTurn(MatrixCell cell, int value)
        {
            _matrix.SetValue(cell, value);             
            CheckIfCompleted();
            OnHumanPlayerTurn(this, new TurnEventArgs { Cell = cell, Value = value });
        }

        private TurnResult MakeComputerPlayerTurn()
        {
            var turnResult = ComputerPlayer.MakeTurn(_matrix);
            _matrix.SetValue(turnResult.Cell, turnResult.Value); 
            CheckIfCompleted();
            OnComputerPlayerTurn(this, new TurnEventArgs { Cell = turnResult.Cell, Value = turnResult.Value });

            return turnResult;
        }

        private void CheckIfCompleted()
        {
            if (_matrix.IsFull())
            {
                int determinant = _matrix.GetDeterminant();
                IPlayer winner;
                if (determinant > 0)
                {
                    winner = PositivePlayer;
                }
                else if (determinant < 0)
                {
                    winner = NegativePlayer;
                }
                else
                {
                    winner = null;
                }

                _isCompleted = true;
                OnCompleted(this, new GameCompletedEventArgs { Determinant = determinant, WinnerPlayer = winner });
            }
        }
    }
}

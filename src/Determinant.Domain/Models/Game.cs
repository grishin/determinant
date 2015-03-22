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



        }

        public TurnResult MakeTurn(int posx, int posy, int value)
        {
            TurnResult computerPlayerTurnResult = null;

            foreach (var player in _players)
            {
                if (player is HumanPlayer)
                {
                    _matrix.SetValue(posx, posy, value);
                }
                else if (player is ComputerPlayer)
                {
                    computerPlayerTurnResult = (player as ComputerPlayer).MakeTurn(_matrix);
                    _matrix.SetValue(computerPlayerTurnResult.Cell.Column, computerPlayerTurnResult.Cell.Row, computerPlayerTurnResult.Value);
                }

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

                    return null;
                }
            }

            return computerPlayerTurnResult;
        }

        private void CheckGameCompleted()
        {
            
        }

    }

    public enum Winner
    {
        None = 0,
        Positive = 1,
        Negative = 2,
        Draw = 3
    }
}

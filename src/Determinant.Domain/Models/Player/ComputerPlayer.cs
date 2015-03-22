using Determinant.Domain.Models.AI;
using Determinant.Domain.Models.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public class ComputerPlayer :IPlayer
    {
        private IComputerPlayerStrategy _strategy;
        private PlayerGoal _goal;

        public ComputerPlayer(IComputerPlayerStrategy strategy, PlayerGoal goal)
        {
            _strategy = strategy;
            _goal = goal;
        }

        public string Name
        {
            get { return _strategy.Name + " AI"; }
        }

        public PlayerGoal Goal
        {
            get { return _goal; }
        }

        public TurnResult MakeTurn(IMatrix matrix)
        {
            return _strategy.MakeTurn(matrix);
        }         
    }
}

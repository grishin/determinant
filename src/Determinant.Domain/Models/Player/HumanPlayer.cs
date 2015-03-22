using Determinant.Domain.Models.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public class HumanPlayer : IPlayer
    {
        public HumanPlayer(PlayerGoal goal)
        {
            Goal = goal;
        }

        public string Name
        {
            get { return "Human"; }
        }

        public PlayerGoal Goal { get; private set; }
       
    }
}

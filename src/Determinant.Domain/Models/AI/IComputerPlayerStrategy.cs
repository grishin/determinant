using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.AI
{
    public interface IComputerPlayerStrategy
    {
        TurnResult MakeTurn(Matrix3x3 matrix, PlayerGoal goal);

        string Name { get;}
    }
}

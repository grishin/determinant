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
        TurnResult MakeTurn(IMatrix matrix);

        string Name { get;}
    }
}

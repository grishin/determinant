using Determinant.Domain.Models.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public class AiPlayer : IPlayer
    {
        private IAI _ai;

        public AiPlayer(IAI ai)
        {
            _ai = ai;
        }

        public string GetName()
        {
            return "AI";
        }
    }
}

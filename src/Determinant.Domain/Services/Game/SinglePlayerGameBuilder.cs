using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Services.Game
{
    using Determinant.Domain.Models.AI;
    using Determinant.Domain.Models.Player;
    using Game = Determinant.Domain.Models.Game;

    public class SinglePlayerGameBuilder : IGameBuilder
    {
        public Game CreateGame()
        {
            var players = new IPlayer[] {
                  new HumanPlayer(),
                    new AiPlayer(new RandomAI())
            };

            return new Game(players);
        }
    }
}

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
            var playerGoals = new[] { PlayerGoal.Positive, PlayerGoal.Negative }.OrderBy(x => Guid.NewGuid()).ToArray();
            var players = new IPlayer[] { new HumanPlayer(playerGoals[0]), new ComputerPlayer(new ComputerPlayerStrategyRandom(), playerGoals[1]) }.OrderBy(x => Guid.NewGuid()).ToArray();

            return new Game(players);
        }
 
    }
}

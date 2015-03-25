using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Determinant.Domain.Models.AI;
using Determinant.Domain.Models.Player;

namespace Determinant.Domain.Services.Game
{
    using Game = Determinant.Domain.Models.Game;

    public class SinglePlayerGameBuilder : IGameBuilder
    {
        private readonly IComputerPlayerStrategy _computerPlayerStrategy;

        public SinglePlayerGameBuilder(IComputerPlayerStrategy computerPlayerStrategy)
        {
            _computerPlayerStrategy = computerPlayerStrategy;
        }

        public Game CreateGame()
        {
            var playerGoals = new[] { PlayerGoal.Positive, PlayerGoal.Negative }.OrderBy(x => Guid.NewGuid()).ToArray();
            var players = new IPlayer[] { new HumanPlayer(playerGoals[0], "Human"), new ComputerPlayer(_computerPlayerStrategy, playerGoals[1]) }.OrderBy(x => Guid.NewGuid()).ToArray();

            return new Game(players);
        }
 
    }
}

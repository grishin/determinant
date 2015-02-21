using System;
using System.Drawing ;

namespace MatrixGame.Screens
{
	public class SingleNameInputScreen:NamesInputScreen
	{
		public SingleNameInputScreen(GAME f,Graphics g):base(f,g)
		{
			this.p2 = p2;
		}

		public override void Display()
		{
			base.Display ();
		}

		protected override void DrawHelpText()
		{
			//пояснительная надпись
			gx.DrawString ("Now you should enter",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+10);
			gx.DrawString ("your name using the ",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+30);
			gx.DrawString ("keypad below or choose",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+50);
			gx.DrawString ("a name from list...",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+70);
		}

		protected override void Continue()
		{
			f.Pl1 = PlayerFactoryExtended.CreatePosHumanPlayer (name);
			f.Pl2 = PlayerFactoryExtended.CreateNegAiPlayer (f.mMatrix );
			
			if (GAME.RandomSideSelection )
			{
				f.RandomSides ();
			}
			else
			{
				f.StandartAssign ();
			}

				if (GAME.PositiveAlwaysStarts )
					AbstractPlayer.IsPosTurn = false; 



				f.CurrentScreen = new GameScreen (f,gx,f.mMatrix ,f.PosPlayer,f.NegPlayer );
			f.CurrentScreen.Display ();
			f.GameLoop ();
		}
	
		

	}
}


using System;
using System.Windows.Forms;
using System.Drawing ;
using System.IO ;

namespace MatrixGame.Screens
{

	public class GameResultScreen:AbstractScreen
	{
		private Matrix m;
		Rectangle CloseRect = new Rectangle (50,190,50,15);
		Rectangle PlayAgainRect = new Rectangle (120,190,70,17);

		public GameResultScreen(GAME f,Graphics gx,AbstractPlayer p1,AbstractPlayer p2,Matrix m):base(f,gx)
		{
			this.p1 =p1;
			this.p2 =p2;
			this.m = m;
		}

		public override void  Display()
		{
			base.DisplayWithBlackBackground ();

			//подсказка в панели имён
			gx.DrawString ("Game results",new Font ("Times",10,FontStyle.Bold ),new SolidBrush (Color.Red),75,247);

			String winname="";     
			if (p1.IsWinner )
				winname = p1.Name ;
			else if (p2.IsWinner )
				winname = p2.Name ;
				else
				winname = "Deuce";

			//вывод результатов
			gx.DrawString ("Winner: " + winname,MenuFont,MenuBrush,50,70);
			gx.DrawString ("Score: " + f.mMatrix.getDeterminant ().ToString (),MenuFont,MenuBrush,50,90);

			gx.DrawString ("Close",MenuFont,MenuBrush,CloseRect);
			gx.DrawString ("Play Again",MenuFont,MenuBrush,PlayAgainRect);

			f.RefreshScreen ();
		}

		public override void CheckClicks(int x,int y)
		{
			base.CheckClicks (x,y);
			if (CloseRect.Contains (x,y))
				CloseButton_Click();
			else if (PlayAgainRect.Contains (x,y))
				PlayAgainButton_Click();
		}

		public void CloseButton_Click()
		{
			if (GAME.PositiveAlwaysStarts )
				AbstractPlayer.IsPosTurn = false; 

			f.mMatrix.Clear ();
			f.CurrentScreen = new MainMenuScreen(f,gx);
			f.CurrentScreen.Display ();
			f.RefreshScreen ();
		}

		public void PlayAgainButton_Click()
		{

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

			f.mMatrix.Clear ();
			f.CurrentScreen = new GameScreen (f,gx,f.mMatrix,f.PosPlayer,f.NegPlayer );
			f.CurrentScreen.Display ();
			f.GameLoop ();
		}

	}
}

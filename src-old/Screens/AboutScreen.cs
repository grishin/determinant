using System;
using System.Drawing ;
using System.Threading ;

namespace MatrixGame.Screens
{
	public class AboutScreen:AbstractScreen
	{
		protected Rectangle BackRect = new Rectangle (50,190,50,15);

		public AboutScreen(GAME f,Graphics gx):base(f,gx)
		{

		}
		private void DisplayScreen1()
		{
			gx.DrawString ("Matrix Game v 1.0",MenuFont,MenuBrush,VSCREEN_LEFT+20,VSCREEN_TOP+10);
			gx.DrawString ("Pocket PC Edition",MenuFont,MenuBrush,VSCREEN_LEFT+20,VSCREEN_TOP+25);

			gx.DrawString ("Thank you for trying",MenuFont,MenuBrush,VSCREEN_LEFT+10,VSCREEN_TOP+55);
			gx.DrawString ("the full version of ",MenuFont,MenuBrush,VSCREEN_LEFT+20,VSCREEN_TOP+70);

			gx.DrawString ("Matrix Game ",MenuFont,MenuBrush,VSCREEN_LEFT+30,VSCREEN_TOP+85);

			gx.DrawString ("For support visit ",MenuFont,MenuBrush,VSCREEN_LEFT+20,VSCREEN_TOP+110);
			gx.DrawString ("www.pocketmatrix.h15.ru",MenuFont,MenuBrush,VSCREEN_LEFT+2,VSCREEN_TOP+125);
			
			//	gx.DrawString ("Back",MenuFont,MenuBrush,BackRect);
			f.RefreshScreen ();
		}

		private void DisplayScreen2()
		{
			base.DisplayWithBlackBackground ();
			gx.DrawString ("Concept&Code",MenuFont,MenuBrush,VSCREEN_LEFT+30,VSCREEN_TOP+50);
			gx.DrawString ("by",MenuFont,MenuBrush,VSCREEN_LEFT+70,VSCREEN_TOP+65);
			gx.DrawString ("Andrej Grishin (lumini)",MenuFont,MenuBrush,VSCREEN_LEFT+10,VSCREEN_TOP+80);
			//gx.DrawString ("Back",MenuFont,MenuBrush,BackRect);
			f.RefreshScreen ();
		}

		public override void Display()
		{
			
			base.DisplayWithBlackBackground ();
			this.CreateControlsTemplate ();
			DisplayScreen1();
			//Timer t = new Timer (DisplayScreen2(),null,4000,Timeout.Infinite );
			DisplayScreen1();
			Thread.Sleep (4000);
			DisplayScreen2();
		//	Thread.Sleep (4000);
			
			Thread.Sleep (4000);

			base.DisplayWithBlackBackground ();
			gx.DrawString ("Graphics",MenuFont,MenuBrush,VSCREEN_LEFT+50,VSCREEN_TOP+50);
			gx.DrawString ("by",MenuFont,MenuBrush,VSCREEN_LEFT+70,VSCREEN_TOP+65);
			gx.DrawString ("Vladius",MenuFont,MenuBrush,VSCREEN_LEFT+55,VSCREEN_TOP+80);
			//gx.DrawString ("Back",MenuFont,MenuBrush,BackRect);
			f.RefreshScreen ();
			Thread.Sleep (4000);

			base.DisplayWithBlackBackground ();
			gx.DrawString ("IDEAs & BetaTesting",MenuFont,MenuBrush,VSCREEN_LEFT+20,VSCREEN_TOP+50);
			gx.DrawString ("by",MenuFont,MenuBrush,VSCREEN_LEFT+70,VSCREEN_TOP+65);
			gx.DrawString ("Nikita1580",MenuFont,MenuBrush,VSCREEN_LEFT+42,VSCREEN_TOP+80);
			//gx.DrawString ("Back",MenuFont,MenuBrush,BackRect);
			f.RefreshScreen ();
			Thread.Sleep (4000);

			base.DisplayWithBlackBackground ();
			gx.DrawString ("Additional BetaTesting",MenuFont,MenuBrush,VSCREEN_LEFT+10,VSCREEN_TOP+50);
			gx.DrawString ("by",MenuFont,MenuBrush,VSCREEN_LEFT+70,VSCREEN_TOP+65);
			gx.DrawString ("Steven",MenuFont,MenuBrush,VSCREEN_LEFT+55,VSCREEN_TOP+80);
			f.RefreshScreen ();
			Thread.Sleep (4000);

			base.DisplayWithBlackBackground ();
			gx.DrawString ("And remember:",MenuFont,MenuBrush,VSCREEN_LEFT+30,VSCREEN_TOP+40);
			gx.DrawString ("This game is donationware",MenuFont,MenuBrush,VSCREEN_LEFT+0,VSCREEN_TOP+65);
			gx.DrawString ("Please see readme file ",MenuFont,MenuBrush,VSCREEN_LEFT+11,VSCREEN_TOP+80);
			gx.DrawString ("to know how you can ",MenuFont,MenuBrush,VSCREEN_LEFT+15,VSCREEN_TOP+95);
			gx.DrawString ("pay the author",MenuFont,MenuBrush,VSCREEN_LEFT+33,VSCREEN_TOP+110);
			f.RefreshScreen ();
			Thread.Sleep (4000);
			CloseButton_Click();
		}

		protected void CreateControlsTemplate()
		{

		}

		public override void CheckClicks(int x,int y)
		{
			base.CheckClicks (x,y);
			if (BackRect.Contains (x,y))
				CloseButton_Click();
		}

		protected void CloseButton_Click()
		{
			f.Controls.Clear ();
			f.CurrentScreen = new MainMenuScreen (f,gx) ;
			f.CurrentScreen.Display ();
			f.RefreshScreen ();
		}
	}
}

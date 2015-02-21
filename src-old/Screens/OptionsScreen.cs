using System;
using System.Drawing ;
using System.Windows.Forms ;

namespace MatrixGame.Screens
{
	public class OptionsScreen:AboutScreen
	{
		const int ITEMS=2;							//число элементов главного меню
		const int ITEM_START_Y=VSCREEN_TOP+40;			
		const int ITEM_HEIGHT=20;

		//список активных мест
		private Rectangle[] HSpots = new Rectangle[ITEMS]; 

		public OptionsScreen(GAME f,Graphics gx):base(f,gx)
		{
			for (int i=0;i<ITEMS;i++)
			{
				HSpots[i] = new Rectangle (VSCREEN_LEFT,VSCREEN_TOP+40+20*i,200,ITEM_HEIGHT);
			}
		}

		public override void Display()
		{
			base.DisplayWithBlackBackground ();
			gx.DrawString ("Options List",MenuFont,MenuBrush,VSCREEN_LEFT+50,VSCREEN_TOP+10);
			gx.DrawString ("Random Side Selection",MenuFont,GetBrush(GAME.RandomSideSelection ),VSCREEN_LEFT+10,VSCREEN_TOP+40);
			gx.DrawString ("P- Is Allowed To Start",MenuFont,GetBrush(GAME.PositiveAlwaysStarts),VSCREEN_LEFT+10,VSCREEN_TOP+60);

			gx.DrawString ("Back",MenuFont,MenuBrush,BackRect);
			f.RefreshScreen ();
		}

		private Brush GetBrush(bool option)
		{	Brush ActiveBrush = new SolidBrush (Color.Yellow );
			Brush PassiveBrush = new SolidBrush(Color.Gray);

			if (option)
				return ActiveBrush;
			else
				return PassiveBrush;
		}

		public override void CheckClicks(int x, int y)
		{	
			
			if (HSpots[0].Contains (x,y))				
			{				
				GAME.RandomSideSelection = !GAME.RandomSideSelection ;
				FileWriter.SaveOptions ();
			}
			else if (HSpots[1].Contains (x,y))			
			{
				GAME.PositiveAlwaysStarts = !GAME.PositiveAlwaysStarts ;
				FileWriter.SaveOptions ();
			}
			this.Display ();	
			base.CheckClicks (x,y);		
		}
 
	}
}

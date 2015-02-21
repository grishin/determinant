
#define FULL
//#define GRAPH_DEBUG_MODE		//включить режим теста графики
//#define DEMO
using System;
using System.Drawing;
using System.IO ;
using System.Windows.Forms;
using MatrixGame;

namespace MatrixGame.Screens
{
	public class MainMenuScreen:AbstractScreen
	{
		const int ITEMS=5;							//число элементов главного меню
		const int ITEM_START_Y=80;			
		const int ITEM_HEIGHT=23;

		public static int debug_test=0;

		//список активных мест
		private Rectangle[] HSpots = new Rectangle[ITEMS]; 

		#region Инициализация данных
		public MainMenuScreen(GAME f,Graphics gx):base(f,gx)
		{
			InitHSpotRectangles ();
			f.ClickFlag = true;
		}

		private void InitHSpotRectangles()
		{
			for (int i=0;i<ITEMS;i++)
			{
				HSpots[i] = new Rectangle (0,ITEM_START_Y+ITEM_HEIGHT*i,240,ITEM_HEIGHT);
			}
		}

		#endregion

		#region Проверка кликов
		public override void CheckClicks(int x,int y)
		{
			base.CheckClicks (x,y);
			if (HSpots[0].Contains (x,y))				//два игрока
			{	
				GAME.IsMultiplayer  = true;
			//	NamesInputScreen.statecnt = 0;			
				f.CurrentScreen  = new NamesInputScreen (f,gx);
				debug_test=1;
				f.CurrentScreen.Display ();
			
			}
#if FULL
			else if (HSpots[1].Contains (x,y))			//один игрок
			{
				GAME.IsMultiplayer = false;
				f.CurrentScreen = new  SingleNameInputScreen (f,gx);
				f.CurrentScreen.Display ();
				
			}
			else if (HSpots[2].Contains (x,y))			//опции
			{
				f.CurrentScreen = new OptionsScreen (f,gx);
				f.CurrentScreen.Display ();
				
			}
#endif
			else if(HSpots[3].Contains (x,y))			//выход
			{
				f.CurrentScreen = new AboutScreen (f,gx);
				f.CurrentScreen.Display ();
			}
			else if (HSpots[4].Contains (x,y))
			{
				f.Close ();
			}
		}
		#endregion

		#region Отображение данных
		public override void Display()
		{		
			base.DisplayWithBlackBackground ();
			
			//отображение меню
#if DEMO
			gx.DrawString ("Matrix Game (Lite)",MenuFont,MenuBrush,60,ITEM_START_Y-ITEM_HEIGHT-10);
#else
			gx.DrawString ("Matrix Game",MenuFont,MenuBrush,77,ITEM_START_Y-ITEM_HEIGHT-10);
#endif
			gx.DrawString ("Human vs Human",MenuFont,MenuBrush,63,ITEM_START_Y);
#if DEMO
			gx.DrawString ("Human vs Comp",MenuFont,new SolidBrush(Color.Gray ),68,ITEM_START_Y+ITEM_HEIGHT);
			gx.DrawString ("Options",MenuFont,new SolidBrush(Color.Gray ),91,ITEM_START_Y+2*ITEM_HEIGHT);
#else
			gx.DrawString ("Human vs Comp",MenuFont,MenuBrush,68,ITEM_START_Y+ITEM_HEIGHT);
			gx.DrawString ("Options",MenuFont,MenuBrush,91,ITEM_START_Y+2*ITEM_HEIGHT);
#endif
			gx.DrawString ("About",MenuFont,MenuBrush,98,ITEM_START_Y+3*ITEM_HEIGHT);
			gx.DrawString ("Exit",MenuFont,MenuBrush,104,ITEM_START_Y+4*ITEM_HEIGHT);
			
			//подсказка в панели имён
			gx.DrawString ("Your choice:",new Font ("Times",10,FontStyle.Bold ),new SolidBrush (Color.Red),75,247);
					
#if GRAPH_DEBUG_MODE			
				for (int i=0;i<ITEMS;i++)
					gx.DrawRectangle (new Pen(Color.Red ),HSpots[i]);
#endif
			f.RefreshScreen ();
		}
		#endregion
	}
}

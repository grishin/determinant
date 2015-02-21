//#define GRAPH_DEBUG_MODE

using System;
using System.Drawing ;
using System.IO ;
using System.Windows.Forms ;
using MatrixGame;

namespace MatrixGame.Screens
{
	public class NamesInputScreen:AbstractScreen	
	{
		protected PlayerFactory fact;
		protected Rectangle[] LetPadRect = new Rectangle[13];

		const int MAX_NAME_LENGTH=6;

		const int NAMELIST_TOP = VSCREEN_TOP+100;
		const int NAMELIST_DISTANCE=15;
		const int NAMELIST_ITEMS=4;

		protected String name="";

		protected Rectangle DelRect;
		protected Rectangle OkRect;
		protected Rectangle MoveDown;
		protected Rectangle MoveUp;

		protected Rectangle[] NamelistRects = new Rectangle [NAMELIST_ITEMS]; 
		protected String[] NamelistStandartItems = new String [NAMELIST_ITEMS];
	
		
		//шрифт панели ввода букв
		Brush LetPadBrush = new SolidBrush (Color.Yellow);
		Font LetPadFont = new Font ("Courier New",10,FontStyle.Regular);
		
		//размеры клеток панели букв
		const int NUMPAD_CELL_SIZE=11;
		const int NUMPAD_CELL_DISTANCE=1;

		int istart;						//ASCII - код начальной буквы в панели ввода (меняется при помощи istate)
		int istate = 0; //0..3			//Характеризует состояние панели ввода (набор отображаемых символов)

		public static int statecnt=0;
		private bool IsNameEdited=false;

		#region Инициализация данных
		public NamesInputScreen(GAME f,Graphics gx):base(f,gx)
		{
			this.p1 = p1;
			this.p2 = p2;

			InitLetPadRects ();
			InitDelOkRect();
			InitNamelistRects();
			InitNamelistStandartItems();
			changeistart();
			statecnt++;
		}

		protected void InitNamelistStandartItems()
		{
			try
			{
				FileWriter.LoadNamelist (NamelistStandartItems);
				}
			catch(IOException e)
			{
				NamelistStandartItems[0] = "Andrej"; 
				NamelistStandartItems[1] = "Nikita"; 
				NamelistStandartItems[2] = "Steven"; 
				NamelistStandartItems[3] = "Betty"; 
			}
		}

		protected void InitLetPadRects()
		{
			for (int i=0;i<13;i++)
			{
				LetPadRect[i] = new Rectangle (NUMPAD_LEFT+(NUMPAD_CELL_SIZE+NUMPAD_CELL_DISTANCE)*(i),NUMPAD_TOP,NUMPAD_CELL_SIZE,NUMPAD_CELL_SIZE);
			}
			MoveUp = new Rectangle (NUMPAD_LEFT+157,NUMPAD_TOP+1,NUMPAD_CELL_SIZE,NUMPAD_CELL_SIZE/2+1);
			MoveDown = new Rectangle (NUMPAD_LEFT+157,NUMPAD_TOP+NUMPAD_CELL_SIZE/2+4,NUMPAD_CELL_SIZE,NUMPAD_CELL_SIZE/2+1);

		}

		protected void InitDelOkRect()
		{
			DelRect = new Rectangle (142,249,13,13);
			OkRect = new Rectangle (158,249,20,13);
		}

		protected void InitNamelistRects()
		{
			for (int i=0;i<NAMELIST_ITEMS;i++)
			{
				NamelistRects[i] = new Rectangle (VSCREEN_LEFT,NAMELIST_TOP+i*NAMELIST_DISTANCE,VSCREEN_SIZE,NAMELIST_DISTANCE);
			}
		}

		#endregion 

		#region Проверка кликов
		protected void CheckLetPadClicks(int x,int y)
		{
			
			for (int i=0;i<13;i++)
			{
				if (LetPadRect[i].Contains (x,y))
				{
					if (name.Length <MAX_NAME_LENGTH)
					{
						name+=((char)(i+istart)).ToString ();
						IsNameEdited = true;
					}
				}
			}

			CheckMoveDownClicks(x,y);
			CheckMoveUpClicks(x,y);		
		}

		protected void CheckDelClicks(int x,int y)
		{
			if (DelRect.Contains (x,y))
			{
				name = name.Substring (0,name.Length -1);
				IsNameEdited = true;
			}

		}

		protected void CheckOkClicks(int x,int y)
		{
			if (OkRect.Contains (x,y))
			{
				if (name!="")
				{
					if (IsNameEdited)
					{
						AddToNamelist (name);
					}
					Continue();
				}
			}
		}

		protected void CheckMoveUpClicks(int x,int y)
		{
			if (MoveUp.Contains (x,y))
			{
				deci();
				changeistart();
				Display();
			}
		}

		protected void CheckNamelistClicks(int x,int y)
		{
			for (int i=0;i<NAMELIST_ITEMS;i++)
			{
				if (NamelistRects[i].Contains (x,y))
				{
					name = NamelistStandartItems[i];
					IsNameEdited=false;
					MoveOnTop(i);
					break;
				}
			}

		}

		protected void CheckMoveDownClicks(int x,int y)
		{
			if (MoveDown.Contains (x,y))
			{
				inci();
				changeistart();
				Display();
			}
		}

		public override void CheckClicks(int x, int y)
		{
			CheckLetPadClicks (x,y);
			CheckDelClicks(x,y);
			CheckNamelistClicks(x,y);
			Display ();
			CheckOkClicks(x,y);
		}

		#endregion

		#region Отображение данных

		public override void Display()
		{
			base.DisplayWithBlackBackground ();			
			DrawLetPad();
			DrawHelpText();
			DrawNamelist();
			//gx.DrawString ("X",new Font ("Times",8,FontStyle.Bold   ),new SolidBrush (Color.Black),QuitRect);

#if GRAPH_DEBUG_MODE
			for (int i=0;i<NAMELIST_ITEMS;i++)
				gx.DrawRectangle (new Pen(Color.Red),NamelistRects[i]);
#endif
			//отображает строку с именем игрока
			gx.DrawString (name,new Font ("Times",8,FontStyle.Bold ),new SolidBrush (Color.Red),65,247);
			f.RefreshScreen ();
		}

		private void DrawLetPad()
		{
			//рисуем буквы
			for (int i=istart;i<istart+13;i++)
			{			
				gx.DrawString (((char)i).ToString (),LetPadFont,LetPadBrush,
					NUMPAD_LEFT+(NUMPAD_CELL_SIZE+NUMPAD_CELL_DISTANCE)*(i-istart),NUMPAD_TOP);
			}
			//..кнопку удаления
			gx.DrawString ("<",new Font ("Times",10,FontStyle.Bold ),new SolidBrush (Color.Red),143,248);
			gx.DrawString ("ok",new Font ("Times",10,FontStyle.Bold ),new SolidBrush (Color.Red),160,248);
			gx.DrawRectangle (new Pen (Color.Red ),DelRect);
			gx.DrawRectangle (new Pen (Color.Red ),OkRect);
			//..кнопки вверх-вниз
			gx.FillRectangle (new SolidBrush (Color.Yellow ),MoveDown);
			gx.FillRectangle (new SolidBrush (Color.Yellow ),MoveUp);
			gx.FillEllipse (new SolidBrush (Color.Red  ),NUMPAD_LEFT+161,NUMPAD_TOP+2,2,2);
			gx.FillEllipse (new SolidBrush (Color.Red  ),NUMPAD_LEFT+161,NUMPAD_TOP+10,2,2);
		}

		protected virtual void DrawHelpText()
		{
			//пояснительная надпись
			if (GAME.RandomSideSelection )
			{
				gx.DrawString ("Player "+statecnt.ToString ()+" should enter",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+10);
			}
			else
			{
				String s;
				if (statecnt==1)
					s="P+ player ";
				else if (statecnt==2)
					s = "N- player";
				else s="??";
				gx.DrawString (s+" should enter",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+10);

			}
			gx.DrawString ("his name using the ",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+30);
			gx.DrawString ("keypad below or choose",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+50);
			gx.DrawString ("a name from list...",MenuFont,MenuBrush,VSCREEN_LEFT+5,VSCREEN_TOP+70);

		}

		protected void DrawNamelist()
		{
			for (int i=0;i<NAMELIST_ITEMS;i++)
				gx.DrawString (NamelistStandartItems[i],MenuFont,MenuBrush,VSCREEN_LEFT+5,NAMELIST_TOP+NAMELIST_DISTANCE*i);
		}
		#endregion

		#region Вспомогательные методы

		protected virtual void Continue()
		{
			switch(statecnt)
			{
				case 1:
					f.Pl1 = PlayerFactoryExtended.CreatePosHumanPlayer (name);
					f.CurrentScreen = new NamesInputScreen (f,gx);
					break;
				case 2:
					f.Pl2 = PlayerFactoryExtended.CreatePosHumanPlayer (name);
					f.CurrentScreen = new GameScreen (f,gx,f.mMatrix ,f.PosPlayer,f.NegPlayer );
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

					statecnt = 0;
					f.GameLoop ();
					break;
			}
			f.CurrentScreen.Display ();
		}

		protected void changeistart()
		{
			switch (istate)
			{
				case 0:
					istart = 65;
					break;
				case 1:
					istart = 78;
					break;
				case 2:
					istart = 97;
					break;
				case 3:
					istart = 110;
					break;

			}
		}

		protected void inci()
		{
			istate++;
			if (istate>3)
				istate=0;
		}

		protected void deci()
		{
			istate--;
			if (istate<0)
				istate=3;
		}

		private void AddToNamelist(String name)
		{
			String[] ret = new String[NAMELIST_ITEMS];
			ret[0]=name;
			for (int i = 0;i<NAMELIST_ITEMS-1;i++)
			{
				ret[i+1] = NamelistStandartItems[i];
			}
			NamelistStandartItems = ret;
			FileWriter.SaveNamelist (ret);
		}

		private void MoveOnTop(int pos)
		{
			if (pos!=0)
			{
			String temp = NamelistStandartItems[pos];
				for (int i=pos;i>0;i--)
				{
					NamelistStandartItems[i]=NamelistStandartItems[i-1];
				}
				NamelistStandartItems[0]=temp;
				FileWriter.SaveNamelist (NamelistStandartItems);
			}
		}

		#endregion

	}
}

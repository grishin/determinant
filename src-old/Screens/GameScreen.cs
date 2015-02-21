using System;
using System.IO ;
using System.Drawing ;
using System.Windows.Forms;

namespace MatrixGame.Screens
{
	public class GameScreen:AbstractScreen
	{
		const int TOP_LEFT_FIELD_RECT_X=31;
		const int TOP_LEFT_FIELD_RECT_Y=37;

		const int FIELD_RECT_SIZE=56;			//размер квадрата
		const int FIELD_RECT_SPACE=5;			//промежуток между квадратами
		const int FIELD_RECT_OFFSET=15;
	
		const int NUMPAD_TOP=278;
		new const int NUMPAD_LEFT=35;
		new const int NUMPAD_CELL_SIZE=17;
		const int NUMPAD_CELL_DISTANCE=2;
	
		const int QUIT_RECT_SIZE=10;
		const int QUIT_RECT_X = 210;
		const int QUIT_RECT_Y = 20;

		const int NAMES_X = 75;
		const int NAMES_Y = 248;

		const int BALL_SIZE=3;					//размер указателя на текущего игрока

		
		private Rectangle[,] FieldRects = new Rectangle [3,3];
		private Rectangle[] NumpadRects = new Rectangle[9];
	
		private int ChosenFieldRectX=-1;			//принимает значения  0..2 (-1;-1 -если ничего не выбрано) 
		private int ChosenFieldRectY=-1;			//принимает значения  0..2


		public GameScreen(GAME f,Graphics gx,Matrix m,AbstractPlayer p1,AbstractPlayer p2):base(f,gx)
		{
			mMatrix = m;
			InitFieldRects();
			InitNumpad();

			this.p1 = p1;
			this.p2 = p2;

			f.PosPlayer = p1;
			f.NegPlayer = p2;

			f.LoopBreak = false;

			this.Restart ();
		}
		
		public override void Display()
		{
			base.Display ();
			DrawFieldRects();
		//	DrawControls();				//временно отключено
			DrawNumpad();
			DrawPlayers();
			DrawBall();
			gx.DrawString ("X",new Font ("Times",8,FontStyle.Bold   ),new SolidBrush (Color.Black),QuitRect);
			f.RefreshScreen ();
		}

		public void InitFieldRects()
		{
			FieldRects[0,0] = new Rectangle (33,38,54,55);
			FieldRects[0,1] = new Rectangle (92,38,56,54);
			FieldRects[0,2] = new Rectangle (153,38,54,55);

			FieldRects[1,0] = new Rectangle (33,97,55,57);
			FieldRects[1,1] = new Rectangle (92,98,56,56);
			FieldRects[1,2] = new Rectangle (153,97,55,57)
				;
			FieldRects[2,0] = new Rectangle (33,159,54,54);
			FieldRects[2,1] = new Rectangle (92,159,56,54);
			FieldRects[2,2] = new Rectangle (153,158,55,55);
		}

		public void InitNumpad()
		{
			for (int i=0;i<9;i++)
			{
				NumpadRects[i] = new Rectangle (NUMPAD_LEFT+(NUMPAD_CELL_SIZE+NUMPAD_CELL_DISTANCE)*i,NUMPAD_TOP,
										NUMPAD_CELL_SIZE,NUMPAD_CELL_SIZE);
			}
		}

		public override void CheckClicks(int x,int y)
		{
				base.CheckClicks (x,y);
				this.CheckFieldRectClicks (x,y);
				this.CheckNumpadClicks (x,y);
				this.Display ();				
				f.RefreshScreen ();
				CheckQuitClick (x,y);
		}

		private void CheckFieldRectClicks(int x,int y)
		{
			for (int i=0;i<3;i++)
				for (int j=0;j<3;j++)
				{
					if (FieldRects[i,j].Contains (x,y))
					{
						if (mMatrix.getElement (i,j)==0)
						{
							ChosenFieldRectX=i;
							ChosenFieldRectY=j;
						}
					}
				}
		}

		protected void CheckQuitClick(int x,int y)
		{
			if (QuitRect.Contains (x,y))
			{
				mMatrix.MakeItFull ();
                f.LoopBreak = true;		
				
				f.CurrentScreen = new MainMenuScreen (f,gx);
				f.ClickFlag = true;
				f.CurrentScreen.Display ();
				f.RefreshScreen ();		

			}
		}

		private void CheckNumpadClicks(int x,int y)
		{
				for (int i=0;i<9;i++)
				{				
					if (NumpadRects[i].Contains (x,y))
					{
						if (mMatrix.IsDeleted (i+1)==false)
						{
							if (ChosenFieldRectX!=-1)
							{
								mMatrix.setElement (ChosenFieldRectX,ChosenFieldRectY,i+1);
								ChosenFieldRectX=-1;							
								this.Display ();
								f.RefreshScreen ();
								f.ClickFlag  = true;
							}
						}
					}
				}
		}

		private void DrawPlayers()
		{
			gx.DrawString (f.PosPlayer .Name + " vs "+ f.NegPlayer.Name ,new Font ("TIMES",8,FontStyle.Bold ),new SolidBrush (Color.Red   ),
				NAMES_X,NAMES_Y );
		}

		private void DrawBackground()
		{
			gx.DrawImage (bmpBack,0,0);
		}

		private void DrawBall()
		{
			if (GAME.IsMultiplayer )
			{
				if (!HumanPlayer.IsPosTurn )
					//gx.FillEllipse (new SolidBrush(Color.PowderBlue  ),50,255,BALL_SIZE,BALL_SIZE);
					gx.DrawString ("+",new Font ("TIMES",10,FontStyle.Bold ),new SolidBrush(Color.PowderBlue  ),45,248);
				else 
					//gx.FillEllipse (new SolidBrush(Color.PowderBlue  ),187,255,BALL_SIZE,BALL_SIZE);
					gx.DrawString ("-",new Font ("TIMES",10,FontStyle.Bold ),new SolidBrush(Color.PowderBlue  ),187,248);
			}
			else
			{
					gx.DrawString ("+",new Font ("TIMES",10,FontStyle.Bold ),new SolidBrush(Color.PowderBlue  ),45,248);
					gx.DrawString ("-",new Font ("TIMES",10,FontStyle.Bold ),new SolidBrush(Color.PowderBlue  ),187,248);
			}
		
		}

		private void DrawFieldRects()
		{
			for (int i=0;i<3;i++)
				for (int j=0;j<3;j++)
				{
					if ((ChosenFieldRectX==i)&&(ChosenFieldRectY==j))
						gx.FillRectangle (new SolidBrush (Color.CornflowerBlue  ),FieldRects[i,j]);
				}

			//рисуем цифры					
			for (int i=0;i<3;i++)
				for (int j=0;j<3;j++)
				{
					if (mMatrix.getElement (i,j) != 0)
						DrawCellNumber(j,i,mMatrix.getElement (i,j));
				}
		}

		private void DrawControls()
		{
			Pen p = new Pen (Color.LightGray);
			gx.DrawRectangle (p,QuitRect);
			gx.DrawLine (p,QUIT_RECT_X,QUIT_RECT_Y,QUIT_RECT_X+QUIT_RECT_SIZE,QUIT_RECT_Y+QUIT_RECT_SIZE);
			gx.DrawLine (p,QUIT_RECT_X+QUIT_RECT_SIZE,QUIT_RECT_Y,QUIT_RECT_X,QUIT_RECT_Y+QUIT_RECT_SIZE);
		}

		private void DrawNumpad()
		{
			int c=0;
			for (int i=0;i<9;i++)
			{
				c=i+1;
				if (!mMatrix.IsDeleted (c))
				{
					//gx.DrawRectangle (new Pen(Color.Blue ) ,NumpadRects[i]);
					gx.DrawString (c.ToString (),new Font ("TIMES",10,FontStyle.Bold  ),new SolidBrush (Color.Yellow ),
						NumpadRects[i].Left +5,NumpadRects[i].Top );
				}	
			}
		}
		private void DrawCellNumber(int x,int y,int number)
		{
			gx.DrawString (number.ToString (),new Font("Arial",32,FontStyle.Regular ),new SolidBrush (Color.OrangeRed   ),
				TOP_LEFT_FIELD_RECT_X+(FIELD_RECT_SIZE+FIELD_RECT_SPACE)*x+FIELD_RECT_OFFSET,
				TOP_LEFT_FIELD_RECT_Y+(FIELD_RECT_SIZE+FIELD_RECT_SPACE)*y);
		}

		public void Restart()
		{
			mMatrix.Clear ();
			InitNumpad();
			DrawNumpad();
		}
	}
}

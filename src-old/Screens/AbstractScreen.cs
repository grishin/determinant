using System;
using MatrixGame;
using System.Drawing;

namespace MatrixGame.Screens
{
	public interface IScreen
	{
		 void Display();
		 void CheckClicks(int x,int y);
	}

	public abstract class AbstractScreen
	{
		protected GAME f;												//главная форма
		protected Graphics gx;											//здесь рисуем
		protected AbstractPlayer p1,p2;									//игроки
		protected Bitmap bmpBack = GAME.LoadBitmap ("Game.pnh");		//битмэп фона 
		protected Matrix mMatrix;										//рабочая матрица

		protected Rectangle QuitRect = new Rectangle (30,237,10,10);

		//стандартный шрифт "виртуального экрана"
		protected Brush MenuBrush = new SolidBrush (Color.LightGreen);
		protected Font MenuFont = new Font ("Times",10,FontStyle.Bold);

		//позиция "виртуального экрана"
		public const int VSCREEN_TOP = 31;
		public const int VSCREEN_LEFT = 37;
		public const int VSCREEN_SIZE = 178;

		//позиция панели ввода цифр
		public const int NUMPAD_TOP=278;
		public const int NUMPAD_LEFT=38;

		public AbstractScreen(GAME f,Graphics gx)
		{
			this.f = f;
			this.gx = gx;
			this.p1 = p1;
			this.p2 = p2;
		}

		public virtual void CheckClicks(int x,int y)					//Проверка события On_Click
		{
			//метод будет переопределён	
			//CheckQuitClick(x,y);
		}

		public virtual void Display()
		{
			gx.DrawImage (bmpBack,0,0);
		}

		public void DisplayWithBlackBackground()
		{
			gx.DrawImage (bmpBack,0,0);	
			gx.FillRectangle (new SolidBrush (Color.Black ),new Rectangle (VSCREEN_TOP,VSCREEN_LEFT
				,VSCREEN_SIZE,VSCREEN_SIZE));
		}



	}
}

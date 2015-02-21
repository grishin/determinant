#define POCKET					//define in NamesInputScreen

//////////////////////////////////////////
///���������� ������:
///3. ��������� ���������� ����� P+ Always starts first
////////////////////////////////////////////////////// 

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection ;		
using System.Runtime.InteropServices;
using System.Threading ;

using MatrixGame.Screens ;

namespace MatrixGame
{	
	public class GAME : Form
	{ 
		private Graphics gxOff,gxOn;				
		private Bitmap bmpOff;

		public AbstractPlayer PosPlayer,NegPlayer;
		public AbstractPlayer Pl1,Pl2;
		public AbstractScreen CurrentScreen; 
		public Matrix mMatrix = new Matrix();

		public bool LoopBreak = false;
		
		# region ���������� ������
		public static bool IsMultiplayer;					//1- ���� ��������������������� ����
		public bool ClickFlag=true;							//1 - ����� ������� �� �������� ������
		#endregion

		#region ������� �����
		public static bool RandomSideSelection=false;		//1 - ������� ������ ���������� ��������� �������
															//0 - ����� ��� �������� ���� �������

		public static bool PositiveAlwaysStarts = true;		//1 - ������ �������� ������������ �����
															//0 - ������ ��� ������������ ��������
	
		//Negat starts -	ClickFlag=True
		//					IsPlayerMyTurn=false
		//Pos starts   -    IsPlayerMyTurn=true
							
		#endregion
	 
		#region ������ �������� �����������
		public static String AppPath					//�������� ���� � ����� ���������
		{	
			get
			{
				String fullAppName = Assembly.GetExecutingAssembly().GetName().CodeBase;
				String fullAppPath = Path.GetDirectoryName(fullAppName);
				return fullAppPath;
			}
		}
				
		public static String ImagePath			//�������� ���� � ����� � ����������
		{	
			get
			{
				String fullAppName = Assembly.GetExecutingAssembly().GetName().CodeBase;
				String fullAppPath = Path.GetDirectoryName(fullAppName);
				String fullImgPath = Path.Combine (fullAppPath,"GameData");
				return fullAppPath;
			}
		}
		
		public void LoadEmbeddedImage()		//!!!��� ������������(gjrf yt nhjufnm)
		{
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream file = thisExe.GetManifestResourceStream("Bitmap1.bmp");
			Bitmap b = new Bitmap (file);
		}

		public static Bitmap LoadBitmap(String name)		//������������� ����� �������� ����������� �� �����
		{
			Bitmap b;
			try
			{
				b = new Bitmap(Path.Combine(GAME.ImagePath   ,name));
			}
			catch (Exception e)
			{
				try
				{
					b = new Bitmap ("GameData\\"+name);
				}
				catch (Exception ex)
				{
					b = new Bitmap(240,320);
				}
			}
			return b;
		}

		#endregion

		#region ���������� ����������� ������� �����
#if POCKET
		const uint SIPF_OFF = 0x0;
		const uint SIPF_ON = 0x1;
		[DllImport("coredll.dll")]
		private extern static void SipShowIM(uint dwFlag);

		public void ShowSIPF()
		{
			SipShowIM(SIPF_ON);
		}

		public void HideSIPF()
		{
			SipShowIM(SIPF_OFF);
		}
#endif
		#endregion

		#region ��������� �����
		private void InitializeComponent()
		{
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Matrix Game";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GAME_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.GAME_Closing);
			this.Load += new System.EventHandler(this.GAME_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.GAME_Paint);

		}

		private void CreateFullScreenForm()
		{
			this.WindowState = FormWindowState.Maximized;
			this.FormBorderStyle = FormBorderStyle.None;
			this.ControlBox = false;
			this.Menu = null;
			
		}
		#endregion

		#region ������������� ����� � ������ ����
		
		public static void Main() 
		{
			Application.Run(new GAME());
			
		}

		public GAME()
		{
			InitializeComponent();

		}

		private void gfx_init()
		{
			bmpOff = new Bitmap(this.ClientSize.Width ,this.ClientSize.Height );	//������� ������ ������		
			gxOff = Graphics.FromImage (bmpOff);									//������ ������ �������		
			gxOn = this.CreateGraphics ();											//������ ������� �������
		}
				


		private void GAME_Load(object sender, System.EventArgs e)
		{
			try
			{
				FileWriter.LoadOptions ();
				}
			catch(Exception ex){};

			CreateFullScreenForm();									//������ ����� �� ������ �����		
			
			//�������� �������� ������� (���� ������ ��� �� ���)
			PosPlayer = new HumanPlayer ("John",true);
			NegPlayer = new HumanPlayer ("Vasya",false);

			gfx_init ();											//�������������� �������
			CurrentScreen =new MainMenuScreen(this,gxOff);			//�������� � �������� ����
			CurrentScreen.Display ();							
						
		}
		#endregion

		#region ����������� ����������� ������� ����� 
		private void GAME_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{			
			RefreshScreen();
		}

		public void RefreshScreen()		//������ ����������� ������
		{
			gxOn.DrawImage (bmpOff,0,0);
		}

		private void GAME_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.CurrentScreen.CheckClicks (e.X ,e.Y );
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		private void GAME_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}
		#endregion

		#region ��������� �������� ��������
		public void DetermineWinner()
		{
			int det = mMatrix.getDeterminant ();
			if (det>0)
			{
				PosPlayer.IsWinner = true;
				NegPlayer.IsWinner = false;
			}
			else if (det<0)
			{
				PosPlayer.IsWinner = false;
				NegPlayer.IsWinner = true;
			}
			else	//�����
			{
				PosPlayer.IsWinner = false;
				NegPlayer.IsWinner = false;
			};
		}

		public void GameLoop()
		{
			while (!mMatrix.IsFull ())
			{
				Application.DoEvents ();
				if (HumanPlayer.IsPosTurn )
				{
					if (ClickFlag)
					{
						PosPlayer.MakeTurn ();
						HumanPlayer.IsPosTurn = false;
						ClickFlag  = false;
						CurrentScreen.Display ();
						this.RefreshScreen ();	
							
					}
					if (LoopBreak)
						break;
				}
				else
				{
					if (ClickFlag)
					{
						NegPlayer.MakeTurn ();
						HumanPlayer.IsPosTurn = true;
						if (IsMultiplayer )
							ClickFlag = false;
						CurrentScreen.Display ();
						this.RefreshScreen ();
						
					}
					if (LoopBreak)
						break;
				}
			}
			if (!LoopBreak)
			{
				LoopBreak = false;
				DetermineWinner();
				CurrentScreen = new GameResultScreen (this,gxOff,PosPlayer,NegPlayer,mMatrix);
				CurrentScreen.Display ();
			}
		}

		//��������� ������� ��������� �������
		public void RandomSides()
		{
			PlayerFactoryExtended.RandomPlayers (Pl1,Pl2);
			if (Pl1.IsPositive)
			{
				PosPlayer = Pl1 ;
				NegPlayer = Pl2 ;
			}
			else
			{
				NegPlayer = Pl1;
				PosPlayer = Pl2;
			}

		}

		//��������� ������� ����������� �������
		public void StandartAssign()
		{
			PosPlayer = Pl1;
			NegPlayer = Pl2;
		}
		#endregion
	}
}

using System;
using System.Drawing;
using System.Collections ;
using System.Windows.Forms ; 

namespace MatrixGame
{
	public abstract class AbstractPlayer
	{
		private String PlayerName;
		private bool IsPlayerPositive; 
		private bool IsPlayerWinner;
		private static bool IsPlayerMyTurn=true;

		public String Name
		{
			get {return PlayerName;}
			set {PlayerName = value;}
		}

		public bool IsPositive
		{
			get {return IsPlayerPositive; }
			set {IsPlayerPositive=value;}
		}

		public bool IsWinner
		{
			get {return IsPlayerWinner;}
			set {IsPlayerWinner=value;}
		}

		public static bool IsPosTurn
		{
			get {return IsPlayerMyTurn;}
			set {IsPlayerMyTurn = value;}
		}

		public AbstractPlayer(String name,bool IsPlPositive)
		{
			this.IsPlayerPositive = IsPlPositive;
			this.PlayerName = name;

		}

		public virtual void MakeTurn()
		{
			
		}
	}

	public class HumanPlayer:AbstractPlayer
	{

		public HumanPlayer(String name,bool IsPlPositive):base(name,IsPlPositive)
		{

		}
	}



	public class AIPlayer:AbstractPlayer
	{
		protected Matrix mMatrix;
		
		public AIPlayer(bool IsPositive,Matrix mMatrix):base(AIPlayer.AIName ,IsPositive)		
		{
			this.mMatrix = mMatrix;
		}

		protected virtual int ReturnNumber()
		{
			int	n = new Random ().Next(0,mMatrix.CountUnusedElements ());	
			ArrayList a = mMatrix.GetUnusedElements ();	
			return (int)a[n];
		}

		protected virtual Point ReturnPlace()
		{
			bool exit = false;
			int x=-1,y=-1;
			while (!exit)
			{
				x = new Random ().Next (0,3);
				y = new Random ().Next (0,3);

				if (!mMatrix.IsBlockedPlace (x,y))
				{
					exit = true;
				}
			}
			return new Point (x,y);

		}

		public override void MakeTurn()
		{
			if (!mMatrix.IsFull() )
			{				
				Point p = this.ReturnPlace ();
				int n = this.ReturnNumber ();
				mMatrix.setElement (p.X,p.Y ,n);	//WARNING!!!
				IsPosTurn = !IsPosTurn;
			}
		}
		
		//стандартное имя для компьютерного игрока
		public static String AIName
		{
			get{return "Eliza";}		
		}

	}

	public class SmartAiPlayer:AIPlayer
	{
		ArrayList Chainlist = new ArrayList (9); 
		ArrayList CellIDsList = new ArrayList ();
		CellID curcell;

		const double R_MIN=0.24;
		const double R_MAX=3.5;

		public SmartAiPlayer(bool IsPositive,Matrix mMatrix):base(IsPositive,mMatrix)
		{
			//инициализация значений цепочек
			Chainlist.Add (new Chain (0,0,1,1,2,2,0,0,2,1,1,2));
			Chainlist.Add (new Chain (1,0,2,1,0,2,1,0,2,2,0,1));
			Chainlist.Add (new Chain (2,0,1,2,0,1,2,0,1,1,0,2));

			Chainlist.Add (new Chain (0,1,2,0,1,2,0,1,1,0,2,2));
			Chainlist.Add (new Chain (1,1,2,2,0,0,1,1,2,0,0,2));
			Chainlist.Add (new Chain (2,1,0,2,1,0,2,1,1,2,0,0));

			Chainlist.Add (new Chain (0,2,1,0,2,1,0,2,1,1,2,0));
			Chainlist.Add (new Chain (1,2,0,1,2,0,1,2,0,0,2,1));
			Chainlist.Add (new Chain (2,2,0,0,1,1,2,2,0,1,1,0));
		}
		
		private void ReinitCellIDs()
		{
			CellIDsList.Clear ();
			for (int y=0;y<3;y++)
			{
				for (int x=0;x<3;x++)
				{
					if (mMatrix.getElement(x,y) ==0)
					{
						CellIDsList.Add (new CellID (x,y,Chainlist,mMatrix));
					}
				}
			}


		}
		protected override Point ReturnPlace()
		{
			this.ReinitCellIDs ();
			CellIDsList.Sort ();
			try
			{
				CellID fircell = (CellID)CellIDsList[0]; 
				int pr = fircell.Priority ;
				ArrayList WorkCellList = new ArrayList ();
					while(pr==((CellID)CellIDsList[0]).Priority )
					{
						WorkCellList.Add (CellIDsList[0]);
						CellIDsList.RemoveAt (0);
						if(CellIDsList.Count ==0)
							break;
					}
				int rndnum = new Random ().Next (0,WorkCellList.Count );
				curcell = (CellID)WorkCellList[rndnum];

				return new Point (curcell.PosX ,curcell.PosY );

			}
			catch (Exception e)
			{
				MessageBox.Show (e.Message );
				return new Point (0,0);
			}
				
		}

		protected override int ReturnNumber()
		{
			if (curcell.Type != ChainType.oo )
			{
				if (curcell.R>R_MAX)
					return GetMax();
				else if (curcell.R < R_MIN)
					return GetMin();
				else 
					return GetMid();
			}
			else
				 return base.ReturnNumber ();
			
		}

		private int GetMax()
		{
			int numofels=mMatrix.CountUnusedElements ();
			ArrayList temp = mMatrix.GetUnusedElements ();
			return (int)temp[numofels-1];

		}

		private int GetMin()
		{
			ArrayList temp = mMatrix.GetUnusedElements ();
			return (int)temp[0];
		}

		private int GetMid()
		{
			int numofels=mMatrix.CountUnusedElements ();
			ArrayList temp = mMatrix.GetUnusedElements ();
			if (numofels>=3)
			{
				return (int)temp[new Random ().Next (numofels/2-1,numofels/2+1)];
			}
			else 
				return base.ReturnNumber ();

		}

		class CellID:IComparable
		{
			ArrayList cl;
			int pos;
			int x,y;
			Matrix m;
			int e1,e2,e3,ne1,ne2,ne3;
			int pose,nege;

			public CellID(int x,int y, ArrayList cl,Matrix m)
			{
				this.cl = cl;
				pos = xy2pos(x,y);
				this.m = m;
				this.x = x;
				this.y = y;
				InitEls();
			}

			private void InitEls()
			{
				Chain chain = (Chain)cl[pos];		//рабочая цепочка
					
				//получаем цифры положит.
				e1 = m.getElementAI (chain.x1,chain.y1 );
				e2 = m.getElementAI (chain.x2,chain.y2);
				e3 = m.getElementAI (chain.x3,chain.y3);

				//получаем цифры отриц.
				ne1 = m.getElementAI (chain.nx1,chain.ny1);
				ne2 = m.getElementAI (chain.nx2,chain.ny2);
				ne3 = m.getElementAI (chain.nx3,chain.ny3);
			}

			private int xy2pos(int x,int y)
			{
				switch (x)
				{
					case 0:
					switch (y)
					{
						case 0:
							return 0;
						case 1:
							return 3;
						case 2:
							return 6;
					}
						break;
					case 1:
					switch (y)
					{
						case 0:
							return 1;
						case 1:
							return 4;
						case 2:
							return 7;
					}
						break;
					case 2:
					switch (y)
					{
						case 0:
							return 2;
						case 1:
							return 5;
						case 2:
							return 8;
					}
						break;
					
				}
				return -1;		//случай ошибки
			}
			
			public int PosX
			{
				get {return this.x; }
			}

			public int PosY
			{
				get {return this.y ;}
			}

			public double R
			{
				get
				{
					pose = e1*e2*e3;
					nege = ne1*ne2*ne3;
					
					if (AbstractPlayer.IsPosTurn)
						return (double)pose/nege;
					else 
						return (double)nege/pose;
				}
			
			}

			public ChainType Type
			{
				get 
				{
					ChainSubtype poscst,negcst;
					poscst = GetChainSubtype (e1,e2,e3);
					negcst = GetChainSubtype (ne1,ne2,ne3);
					return GetChainType(poscst,negcst);

				}
			}

			private ChainSubtype GetChainSubtype(int e1,int e2,int e3)
			{
				if ((e1==1)&&(e2==1)&(e3==1))		//opening chain
					return ChainSubtype.o;
				else if (((e1==1)&&(e2==1))||((e1==1)&&(e3==1))||((e2==1)&&(e3==1)))
					return ChainSubtype.p ;			//perspective chain
				else if ((e1==1)||(e2==1)||(e3==1))
					return ChainSubtype.c ;			//closing chain
				throw new ArgumentException ("GetChainSubtype error");
			}

			private ChainType GetChainType(ChainSubtype cs1,ChainSubtype cs2) 
			{
				if ((cs1==ChainSubtype.c)&&(cs2==ChainSubtype.c ))
					return ChainType.cc ;
				else if ((cs1==ChainSubtype.c )&&(cs2==ChainSubtype.o ))
					return ChainType.oc ;
				else if ((cs1==ChainSubtype.c)&&(cs2==ChainSubtype.p ))
					return ChainType.pc ;
				else if ((cs1==ChainSubtype.o )&&(cs2==ChainSubtype.c ))
					return ChainType.oc ;
				else if ((cs1==ChainSubtype.o )&&(cs2==ChainSubtype.o ))
					return ChainType.oo ;
				else if ((cs1==ChainSubtype.o )&&(cs2==ChainSubtype.p ))
					return ChainType.po;
				else if ((cs1==ChainSubtype.p )&&(cs2==ChainSubtype.c ))
					return ChainType.pc ;
				else if ((cs1==ChainSubtype.p )&&(cs2==ChainSubtype.o ))
					return ChainType.po ;
				else if ((cs1==ChainSubtype.p )&&(cs2==ChainSubtype.p ))
					return ChainType.pp ;

				throw new ArgumentException ("GetChainType Exception");
			}


			public int Priority
			{
				get
				{
				ChainType act = this.Type ;
				double cR = this.R ;
				
					if((act==ChainType.cc )&&((cR>R_MAX)||(cR<R_MIN)))
						return 1;
					else if((act==ChainType.oc)&&((cR>R_MAX)||(cR<R_MIN)))
						return 2;
					else if((act==ChainType.pc )&&((cR>R_MAX)||(cR<R_MIN)))
						return 3;
					else if (act==ChainType.cc)
						return 4;
					else if ((act==ChainType.oc )||(act==ChainType.pc)||(act==ChainType.po )||(act==ChainType.pp )||(act==ChainType.oo ))
						return 5;
					throw new ArgumentException ("Priority Property failed");
				}
			}

			public int CompareTo(object obj)
			{
				if (obj is CellID )
				{
					CellID o = (CellID)obj; 
					if (o.Priority>this.Priority )
						return -1;
					else if (o.Priority == this.Priority )
						return 0;
					else
						return 1;
				}
				throw new ArgumentException("object is not a CELLID");
			}
		}

		class Chain
		{
			public int x1,x2,x3,y1,y2,y3;
			public int nx1,nx2,nx3,ny1,ny2,ny3;

			public Chain(int x1,int y1,int x2,int y2,int x3,int y3,
				int nx1, int ny1, int nx2,int ny2, int nx3,int ny3)
			{
				this.x1 = x1;
				this.x2 = x2;
				this.x3 = x3;
				this.y1 = y1;
				this.y2 = y2;
				this.y3 = y3;

				this.nx1 = nx1;
				this.nx2 = nx2;
				this.nx3 = nx3;
				this.ny1 = ny1;
				this.ny2 = ny2;
				this.ny3 = ny3;
			}
		}
	}

		public enum ChainType
		{
			cc,oc,pc,po,pp,oo
		}

	public enum ChainSubtype
	{
		c,p,o
	}
		#region KillerPlayer
		public class KillerPlayer:AIPlayer
		{
			ArrayList list = new ArrayList (10);
			Matrix mMatrix;
			ArrayList l = new ArrayList ();
		
			public KillerPlayer(bool IsPositive,Matrix mMatrix):base(IsPositive,mMatrix)
			{
				InitializeList();
			}
		
			private void Iteration(ArrayList InitList,ArrayList CurList)
			{
				if (InitList.Count ==0)
				{
				
					Matrix m = new Matrix (InitList);
					l.Add (m);
                				
				}
				else
				{
					for (int c=0;c<InitList.Count ;c++)
					{
						int newElem = (int)InitList[c];
						CurList.Add (newElem);
						ArrayList newInitList = (ArrayList)InitList.Clone ();
						newInitList.RemoveAt(c);
						Iteration(newInitList,CurList);
						CurList.Remove (newElem);
					}
				} 
			}

			private void InitializeList()
			{
				for (int i=1;i<10;i++)
					list.Add (i);
			}

			protected override Point ReturnPlace()
			{
				if (mMatrix.IsEmpty() )
					return base.ReturnPlace ();
				else
				{
					int det;
					//int maxdet = l[0]
					Iteration(list,new ArrayList ());
					for (int i=0;i<l.Count ;i++)
					{
						//det = l[i].getDeterminant ();

					}
			
					return new Point (0,0);
				}
			}

			protected override int ReturnNumber()
			{
				if (mMatrix.IsEmpty() )
					return base.ReturnNumber ();
				else
				{
					return 0;	
				}
			
			}


		}
		#endregion

	
}
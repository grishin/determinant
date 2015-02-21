using System;

namespace MatrixGame
{
	public interface PlayerFactory
	{
		AbstractPlayer PosPlayer	{get;}
		AbstractPlayer NegPlayer	{get;}
	}

//Создание двух игроков-людей 
	public class MultiPlayerFactory:PlayerFactory
	{
		String P1Name,P2Name;
		
		public MultiPlayerFactory(String P1Name,String P2Name)
		{
			this.P1Name = P1Name;
			this.P2Name = P2Name;
		}
		
		public AbstractPlayer PosPlayer
		{
			get{return new HumanPlayer (P1Name,true);}
		}

		public AbstractPlayer NegPlayer
		{
			get {return new HumanPlayer (P2Name,false);}
		}
	}

//создание игрока-человека и игрока АI
	public class SinglePlayerFactory:PlayerFactory
	{
		String HumanName;
		Matrix mMatrix;
		
		public SinglePlayerFactory(String HumanName,Matrix mMatrix)
		{
			this.HumanName = HumanName;
			this.mMatrix = mMatrix;

		}
		public AbstractPlayer PosPlayer
		{
			get{return new HumanPlayer (HumanName,true);}	
		}

		public AbstractPlayer NegPlayer
		{
			get {return new AIPlayer (false,mMatrix);}
		}
	}

	public class PlayerFactoryExtended
	{
		public static AbstractPlayer CreatePosHumanPlayer(String name)
		{
			return new HumanPlayer (name,true);
		}

		public static AbstractPlayer CreateNegHumanPlayer(String name)
		{
			return new HumanPlayer (name,false);
		}

		public static AbstractPlayer CreatePosAiPlayer(Matrix mMatrix)
		{
			return new SmartAiPlayer (true,mMatrix);
		}

		public static AbstractPlayer CreateNegAiPlayer(Matrix mMatrix)
		{
			return new SmartAiPlayer (false,mMatrix);
		}

		public static void RandomPlayers(AbstractPlayer p1,AbstractPlayer p2)
		{
			int i = new Random ().Next (0,2);
			switch(i)
			{
				case 0:
					p1.IsPositive = true;
					p2.IsPositive = false;
					break;
				case 1:
					p1.IsPositive = false;
					p2.IsPositive = true;
					break;
			}
		}


    }
}

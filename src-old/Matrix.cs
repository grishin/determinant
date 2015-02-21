using System;
using System.Drawing;
using System.Collections;
namespace MatrixGame
{
	// Класс матрицы и её преобразований 
  	public class Matrix
	{
		private int[,] a=new int[3,3];				//matrix
		private ArrayList dnum = new ArrayList(9);	//deleted numbers

		public Matrix()
		{
			this.Clear ();
		}

		public Matrix(ArrayList ar)				//create matrix from an array
		{
			int c=0;
					for (int i=0;i<3;i++)
						for (int j=0;j<3;j++)
						{
							a[i,j]=(int)ar[c];
							c++;
						}
		}

		public void Clear()
		{
			for (int i=0;i<3;i++)
				for (int j=0;j<3;j++)
					a[i,j]=0;
			dnum.Clear ();
		}

		//возвращает элемент с индексом x,y
		//x = 0..2
		//y = 0..2
		public int getElement(int x,int y)
		{
			return a[x,y];
		}

		public int getElementAI(int x,int y)
		{
			if (a[x,y] != 0)
				return a[x,y];
			else 
				return 1;
		}

		/**
		 * Устанавливает значение элемента с индексом x,y
		 *	x = 0..2
		 *	y = 0..2
		 */
		public void setElement(int x,int y, int val )
		{
			if ((x>=0 && x<=2) && (y>=0 && y<=2))
				if (val>=1 && val<=9)
					a[x,y]=val;
			this.AddToDeleted (val);
		}

		public void AddToDeleted(int number)
		{
			dnum.Add (number);
		}

		public bool IsDeleted(int number)
		{
			bool temp = false;
			foreach (int i in dnum)
			{
				if (i==number)
				{
					temp = true;
					break;
				}
			}
			return temp;
		}

		public bool IsBlockedPlace(int x,int y)
		{
			return (getElement(x,y)!=0);
		}

		public bool IsFull()
		{
			return (dnum.Count ==9);
		}

		public bool IsEmpty()
		{
			return (dnum.Count ==0);
		}

		/**
		 * Производит подсчёт определиителя матрицы методом "звезды".
		 */
		public int getDeterminant()
		{
			return (a[0,0]*a[1,1]*a[2,2] +
				a[0,1]*a[1,2]*a[2,0] +
				a[1,0]*a[2,1]*a[0,2]) -
				(a[0,2]*a[1,1]*a[2,0] +
				a[0,1]*a[1,0]*a[2,2] +
				a[1,2]*a[2,1]*a[0,0]);
		}

		public void MakeItFull()
		{
			while (!this.IsFull())
			{
				dnum.Add (0);
			}
		}

		public ArrayList GetUnusedElements()
		{
			ArrayList ret = new ArrayList ();
			for (int i=1;i<10;i++)
			{
				if (!IsDeleted(i))
				{
					ret.Add (i);
				}
			}
			return ret;
		}

		public int CountUnusedElements()
		{
			return GetUnusedElements().Count ;			
		}

	}
}

using System;
using System.IO ;
using System.Collections ;


namespace MatrixGame.Screens
{
	public class FileWriter
	{
		public FileWriter()
		{

		}

		public static void SaveOptions()
		{
			FileStream fout;
			try
			{
				fout = new FileStream(Path.Combine(GAME.ImagePath ,"options.dat"),FileMode.Create );
				try
				{
					if (GAME.PositiveAlwaysStarts)
						fout.WriteByte (1);
					else
						fout.WriteByte (0);

					if (GAME.RandomSideSelection )
						fout.WriteByte (1);
					else
						fout.WriteByte (0);

					fout.Close ();
				}
				catch(IOException e){}
			}
			catch(Exception e){}
		}

		public static void SaveNamelist(Array names)
		{
			FileStream fout;
			try
			{
				fout = new FileStream(Path.Combine(GAME.ImagePath ,"namelist.dat"),FileMode.Create );
				StreamWriter fstr_out = new StreamWriter (fout);
				for (int i=0;i<names.Length  ;i++)
				{
					fstr_out.WriteLine (names.GetValue (i));
				}
				fstr_out.Close ();
			}
			catch(Exception e){}			
		}

		public static void LoadOptions()
		{
			FileStream fin;
			try
			{
				fin = new FileStream(Path.Combine(GAME.ImagePath ,"options.dat"),FileMode.Open );
				try
				{
					int r1 = fin.ReadByte ();
					int r2 = fin.ReadByte ();

					if (r1==0)
						GAME.PositiveAlwaysStarts = false;
					else
						GAME.PositiveAlwaysStarts = true;

					if (r2==0)
						GAME.RandomSideSelection = false;
					else
						GAME.RandomSideSelection = true;
					fin.Close ();
				}
				catch(IOException e){}
			}
			catch(Exception e){}
		}

		public static void LoadNamelist(Array names)
		{
			FileStream fin;
			try
			{
				fin = new FileStream(Path.Combine(GAME.ImagePath ,"namelist.dat"),FileMode.Open  );
				StreamReader fstr_in = new StreamReader (fin);
				for (int i=0;i<names.Length  ;i++)
				{
					names.SetValue (fstr_in.ReadLine (),i);
				}
				fstr_in.Close ();
			}
			catch(Exception e){}			
		}
	}
}

using System;

namespace Nine.Desktop
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new NineGame())
			{
				game.Run();
			}
		}
	}
}

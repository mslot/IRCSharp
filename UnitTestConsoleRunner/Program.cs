using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestConsoleRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			UnitTest.UnitTest test = new UnitTest.UnitTest();
			string[] my_args = { System.Reflection.Assembly.GetAssembly(test.GetType()).Location};
			int returnCode = NUnit.ConsoleRunner.Runner.Main(my_args);

			if (returnCode != 0)
				Console.Beep();

			Console.WriteLine("press some key to exit ...");
			Console.ReadKey();
		}
	}
}

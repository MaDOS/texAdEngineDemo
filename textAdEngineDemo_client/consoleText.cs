using System;

namespace textAdEngineDemo_client
{
	public class consoleText
	{
		public string text;
		public ConsoleColor foreColor = ConsoleColor.White;
		public ConsoleColor backColor = ConsoleColor.Black;
		
		public consoleText(string text_param, ConsoleColor foreColor_param, ConsoleColor backColor_param)
		{
			text = text_param;
			foreColor = foreColor_param;
			backColor = backColor_param;
		}
		
		public consoleText()
		{
			text = "";
			foreColor = ConsoleColor.White;
			backColor = ConsoleColor.Black;
		}
	}
}

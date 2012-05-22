using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace textAdEngineDemo_client
{
	public class view
	{
		private int health;
		private string area;
		private List<consoleText> textBuffer;
		
		public view()
		{
			resetBuffer();
			render();
		}
		
		private void render()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.Write("Health: ");
			if(health > 75)
			{
				Console.ForegroundColor = ConsoleColor.DarkGreen;
			}
			else if(health > 25 && health < 75)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else if(health < 25)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			Console.Write(health.ToString());
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Write("\t\t\t\t\t\tArea: ");
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.Write(area);
			Console.Write("\n\r");
			resetColors();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			
			for(int i = textBuffer.Count - 1; i >= 0; i--)
			{
				if(textBuffer[i].GetType() == typeof(consoleText))
				{
					Console.BackgroundColor = textBuffer[i].backColor;
					Console.ForegroundColor = textBuffer[i].foreColor;
					Console.WriteLine(textBuffer[i].text);
				}
				else
				{
					Console.BackgroundColor = ConsoleColor.Black;
					Console.WriteLine();
				}
			}
//			foreach(consoleText conText in textBuffer)
//			{
//				Console.BackgroundColor = conText.backColor;
//				Console.ForegroundColor = conText.foreColor;
//				Console.WriteLine(conText.text);
//			}
			resetColors();
			Console.WriteLine();
		}
		
		public void WriteText(string text, ConsoleColor foreColor, ConsoleColor backColor)
		{
			int bufferLines = ((int)Math.Ceiling((double)text.Length / (double)70));
//			List<consoleText> tempBuffer = new List<consoleText>(16);
//			resetBuffer();
//			for(int i = bufferLines; i == 1; i--)
//			{
//				if(i * 70 > text.Length)
//				{
//					tempBuffer[i - 1] = new consoleText(text.Substring((i - 1) * 70, text.Length - ((i - 1) * 70)), foreColor, backColor);
//				}
//				else					
//				{
//					tempBuffer[i - 1] = new consoleText(text.Substring((i - 1) * 70, 70), foreColor, backColor);
//				}
//			}
//			for(int i = 0; i < 15 - bufferLines; i++)
//			{
//				textBuffer[i + bufferLines] = tempBuffer[i];
//			}
			for(int i = bufferLines; i == 1; i--)
			{
				if(i * 70 > text.Length)
				{
					textBuffer.Insert(0, new consoleText(text.Substring((i - 1) * 70, text.Length - ((i - 1) * 70)), foreColor, backColor));
				}
				else					
				{
					textBuffer.Insert(0, new consoleText(text.Substring((i - 1) * 70, 70), foreColor, backColor));
				}
			}
//			textBuffer.InsertRange(0, tempBuffer);
			render();
		}
		
		public void changeHealth(string healthStr)
		{
			health = Convert.ToInt32(healthStr);
			render();
		}
		
		public void changeArea(string areaName)
		{
			area = areaName;
			render();
		}
		
		public void resetColors()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}
		
		public void resetBuffer()
		{
//			for(int i = 0; i < 16; i++)
//			{
//				textBuffer[i] = new consoleText();
//			}
			textBuffer = new List<consoleText>(16);
		}
	}
}

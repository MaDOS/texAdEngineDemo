using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace textAdEngineDemo_client
{
	static class main
	{
		private static IPAddress serverIP;
		private static int port;
		private static Socket server;
		private static byte[] data;
		private static IPEndPoint serverEndPoint;
		private static string username;
		private static string password;
		private static bool loggedIn = false;
		private static view cView;
		
		public static void Main(string[] args)
		{
//			for(int i = 1; i < 40; i++)
//			{
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("*");
//				Console.Write("#");
//			}
//			Console.WriteLine(((int)Math.Ceiling((double)"".Length / (double)70)).ToString());
//			Console.ReadKey();
			connect();
		}
		
		public static void connect()
		{
			data = new byte[1024];  //to prevent from crashing use the same buffersize like the server
			Console.Write("Server IP: ");
			serverIP = IPAddress.Parse(Console.ReadLine());
			Console.Write("\n\r");
			
			Console.Write("Server port: ");
			port = Convert.ToInt32(Console.ReadLine());
			Console.Write("\n\r");
			
			Console.Write("Username: ");
			username = Console.ReadLine();
			Console.Write("\n\r");
			
			Console.Write("Password: ");
			ConsoleKeyInfo pressed = Console.ReadKey(true);
			while(pressed.Key != ConsoleKey.Enter)
			{
				Console.Write("*");
				if(pressed.Modifiers != ConsoleModifiers.Shift)
				{
					password += pressed.Key.ToString().ToLower();
				}
				pressed = Console.ReadKey(true);
			}
			
			Console.Write("\n\r");
			
			cView = new view();
			
			serverEndPoint = new IPEndPoint(serverIP, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            data = new byte[1024];
            server.Connect(serverEndPoint);
            
            
            string login = username + ":" + password;
           
            server.Send(login.ToByteArray());
            
           	Thread listener = new Thread(Listen);
	        listener.Priority = ThreadPriority.Lowest;
	        listener.Start();
	        
	        Thread.Sleep(1000);
	        
	        while(server.Connected)
	        {
	        	Thread.Sleep(20);
	        	string command = Console.ReadLine();
	        	if(server.Connected)
	        	{
	        		server.Send(command.ToByteArray());
	        	}
	        }
	        
            server.Close();
	        listener.Abort();
            connect();
		}
		
		private static void Listen()
        {
            var recv = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, recv);
            parseReceived(stringData);

            while (server.Connected)
            {
            	try
            	{
	                data = new byte[1024];
	                recv = server.Receive(data);
	                stringData = Encoding.ASCII.GetString(data, 0, recv);
	                parseReceived(stringData);
            	}
            	catch(Exception ex)
            	{
            		Console.WriteLine(ex.Message);
            		if(!server.Connected)
            		{
            			Console.WriteLine("Dropping server...Press ENTER for new session...");
            		}
            	}
            }
        }
		
		private static void parseReceived(string received)
		{
			if(received.StartsWith("#"))
			{
				string[] dataPair = received.Split(':');
				if(dataPair[0] == "#login")
				{
					if(dataPair[1] == "OK")
					{
						loggedIn = true;
						Console.WriteLine("Logged in sucessfully");
					}
				}	
				else if(dataPair[0] == "#health")
				{
					cView.changeHealth(dataPair[1]);
				}
				else if(dataPair[0] == "#area")
				{
					cView.changeArea(dataPair[1]);
					cView.WriteText("** Your are now in area: " + dataPair[1], ConsoleColor.Black, ConsoleColor.Green);
				}
				else if(dataPair[0] == "#areaDesc")
				{
					cView.WriteText(dataPair[1], ConsoleColor.Black, ConsoleColor.White);
				}
				else if(dataPair[0] == "#chat")
				{
					cView.WriteText("'' " + dataPair[1] +  " said: " + dataPair[1], ConsoleColor.Black, ConsoleColor.Red);
				}
			}
			else if(received.Trim() != "")
			{
				cView.WriteText("server said: " + received, ConsoleColor.Blue, ConsoleColor.White);
			}
			
		}
		
		private static void Disconnect()
        {
            if (server.Connected)
                server.Disconnect(true);

            if (!server.Connected)
                Console.WriteLine("Connection closed!");
        }
		
		public static byte[] ToByteArray(this string s)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetBytes(s);
        }
	}
}
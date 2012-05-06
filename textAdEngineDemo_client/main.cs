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
		
		public static void Main(string[] args)
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
			password = Console.ReadLine();
			
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
	        
	        while(loggedIn)
	        {
	        	string command = Console.ReadLine();
	        	
	        	server.Send(command.ToByteArray());
	        	
	        	Thread.Sleep(10);
	        	
	        	Console.Write("WASHERE");
	        }
	        
	        Console.Write("was here");
		}
		
		private static void Listen()
        {
            var recv = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, recv);
            parseReceived(stringData);

            while (server.Connected)
            {
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                parseReceived(stringData);
            }
            Console.WriteLine("Disconnecting from server...");
            server.Close();
        }
		
		private static void parseReceived(string received)
		{
			Console.Write("WASHERE!!!");
			if(received.ToCharArray()[0] == '#')
			{
				string[] dataPair = received.Split(':');
				if(dataPair[0] == "#login")
				{
					if(dataPair[1] == "OK")
						loggedIn = true;
				}
			}
			else
			{
				Console.WriteLine(received);
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
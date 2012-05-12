using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ci_texAdEngine1;

namespace textAdEngineDemo_server
{
	public static class texAdGame
	{
		public static game gme;
		private static Dictionary<string, client> connectedClients;
		private static IPAddress localhost = IPAddress.Loopback;
		private static byte[] data;
		private static int received;
	    private static NetworkStream networkStream;
	    private static TcpClient tcpClient;
	    private static TcpListener tcpListener;
		
		public static void Main(string[] args)
		{
			if(args.Length == 1)
			{
				if(args[0] == "")
				{
					gme = new game("test");
				}
				else
				{
					gme = new game(args[0]);
				}
			}
			else
			{
				gme = new game("test");
			}
			
			data = new byte[1024];
			
			if(args.Length == 2)
			{
				if(args[1] == "")
				{
		        	tcpListener = new TcpListener(localhost, 4); //according to iana port 4 is unassigned
				}
				else
				{
					tcpListener = new TcpListener(localhost, Convert.ToInt32(args[1]));
				}
			}
			else
			{
				tcpListener = new TcpListener(localhost, 4);
			}
			
			connectedClients = new Dictionary<string, client>();
			
	        tcpListener.Start();
	        Console.WriteLine("Server started!");
	
	        while(true)
	        {
	        	bool loggedIn = false;
	        	tcpClient = tcpListener.AcceptTcpClient();
		        networkStream = tcpClient.GetStream();
		        
		        Console.WriteLine("Client connected");
		        
		        data = "WELC".ToByteArray();
		        networkStream.Write(data, 0, data.Length);
		        
		        Thread.Sleep(20);
		        
				while (!loggedIn)
		        {
		            data = new byte[1024];
		            received = networkStream.Read(data, 0, data.Length);
		            if (received == 0)
		                break;    
		            string[] login = Encoding.ASCII.GetString(data, 0, received).Split(':');
		            string username = login[0];
		            string password = login[1];
		            loggedIn = true;
		            if(connectedClients.ContainsKey(username))
		            {
		            	if(!(connectedClients[username].tcpClient.Connected))
		            	{
		            		connectedClients[username] = new client(tcpClient, username, password, ref gme);
		            	}
		            }
		            else
		            {
		            	connectedClients.Add(username, new client(tcpClient, username, password, ref gme));
		            }

				}
	        }
			tcpListener.Stop();
		}
		
		public static byte[] ToByteArray(this string s)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetBytes(s);
        }
	}
}
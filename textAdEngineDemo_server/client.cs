using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace textAdEngineDemo_server
{
	public class client
	{
		public bool loggedIn = false;
		public string username = "";
		public string password = "";
		private NetworkStream clientStream;
		public TcpClient tcpClient;
		private byte[] data;
		private Thread listener;
		
		public client(TcpClient conClient, string username_param, string password_param)
		{ 
			tcpClient = conClient;
			clientStream = tcpClient.GetStream();
			
			username = username_param;
			password = password_param;
			
			data = "#login:OK".ToByteArray();
		    clientStream.Write(data, 0, data.Length);
				
			listener = new Thread(new ThreadStart(listen));
			listener.Priority = ThreadPriority.BelowNormal;
			listener.Start();
		}
		
		private void listen()
		{
			int received = 0;
	
	        while (true)
	        {
	            data = new byte[1024];
	            received = clientStream.Read(data, 0, data.Length);
	            if (received == 0)
	                break;
	
	            var answer = parse(Encoding.ASCII.GetString(data, 0, received)).ToByteArray();
	            clientStream.Write(answer, 0, answer.Length);
	        }
	       	clientStream.Close();
	        tcpClient.Close();
		}
		
		private string parse(string command)
		{
			Console.WriteLine("Client " + username + " sent: \"" + command);
			return "OK";	
		}
	}
}

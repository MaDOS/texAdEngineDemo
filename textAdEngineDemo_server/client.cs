using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using ci_texAdEngine1;

namespace textAdEngineDemo_server
{
	public class client
	{
		public bool loggedIn = false;
		public string username = "";
		public string password = "";
		public player clientPlayer;
		private NetworkStream clientStream;
		public TcpClient tcpClient;
		private byte[] data;
		private Thread listener;
		
		public client(TcpClient conClient, string username_param, string password_param, ref game GAME)
		{ 
			tcpClient = conClient;
			clientStream = tcpClient.GetStream();
			
			username = username_param;
			password = password_param;
			
			clientPlayer = new player(username, password, game.areas);
			
			if(clientPlayer.loginOk)
			{
				Console.WriteLine(username + "'s login was ok");
				send("#login:OK");
			    
			    GAME.onConnect(this.clientPlayer);
			    send("#health:" + clientPlayer.health.ToString());
			    send("#area:" + clientPlayer.position.name);
			    
				listener = new Thread(new ThreadStart(listen));
				listener.Priority = ThreadPriority.BelowNormal;
				listener.Start();
			}
			else
			{
				Console.WriteLine(username + "'s login was false");
				send("#login:FALSE");
			    tcpClient.Close();
			}	
		}
		
		private void listen()
		{
			int received = 0;
	
	        while(tcpClient.Connected)
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
			string[] statement = command.Split(' ');
			string imperativ = statement[0];
			if(imperativ == "go")
			{
				clientPlayer.go(statement[1]);
				send("#area:" + clientPlayer.position.name);
			}
			Console.WriteLine("Client " + username + " sent: \"" + command);
			return "OK";	
		}
		
		public void send(string dataStr)
		{
			data = dataStr.ToByteArray();
			clientStream.Write(data, 0, data.Length);
		}
	}
}

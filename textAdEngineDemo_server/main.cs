using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ci_texAdEngine1;

namespace textAdEngineDemo_server
{
	static class texAdGame
	{
		private static game gme;
		private static IPAddress localhost = IPAddress.Loopback;
		private static byte[] data;
	    private static NetworkStream networkStream;
	    private static int received;
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
			
	        tcpListener.Start();
	        Console.WriteLine("Server started!");
	        Console.WriteLine("Waiting for a client...");
	
	        tcpClient = tcpListener.AcceptTcpClient();
	        networkStream = tcpClient.GetStream();
	        
	        data = "WELC".ToByteArray();
	        networkStream.Write(data, 0, data.Length);
	
	        Thread listener = new Thread(Listen);
	        listener.Priority = ThreadPriority.Lowest;
	        listener.Start();
		}
		
		static void Listen()
	    {
	        var arrOK = "OK".ToByteArray();
	
	        while (true)
	        {
	            data = new byte[1024];
	            received = networkStream.Read(data, 0, data.Length);
	            if (received == 0)
	                break;
	
	            Console.WriteLine("Received from TCP Client: " + Encoding.ASCII.GetString(data, 0, received));
	            networkStream.Write(arrOK, 0, arrOK.Length);
	        }
	        networkStream.Close();
	        tcpClient.Close();
	        tcpListener.Stop();
	    }
		
		public static byte[] ToByteArray(this string s)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetBytes(s);
        }
	}
}
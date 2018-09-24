using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Connect("192.168.0.10", 9000);
		    Console.WriteLine("Connected...");
            NetworkStream ioClient = new NetworkStream(ClientSocket);

		    string fileToRecieve = "C:/Users/olive/Documents/Small.jpg";

            LIB.writeTextTCP(ioClient,fileToRecieve);
            /*
            byte[] msgBuff = Encoding.Default.GetBytes(fileToRecieve);
		    ioClient.Write(msgBuff, 0, msgBuff.Length);
            */

		    long fileSize = LIB.getFileSizeTCP(ioClient);

		    Console.WriteLine($"FileSize: {fileSize.ToString()}");

            //List<byte> byteList = new List<byte>();

		    byte[] readBuf = new byte[fileSize];

		    //ioClient.Read(readBuf, 0, (int)fileSize);

            /*
		    String line = "";
		    char ch;
            */

		    int i = 0;

		    while (fileSize != 0)
		    {
		        readBuf[i] = (byte)ioClient.ReadByte();
		        ++i;
		        --fileSize;
		    }

		    File.WriteAllBytes("C:/Users/olive/Documents/downloadedImage.jpg", readBuf);

		    Console.WriteLine("File downloaded.");


            //Console.WriteLine($"FileText: {line}");

            //receiveFile(fileToRecieve, ioClient);



        }

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
		    
            /*
		    byte[] buf = new byte[255];
		    int rec = io.Read(buf, 0, buf.Length);

            Array.Resize(ref buf, rec);
            */
		    string str = LIB.readTextTCP(io);


            Console.WriteLine($"Received: {str}");

		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		    System.Console.ReadKey();
		}
	}
}

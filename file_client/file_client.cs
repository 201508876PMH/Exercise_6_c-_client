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
            ClientSocket.Connect("10.192.155.179", 9000);
		    Console.WriteLine("Connected...");
            NetworkStream ioClient = new NetworkStream(ClientSocket);

		    while (true)
		    {
		        Console.WriteLine("Select the file you want to download from the server.");
		        Console.Write("C:/");

		        string fileToRecieve = "C:/" + Console.ReadLine();

		        Console.WriteLine("Choose name, type and where to save the file.");
		        Console.Write("C:/");

		        string saveFilePath = "C:/" + Console.ReadLine();

		        receiveFile(fileToRecieve, ioClient, saveFilePath);
            }
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
		private void receiveFile (String filePath, NetworkStream io, string saveFilePath)
		{
		    LIB.writeTextTCP(io, filePath);

		    string error = LIB.readTextTCP(io);

		    if (error == "Error: Could not find file.")
		    {
		        Console.WriteLine("Could not find file on server, please try again.\n");
		    }
		    else
		    {
		        long fileSize = long.Parse(error);

		        Console.WriteLine($"FileSize: {fileSize.ToString()}\nDownloading...");

		        byte[] readBuf = new byte[fileSize];

		        int i = 0;
		        while (fileSize != 0)
		        {
		            readBuf[i] = (byte) io.ReadByte();
		            ++i;
		            --fileSize;
		        }

		        File.WriteAllBytes(saveFilePath, readBuf);

		        Console.WriteLine("File downloaded.\n");
		    }
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

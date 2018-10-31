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
        const int PORT = 9000;

		private file_client (string[] args)
		{
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			ClientSocket.Connect(args[0], PORT);
		    Console.WriteLine("Connected...");
            NetworkStream ioClient = new NetworkStream(ClientSocket);

		    Console.WriteLine("Choose where to save the file, name and format.");
		        
            string saveFilePath = Console.ReadLine();
            
			receiveFile(args[1], ioClient, saveFilePath);
        }

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

		        byte[] readBuf = new byte[1000];

                FileStream fs = File.Create(saveFilePath);

		        int offset = 0, count = 1000;            
		        int lastRead = 1;

		        while (lastRead >= 0)
		        {
		            int i = 0;
		            while (i < count)
		            {
		                readBuf[i] = (byte)io.ReadByte();
		                ++i;
		            }

		            fs.Write(readBuf, offset, count);

		            fileSize -= 1000;

		            if (fileSize < 1000)
		            {
		                count = (int)fileSize;
		                --lastRead;
		            }               
		        }

                fs.Close();

		        Console.WriteLine("File downloaded.\n");
		    }
		}

		public static void Main (string[] args)
		{
			file_client fc =  new file_client(args);
		}
	}
}

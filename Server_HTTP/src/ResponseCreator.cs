using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace NetworkTech
{

    partial class ResponseCreator
    {
        byte[] array;
        StringBuilder b;

        string httpVersion;  
        int responseCode;  
        string responseString; 
        string contentType;  
        string date;  
        string connection;  

        public void Update(string connection, Verb.methods metod ,string httpVersion, string[] preferedContentType, string filename)
        {
            this.connection = connection;
            this.httpVersion = httpVersion;
            this.date = DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’"); // TODO: Datatime Server StartUP.
            contentType =ContentType.HTTP_MCCT.FirstOrDefault((str) => str.Contains(preferedContentType[0])); // TODO: always response ok. remove it.
            this.responseCode = 200;
            this.responseString = HttpStatusCode.StatusCode[responseCode];

            if (contentType == null)
                contentType = "application/binary";

            this.InsertFile(filename);
            this.b = new StringBuilder();
        }

        public ResponseCreator()
        {
           // Console.WriteLine("Start Folder [" +server_resources + "]");
        }
      
        private void InsertFile(string filename)
        {
            System.IO.FileInfo f1;
            if (filename == "/")
            {
                f1 =new System.IO.FileInfo(server_resources + @"/index.html");
             
            }
            else
            {
                f1 = new System.IO.FileInfo(server_resources + filename);
            }

              Console.WriteLine("File: ["+ f1?.FullName + "]");
            if (f1.Exists)
            {
                
                array = new byte[f1.Length];
                int x = f1.OpenRead().Read(array, 0, (int)f1.Length);
            }
            else
            {
                this.responseCode = 404;
                this.responseString = HttpStatusCode.StatusCode[responseCode];
                this.array = System.Text.ASCIIEncoding.UTF8.GetBytes(html_404_code);
            }

        }

        // first poskładaj w kolejnosc.
        public byte[] GetRequest()
        {

            b.Clear();
            b.Append(this.httpVersion);
            b.Append(" " + HttpStatusCode.StatusCode[responseCode]);
            b.Append("\r\nConnection: " + connection);
            b.Append("\r\nContent-Length: " + this.array.Length);
            b.Append("\r\nContent-Type: " + this.contentType);
            b.Append("\r\nDate: ").Append(this.date);
            b.Append("\r\nServer: ").Append("Servers_made_by_Adam\r\n\r\n");
            // now array contain file data.

            byte[] builder = System.Text.ASCIIEncoding.UTF8.GetBytes(b.ToString());

            byte[] re = new byte[builder.Length + this.array.Length];

            Array.Copy(builder, 0, re, 0, builder.Length);
            Array.Copy(this.array, 0, re, builder.Length, this.array.Length);

            return re;
        }
        private static string server_resources = System.Environment.CurrentDirectory+@"/www";  //"C:\Users\Adam\Desktop\pendriveSwitch\C#\Project_ServerHttpMultiClient\EntryPoint\Server_HTTP\www\";
        private static volatile string html_404_code = @"<html><head><title>404 Not Found</title></head><body bgcolor='white'><center><h1>404 Not Found</h1></center><hr><center>Servers_made_by_Adam</center></body></html>";
    }

}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkTech
{
    public partial class HttpServer
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        Socket listener;
        private readonly int port = 8081;

        public System.Collections.Generic.List<StateObject> clients;
        public HttpServer()
        {
            System.Console.WriteLine($"IP_ADDR [{Dns.GetHostEntry("localhost").AddressList[^1]}] ---- PORT [{port}]");
            this.clients = new System.Collections.Generic.List<StateObject>();
        }

        public void StartListening()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), null);
                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            // Console.WriteLine($"Client connectet count: [{ connectedWith() }]");
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = this.listener.EndAccept(ar); ;
            state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        private int connectedWith()
        {
            return this.clients.Count;
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
   
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = state.workSocket.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                bool isAllData = content.EndsWith("\r\n\r\n");

                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                if (isAllData)
                {
                    // All the data has been read from the   
                    // client. Display it on the console. 
                    byte[] response = state.handler.Handle(content);
                    string response_str = System.Text.ASCIIEncoding.UTF8.GetString(response);

                    // Echo the data back to the client.  
                    Send(state, response);
                }
                else
                {
                    // Not all data received. Get more.  
                    state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(StateObject handler, byte[] data)
        {

            handler.workSocket.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            StateObject handler = (StateObject)ar.AsyncState;
            try
            {
                // Complete sending the data to the remote device.  
                int bytesSent = handler.workSocket.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            handler.workSocket.Shutdown(SocketShutdown.Both);
            handler.workSocket.Close();
            handler = null;
        }


    }
}
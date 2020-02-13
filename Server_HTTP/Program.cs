using System;

namespace program
{
    class main
    {
        public static int Main(String[] args)
        {
          
            NetworkTech.HttpServer listener = new NetworkTech.HttpServer();
            listener.StartListening();
  			
            return 0;
        }
    }
}
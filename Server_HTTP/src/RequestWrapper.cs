using System;
using System.Linq;
namespace NetworkTech
{
    public partial class RequestHandler // złapie, obsłurzy, zwroci byte[] do wysłania.
    {
       
        private class RequestWrapper
        {
            public string[] AcceptTypes;
            public string Host;
            public string Connection;
            public Verb.methods Verb;
            public string HttpVersion;

            public string File;
            private string request;
            public RequestWrapper()
            {

            }
            public void Update(string request)
            {
                this.request = request;
                string[] Lines = this.request.Split("\r\n");
                string[] header = Lines[0].Split(' ');
                this.Verb=NetworkTech.Verb.parse(header[0]);
                this.File = header[1];
                this.HttpVersion = header[2];
                //----------------------------------------------------
                this.Host = Lines[1].Split(' ')[1];
                this.Connection = Lines[2].Split(' ')[1];
                this.AcceptTypes = Lines.First((s) => s.StartsWith("Accept:")).Split(' ')[1].Split(',');

            }

           
        }

    }
}

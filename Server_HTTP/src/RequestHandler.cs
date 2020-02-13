using System.Collections.Generic;
using System.Web;
namespace NetworkTech
{
     public partial class RequestHandler // złapie, obsłurzy, zwroci byte[] do wysłania.
    {

        ResponseCreator response;
        RequestWrapper wrapper;
        public RequestHandler()
        {
            this.response = new ResponseCreator();
            this.wrapper = new RequestWrapper();
        }


        public byte[] Handle(string request)
        {
            this.wrapper.Update(request);
            this.response.Update(this.wrapper.Connection,this.wrapper.Verb, this.wrapper.HttpVersion, this.wrapper.AcceptTypes, this.wrapper.File);
            return  this.response.GetRequest();
        }

       

    }
}

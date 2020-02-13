using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkTech
{
    class Verb
    {
        public enum methods
        {
            GET = 0,
            POST,
            HEAD,
            PUT,
            DELETE,
            CONNECT,
            OPTIONS,
            TRACE,
            PATH


        }
        public static methods parse(string Verb)
        {
            if (Verb == "GET")
                return methods.GET;

            if (Verb == "POST")
                return methods.POST;

            if (Verb == "PUT")
                return methods.PUT;

            if (Verb == "HEAD")
                return methods.HEAD;

            if (Verb == "TRACE")
                return methods.TRACE;

            if (Verb == "DELETE")
                return methods.DELETE;

            if (Verb == "CONNECT")
                return methods.CONNECT;
                
            if (Verb == "OPTIONS")
                return methods.OPTIONS;

            throw new NotImplementedException("NetworkTech.Verb.parse()");

        }

    }



}
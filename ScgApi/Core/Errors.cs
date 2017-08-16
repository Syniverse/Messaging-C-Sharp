using System;
using System.Net;

namespace ScgApi
{
    public class ScgEcxeption : Exception
    {
        public ScgEcxeption(String cause)
            : base(cause)
        {

        }
    }

    public class ExceptionRequestFailed : ScgEcxeption
    {
        public HttpStatusCode StatusCode { get; private set; }
        public String ReasonPhrase { get; private set; }
        public ExceptionRequestFailed(String cause, HttpStatusCode code, String status)
            : base(cause)
        {
            StatusCode = code;
            ReasonPhrase = status;
        }
    }
}

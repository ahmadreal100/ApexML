using System;

namespace Apex.Core
{
    public class ApexException : Exception
    {
        public ApexException(string msg) : base(msg)
        {

        }
    }

    public class DocumentPermanentException : Exception
    {
        public string Code { get; set; }
        public DocumentPermanentException(string msg, string code = null) : base(msg)
        {
            Code = code;
        }
    }
}

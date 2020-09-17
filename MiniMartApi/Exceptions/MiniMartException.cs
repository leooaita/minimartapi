using System;

namespace MiniMartApi.Exceptions
{
    public class MiniMartException:Exception
    {
        public String MinimartMessage { get; set; }
        public MiniMartException() : base()
        {
        }
        public MiniMartException(string message) : base(message)
        {
            this.MinimartMessage = message;
        }
    }
}
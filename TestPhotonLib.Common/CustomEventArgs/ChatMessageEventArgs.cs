using System;

namespace TestPhotonLib.Common
{
    public class ChatMessageEventArgs:EventArgs
    {
        public string Message { get; private set; }

        public ChatMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}

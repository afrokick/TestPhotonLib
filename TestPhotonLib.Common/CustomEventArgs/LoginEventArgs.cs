using System;

namespace TestPhotonLib.Common
{
    public class LoginEventArgs:EventArgs
    {
        public ErrorCode Error { get; private set; }

        public LoginEventArgs(ErrorCode error)
        {
            Error = error;
        }
    }
}

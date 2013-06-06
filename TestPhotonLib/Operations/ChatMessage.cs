using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using TestPhotonLib.Common;

namespace TestPhotonLib.Operations
{
    public class ChatMessage:BaseOperation
    {
        public ChatMessage(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

         [DataMember(Code = (byte)ParameterCode.ChatMessage)]
        public string Message { get; set; }
    }
}

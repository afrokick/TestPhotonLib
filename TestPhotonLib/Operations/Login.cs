using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using TestPhotonLib.Common;

namespace TestPhotonLib.Operations
{
    public class Login:BaseOperation
    {
        public Login(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.CharacterName)]
        public string CharacterName { get; set; }
    }
}

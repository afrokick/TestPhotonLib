using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using TestPhotonLib.Common;
using TestPhotonLib.Operations;

namespace TestPhotonLib
{
    public class UnityClient:PeerBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public string CharacterName { get; private set; }

        public UnityClient(IRpcProtocol protocol, IPhotonPeer unmanagedPeer) : base(protocol, unmanagedPeer)
        {
            Log.Debug("Client ip:" + unmanagedPeer.GetRemoteIP());
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.Login:
                    var loginRequest = new Login(Protocol, operationRequest);

                    if (!loginRequest.IsValid)
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    CharacterName = loginRequest.CharacterName;

                    if (World.Instance.IsContain(CharacterName))
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                        return;
                    }

                    World.Instance.AddClient(this);

                    var response = new OperationResponse(operationRequest.OperationCode);
                    SendOperationResponse(response, sendParameters);

                    Log.Info("user with name:" + CharacterName);
                    break;
                case 2:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        Log.Debug("recv:" + operationRequest.Parameters[1]);
                        EventData eventdata = new EventData(1);
                        eventdata.Parameters = new Dictionary<byte, object> { { 1, "response for event" } };
                        SendEvent(eventdata, sendParameters);
                    }
                    break;
                default:
                    Log.Debug("Unknown OperationRequest received!:" + operationRequest.OperationCode);
                    break;
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            World.Instance.RemoveClient(this);
            Log.Debug("Disconnected!");
        }
    }
}

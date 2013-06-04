using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace TestPhotonLib
{
    public class UnityClient:PeerBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public UnityClient(IRpcProtocol protocol, IPhotonPeer unmanagedPeer) : base(protocol, unmanagedPeer)
        {
            Log.Debug("Client ip:" + unmanagedPeer.GetRemoteIP());
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case 1:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        Log.Debug("recv:" + operationRequest.Parameters[1]);
                        OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                        response.Parameters = new Dictionary<byte, object> { { 1, "response message" } };
                        SendOperationResponse(response, sendParameters);
                    }
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
            Log.Debug("Disconnected!");
        }
    }
}

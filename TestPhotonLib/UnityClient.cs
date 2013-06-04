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
            Log.Debug("Unknown operation!");
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Log.Debug("Disconnected!");
        }
    }
}

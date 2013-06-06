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
                case (byte)OperationCode.SendChatMessage:
                    {
                        var chatRequest = new ChatMessage(Protocol, operationRequest);

                        if (!chatRequest.IsValid)
                        {
                            SendOperationResponse(chatRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                            return;
                        }

                        string message = chatRequest.Message;

                        string[] param = message.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (param.Length == 2)
                        {
                            string targetName = param[0];
                            message = param[1];
                            if (World.Instance.IsContain(targetName))
                            {
                                var targetClient = World.Instance.TryGetByName(targetName);
                                if (targetClient == null)
                                    return;

                                message = CharacterName + "[PM]:" + message;

                                var personalEventData = new EventData((byte)EventCode.ChatMessage);
                                personalEventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                                personalEventData.SendTo(new UnityClient[] { this, targetClient }, sendParameters);
                            }
                            return;
                        }

                        message = CharacterName + ": " + message;

                        Chat.Instance.AddMessage(message);

                        var eventData = new EventData((byte)EventCode.ChatMessage);
                        eventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                        eventData.SendTo(World.Instance.Clients, sendParameters);
                    }
                   
                    break;

                case (byte)OperationCode.GetRecentChatMessages:
                    {
                        var messages = Chat.Instance.GetRecentMessages();
                        messages.Reverse();
                        var message = messages.Aggregate((i, j) => i + "\r\n" + j);
                        var eventData = new EventData((byte)EventCode.ChatMessage);
                        eventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                        eventData.SendTo(new UnityClient[] {this}, sendParameters);
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

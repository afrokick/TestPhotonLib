using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TestPhotonLib.Common;
using UnityEngine;

public class PhotonServer : MonoBehaviour, IPhotonPeerListener
{
    private const string CONNECTION_STRING = "localhost:5055";
    private const string APP_NAME = "MyCoolServer";

    private static PhotonServer _instance;
    public static PhotonServer Instance
    {
        get { return _instance; }
    }

    private PhotonPeer PhotonPeer { get; set; }

    public event EventHandler<LoginEventArgs> OnLoginResponse;

    void Awake()
    {
        if (Instance != null)
            DestroyObject(gameObject);

        DontDestroyOnLoad(gameObject);

        Application.runInBackground = true;

        _instance = this;
    }
	// Use this for initialization
	void Start ()
	{
	    PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
        Connect();
	}
	
	// Update is called once per frame
	void Update () {
	    if(PhotonPeer != null)
            PhotonPeer.Service();
	}

    void OnApplicationQuit()
    {
        Disconnect();
    }

    private void Connect()
    {
        if (PhotonPeer != null)
            PhotonPeer.Connect(CONNECTION_STRING, APP_NAME);
    }

    private void Disconnect()
    {
        if(PhotonPeer != null)
            PhotonPeer.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn level:" + level.ToString());
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case (byte)OperationCode.Login:
                LoginHandler(operationResponse);
                break;
            default:
                Debug.Log("Unknown OperationResponse:" + operationResponse.OperationCode);
                break;
        }
    }
    
    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                if (eventData.Parameters.ContainsKey(1))
                {
                    Debug.Log("recv event:" + eventData.Parameters[1]);
                }
                break;
            default:
                Debug.Log("Unknown Event:" + eventData.Code);
                break;
        }
    }
    
    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("Connected to server!");
                break;
            case StatusCode.Disconnect:
                Debug.Log("Disconnected from server!");
                break;
            case StatusCode.TimeoutDisconnect:
                Debug.Log("TimeoutDisconnected from server!");
                break;
            case StatusCode.DisconnectByServer:
                Debug.Log("DisconnectedByServer from server!");
                break;
            case StatusCode.DisconnectByServerUserLimit:
                Debug.Log("DisconnectedByLimit from server!");
                break;
            case StatusCode.DisconnectByServerLogic:
                Debug.Log("DisconnectedByLogic from server!");
                break;
            case StatusCode.EncryptionEstablished:
                break;
            case StatusCode.EncryptionFailedToEstablish:
                break;
            default:
                Debug.Log("Unknown status:" + statusCode.ToString());
                break;
        }
    }

    #region handlers for response

    private void LoginHandler(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode != 0)
        {
            ErrorCode errorCode = (ErrorCode) operationResponse.ReturnCode;
            switch (errorCode)
            {
                    case ErrorCode.NameIsExist:
                    if (OnLoginResponse != null)
                        OnLoginResponse(this, new LoginEventArgs(ErrorCode.NameIsExist));
                    break;
                default:
                    Debug.Log("Error Login returnCode:" + operationResponse.ReturnCode);
                    break;
            }
            
            return;
        }

        if (OnLoginResponse != null)
            OnLoginResponse(this, new LoginEventArgs(ErrorCode.Ok));
    }

    #endregion
    
    
    #region Up-level API

    public void SendLoginOperation(string name)
    {
        PhotonPeer.OpCustom((byte) OperationCode.Login,
                            new Dictionary<byte, object> {{(byte) ParameterCode.CharacterName, name}}, true);
    }

    #endregion
}

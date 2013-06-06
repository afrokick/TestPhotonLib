using System;
using TestPhotonLib.Common;
using UnityEngine;
using System.Collections;

public class Chat : MonoBehaviour
{
    private string message = "";
    private string chatLog = "";

	// Use this for initialization
	void Start () {
	    PhotonServer.Instance.OnReceiveChatMessage += OnOnReceiveChatMessage;
        PhotonServer.Instance.GetRecentChatMessage();
	}

    void OnDestroy()
    {
        PhotonServer.Instance.OnReceiveChatMessage -= OnOnReceiveChatMessage;
    }

    private void OnOnReceiveChatMessage(object sender, ChatMessageEventArgs e)
    {
        chatLog = e.Message + "\r\n" + chatLog;
    }

	void OnGUI()
	{
	    GUI.Label(new Rect(5,5,300,300), chatLog);

	    message = GUI.TextField(new Rect(5, 320, 200, 20), message);

        if (GUI.Button(new Rect(210, 320, 80, 20), "Send"))
        {
            if (message.Length == 0)
                return;
            PhotonServer.Instance.SendChatMessage(message);
            message = "";
        }
	}
}

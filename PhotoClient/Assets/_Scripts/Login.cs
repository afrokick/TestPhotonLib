using System;
using TestPhotonLib.Common;
using UnityEngine;

public class Login : MonoBehaviour {

    private string Error { get; set; }
    private string CharacterName { get; set; }


	// Use this for initialization
	void Start ()
	{
	    CharacterName = "";
	    PhotonServer.Instance.OnLoginResponse += OnLoginHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        CharacterName = GUI.TextField(new Rect(50, 50, 200, 20), CharacterName);

        if (GUI.Button(new Rect(50, 80, 100, 20), "Login"))
        {
            Error = "";
            PhotonServer.Instance.SendLoginOperation(CharacterName);
        }

        GUI.Label(new Rect(50, 5, 300, 20), Error);
    }

    private void OnLoginHandler(object o, LoginEventArgs e)
    {
        if (e.Error != ErrorCode.Ok)
        {
            Error = "Error:" + e.Error.ToString();
            return;
        }

        PhotonServer.Instance.OnLoginResponse -= OnLoginHandler;

        Application.LoadLevel("Game");
    }
}

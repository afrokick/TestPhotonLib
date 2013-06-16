using System.Linq;
using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    private Player localPlayer { get; set; }
    private float Step = 3f;

    private Vector3 oldPosition { get; set; }
    private const float SendRate = 0.05f;
    private float lastSendTime = 0;
	// Use this for initialization
	void Start () {
	    PhotonServer.Instance.WorldEnterOperation();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    InputUpdate();

       try
       {
           MoveLogic();
       }catch{}

	    
	}

    void FixedUpdate()
    {
        if (localPlayer == null)
        {
            var p = PhotonServer.Instance.Players.FirstOrDefault(
                n => n.CharacterName.Equals(PhotonServer.Instance.CharacterName));
            if (p != null)
            {
                localPlayer = p;
                PhotonServer.Instance.ListPlayersOperation();
            }
            return;
        }

        TrySend();
    }

    private void MoveLogic()
    {
        for (int i = 0; i < PhotonServer.Instance.Players.Count; i++)
        {
            var player = PhotonServer.Instance.Players[i];
            if (player != localPlayer)
            {
                //player.Position = player.NewPosition;
                player.Position = Vector3.Lerp(player.Position, player.NewPosition, Time.fixedDeltaTime*15f);
            } 
        }
    }

    private void InputUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Move(-Step, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Step, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Move(0, Step, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(0, -Step, 0);
        }
    }

    private void Move(float x,float y,float z)
    {
        if (localPlayer != null)
            localPlayer.Position += new Vector3(x, y, z)*Time.fixedDeltaTime;
    }

    private void TrySend()
    {
        if (localPlayer.Position != oldPosition && lastSendTime < Time.time)
        {
            oldPosition = localPlayer.Position;
            lastSendTime = Time.time + SendRate;

            PhotonServer.Instance.MoveOperation(oldPosition.x, oldPosition.y, oldPosition.z);
        }
    }
}

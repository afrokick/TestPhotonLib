using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    protected Transform transform { get; set; }

    public string CharacterName { get; set; }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 NewPosition { set; get; }

    void Awake()
    {
        transform = base.transform;
        NewPosition = Position;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

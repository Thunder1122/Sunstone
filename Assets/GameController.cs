using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Player DemoPlayer;

    // Use this for initialization
    void Start () {
        DemoPlayer = GameObject.Find("Player1").transform.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

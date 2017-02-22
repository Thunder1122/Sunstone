using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Player DemoPlayer;
    public Button TurnButton;
    public Text ButtonText;
    public Text TimerText;
    public bool Started;
    public bool Ended;
    public float TurnLength;
    public float TimeLeft;


    // Use this for initialization
    void Start () {
        Started = false;
        Ended = false;
        TurnLength = 10.0f;
        TurnButton = GameObject.Find("TurnButton").GetComponent<Button>();
        ButtonText = GameObject.Find("ButtonText").GetComponent<Text>();
        TimerText = GameObject.Find("TimerText").GetComponent<Text>();
        TimerText.text = TurnLength.ToString();
        DemoPlayer = GameObject.Find("Player1").transform.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Started) {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0) {
                TimerText.text = 0.ToString("F2");
                Ended = true;
            }
            else {
                TimerText.text = TimeLeft.ToString("F2");
            }
        }
	}

    public void ButtonClick() {
        Started = !Started;
        if (!Started) {
            Ended = false;
            MakeButtonStart();
        }
        if (Started) {
            TimeLeft = TurnLength;
            MakeButtonEnd();
        }
    }

    public void MakeButtonStart() {
        ButtonText.text = "Start Matching";
        TurnButton.GetComponent<Image>().color = Color.green;
    }

    public void MakeButtonEnd() {
        ButtonText.text = "End Turn";
        TurnButton.GetComponent<Image>().color = Color.red;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Player P1;
    public Player P2;
    public static Player CurrentPlayer;
    public static Player OtherPlayer;

    public Button TurnButton;
    public Text ButtonText;
    public Text TimerText;
    public bool Started;
    public float TurnLength;
    public float TimeLeft;


    public static bool NeedNewBoard;
    public bool MatchTurn;
    public bool FirstTurn;
    public int TurnCount;
    
    public GameObject BlockBoard;


    // Use this for initialization
    void Start () {
        Started = false;
        TurnLength = 10.0f;
        TurnButton = GameObject.Find("TurnButton").GetComponent<Button>();
        ButtonText = GameObject.Find("ButtonText").GetComponent<Text>();
        TimerText = GameObject.Find("TimerText").GetComponent<Text>();
        TimerText.text = TurnLength.ToString();
        
        BlockBoard = GameObject.Find("BlockBoard");

        NeedNewBoard = false;
        TurnCount = 0;
        FirstTurn = true;
        MatchTurn = true;
        CurrentPlayer = P1;
        OtherPlayer = P2;
    }
	
	// Update is called once per frame
	void Update () {
        if (MatchTurn) {
            MakeButtonStart();
            if (Started) {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0) {
                    BlockBoard.SetActive(true); 
                    TimerText.text = 0.ToString("F2");
                    Started = false;
                    SwapPlayers();
                    if (FirstTurn) {
                        FirstTurn = false;
                    } else {
                        MatchTurn = false;
                        FirstTurn = true;
                    }
                    TurnCount++;
                }
                else {
                    TimerText.text = TimeLeft.ToString("F2");
                }
            }
        } else {
            MakeButtonEnd();
            CurrentPlayer.CardBlocker.SetActive(false);
        }
        
	}

    public void ButtonClick() {
        // Start timer if they make matches this turn
        // Otherwise, they ended their turn
        if (MatchTurn) {
            BlockBoard.SetActive(false);
            Started = !Started;
            TimeLeft = TurnLength;
        } else {
            CurrentPlayer.CardBlocker.SetActive(true);
            SwapPlayers();
            if (FirstTurn) {
                FirstTurn = false;
            } else {
                MatchTurn = true;
                FirstTurn = true;
            }
            TurnCount++;
            if (TurnCount % 4 == 0) {
                NeedNewBoard = true;
                SwapPlayers();
            }
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

    public void SwapPlayers() {
        Player temp = CurrentPlayer;
        CurrentPlayer = OtherPlayer;
        OtherPlayer = temp;
    }
}

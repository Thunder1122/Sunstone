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

    public Text GameOverText;
    public Text PlayerTurnText;
    public GameObject BlockBoard;

    public BoardManager Board;


    // Use this for initialization
    void Start () {
        Board = GameObject.Find("Board").GetComponent<BoardManager>();

        Started = false;
        TurnLength = 20.0f;
        TurnButton = GameObject.Find("TurnButton").GetComponent<Button>();
        ButtonText = GameObject.Find("ButtonText").GetComponent<Text>();
        TimerText = GameObject.Find("TimerText").GetComponent<Text>();
        TimerText.text = TurnLength.ToString();

        GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        PlayerTurnText = GameObject.Find("PlayerTurnText").GetComponent<Text>();
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
        PlayerTurnText.text = "Player " + CurrentPlayer.name[6] + "'s Turn";
        if (MatchTurn) {
            CurrentPlayer.CardBlocker.SetActive(false);
            MakeButtonStart();
            if (Started) {
                TurnButton.gameObject.SetActive(false);
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0) {
                    BlockBoard.SetActive(true); 
                    TimerText.text = 0.ToString("F2");
                    Started = false;
                    CurrentPlayer.CardBlocker.SetActive(true);
                    SwapPlayers();
                    if (FirstTurn) {
                        FirstTurn = false;
                        if (Board.SelectedColumn != -1 || Board.SelectedRow != -1) {
                            Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                                GetComponent<BoardCell>().Deselect(
                                Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                                GetComponent<BoardCell>());
                        }
                    } else {
                        if (Board.SelectedColumn != -1 || Board.SelectedRow != -1) {
                            Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                                GetComponent<BoardCell>().Deselect(
                                Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                                GetComponent<BoardCell>());
                        }
                        MatchTurn = false;
                        FirstTurn = true;
                    }
                    TurnButton.gameObject.SetActive(true);
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
        if (CurrentPlayer.PlayerResources[5] == 0 || OtherPlayer.PlayerResources[5] == 0) {
            GameOver();
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
                Board.GenerateBoard();
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

    public void GameOver() {
        Player Winner;
        if (P1.PlayerResources[5] == 0) {
            Winner = P2;
        } else {
            Winner = P1;
        }
        GameOverText.text = "Player " + Winner.name[6] + " wins!";
    }
}

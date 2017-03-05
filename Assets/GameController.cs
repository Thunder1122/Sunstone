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
    public static bool MatchTurn;
    public static bool FirstTurn;
    public int TurnCount;

    public Text GameOverText;
    public Text PlayerTurnText;
    public GameObject BlockBoard;

    public BoardController Board;


    // Use this for initialization
    void Start () {
        Board = GameObject.Find("Board").GetComponent<BoardController>();

        Started = false;
        TurnLength = 15.0f;
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
        if (MatchTurn) {
            PlayerTurnText.text = "Player " + CurrentPlayer.name[6] + "'s Turn to Match";
            CurrentPlayer.CardBlocker.SetActive(false);
            MakeButtonStart();
            if (Started) {
                TurnButton.gameObject.SetActive(false);
                TimeLeft -= Time.deltaTime;
                if (TimeLeft < 0) {
                    BlockBoard.SetActive(true);
                    Board.Deactivate();
                    TimerText.text = 0.ToString("F2");
                    Started = false;
                    CurrentPlayer.CardBlocker.SetActive(true);
                    SwapPlayers();
                    if (FirstTurn) {
                        FirstTurn = false;
                    } else {
                        MatchTurn = false;
                        FirstTurn = true;
                    }
                    
                    if (Board.SelectedColumn != -1 || Board.SelectedRow != -1) {
                        Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                            GetComponent<BoardCell>().Deselect(
                            Board.BoardArray[Board.SelectedRow, Board.SelectedColumn].
                            GetComponent<BoardCell>());
                    }

                    TurnButton.gameObject.SetActive(true);
                    TurnCount++;
                }
                else {
                    TimerText.text = TimeLeft.ToString("F2");
                }
            }
        } else {
            PlayerTurnText.text = "Player " + CurrentPlayer.name[6] + "'s Turn to Play Cards";
            MakeButtonEnd();
            CurrentPlayer.CardBlocker.SetActive(false);
        }
        if (CurrentPlayer.PlayerResources[5] <= 0 || OtherPlayer.PlayerResources[5] <= 0) {
            GameOver();
        }
        
	}

    public void ButtonClick() {
        // Start timer if they make matches this turn
        // Otherwise, they ended their turn
        if (MatchTurn) {
            BlockBoard.SetActive(false);
            Board.Activate();
            Started = !Started;
            TimeLeft = TurnLength;
        } else {
            foreach (GameObject card in CurrentPlayer.CardArray) {
                card.GetComponent<Card>().Used = false;
                card.GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
            }
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
        if (P1.PlayerResources[5] <= 0) {
            P1.PlayerResources[5] = 0;
            Winner = P2;
        } else {
            P2.PlayerResources[5] = 0;
            Winner = P1;
        }
        CurrentPlayer.CardBlocker.SetActive(true);
        OtherPlayer.CardBlocker.SetActive(true);
        TurnButton.gameObject.SetActive(false);
        foreach (GameObject card in P1.CardArray) {
            card.SetActive(false);
        }
        foreach (GameObject card in P2.CardArray) {
            card.SetActive(false);
        }
        foreach (GameObject cell in Board.BoardArray) {
            cell.SetActive(false);  
        }
        GameOverText.text = "Player " + Winner.name[6] + " wins!";
    }

    #region Card Information

    public static Dictionary<int, bool> UsedIDs = new Dictionary<int, bool>();

    public static string[] CardNames = {
        "Nova",
        "Supernova",
        "September",
        "Burn",
        "Eruption",  // 4 
        "Splash",
        "Fire Burst",
        "Water Burst",
        "Commiserate",
        "En Garde",  // 9
        "Ocean's Gambit",
        "Deadly Infusion",
        "Nature Power",
        "Second Wind",
        "Condensation",  // 14
        "Scorched Earth",
        "Breath of Life",
        "Blacksmith",
        "Devious Fog",
        "Flood"  // 19
    };

    public static string[] CardDescriptions = {
        "Deal 4 damage.",
        "Deal 10 damage.",
        "Deal 21 damage (do you remember?).",
        "Deal 6 damage.",
        "Deal 12 damage.",  // 4
        "Deal 1 damage.",
        "Spend all fire mana; deal that amount in damage.",
        "Spend all water mana; half dealt as damage, half recovered as health.",
        "Evenly redistribute player and opponent health.",
        "Lose all shield; deal equivalent amount in damage.",  // 9
        "Effect ranges between healing 5 health and dealing 15 damage, ignoring shield.",
        "Deal 8 damage; give opponent 1 of each mana.",
        "Spend all earth mana; half recovered as health, half recovered as shield.",
        "Restore both players to full health.",
        "Give yourself 4 air mana.",  // 14 
        "Give yourself 4 earth mana.",
        "Swap players' health bars.",
        "Give yourself 6 shield.",
        "Swap players' mana counts.",
        "Steal up to 6 earth mana from opponent."  // 20
    };

    public static int[][] CardCosts = {
        // wind, earth, fire, water, shield, health
        new int[] { 1, 1, 1, 1, 0, 0 },
        new int[] { 2, 2, 2, 2, 0, 0 },
        new int[] { 5, 5, 5, 0, 0, 0 },
        new int[] { 0, 0, 4, 0, 0, 0 },
        new int[] { 0, 2, 6, 0, 0, 0 },  // 4
        new int[] { 0, 0, 0, 1, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0 },
        new int[] { 10, 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0 }, // 9
        new int[] { 0, 0, 0, 6, 0, 0 },
        new int[] { 1, 1, 1, 1, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0 },
        new int[] { 14, 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 4, 0, 0 },  // 14
        new int[] { 0, 0, 4, 0, 0, 0 },
        new int[] { 6, 0, 0, 0, 0, 0 },
        new int[] { 0, 2, 2, 0, 0, 0 },
        new int[] { 8, 0, 0, 2, 0, 0 },
        new int[] { 0, 0, 0, 4, 0, 0 }  // 19
    };

    #endregion
}

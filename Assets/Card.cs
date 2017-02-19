using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int CardDamage;
    public Player CurrentPlayer;
    public Player OtherPlayer;
    public GameObject Payload;          //will store a misc game object with a specific script tied to it. this lets us run custom actions and keep a singular card class.
    public int[] ManaCosts;


    internal void Start()
    {
        CardDamage = Random.Range(0, 20);
        ManaCosts = new int[6] { 1, 1, 1, 1, 0, 0 };
        CurrentPlayer = GameObject.Find("Player1").GetComponent<Player>();
        OtherPlayer = GameObject.Find("Player2").GetComponent<Player>();
    }

    internal void OnMouseDown() {
        // Check mana for card, do action if appropriate
        // Check mana
        for (int i = 0; i < 6; i++) {
            // Break if not enough mana
            if (CurrentPlayer.PlayerResources[i] < ManaCosts[i]) {
                return;
            }
        }

        // Spend mana
        for (int i = 0; i < 6; i++) {
            CurrentPlayer.PlayerResources[i] -= ManaCosts[i];
        }


        // Attack -- placeholder card deals random damage from 0 to 20
        if (OtherPlayer.PlayerResources[5] < CardDamage) {
            OtherPlayer.PlayerResources[5] = 0;
        } else {
            OtherPlayer.PlayerResources[5] -= CardDamage;
        }

        OtherPlayer.HealthSlider.value -= CardDamage;
        OtherPlayer.HealthText.text = OtherPlayer.HealthSlider.value.ToString();

    }

    internal void Update()
    {

    }
}
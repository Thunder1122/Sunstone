using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Track players
    public Player CurrentPlayer;
    public Player OtherPlayer;

    // Damage (positive number) which is a placeholder for misc effects
    public int CardDamage;

    // Mana costs to use this card
    public int[] ManaCosts;

    // Maybe we'll use this later
    // Stores a GameObject with a script tied to it, letting us run custom
    // actions without making multiple card classes
    public GameObject Payload;

    /// <summary>
    /// Initialize card damage and costs
    /// </summary>
    internal void Start()
    {
        // Demo card damage
        CardDamage = Random.Range(0, 20);

        // Demo mana costs
        ManaCosts = new int[6] { 1, 1, 1, 1, 0, 0 };

        // Set players (this needs to be fixed later)
        CurrentPlayer = GameObject.Find("Player1").GetComponent<Player>();
        OtherPlayer = GameObject.Find("Player2").GetComponent<Player>();
    }

    /// <summary>
    /// Choose card, if appropriate
    /// </summary>
    internal void OnMouseDown() {
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

        // Modify display of other player health
        OtherPlayer.HealthSlider.value -= CardDamage;
        OtherPlayer.HealthText.text = OtherPlayer.HealthSlider.value.ToString();

    }

    internal void Update()
    {
        // Uh, nothing.
    }
}
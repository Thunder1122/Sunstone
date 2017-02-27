using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Card : GameController
{
    // Damage (positive number) which is a placeholder for misc effects
    public int CardDamage;
    public string Name = "Demo Card";
    public string Description = "Deals random damage between 10 and 40";

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
        // Demo mana costs
        // Air, Earth, Fire, Water, Metal, Life
        ManaCosts = new int[6] { Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), 0, 0 };
    }

    /// <summary>
    /// Choose card, if appropriate
    /// </summary>
    internal void OnMouseDown()
    {
        // Demo card damage
        CardDamage = Random.Range(10, 40);

        // Check mana
        for (int i = 0; i < 6; i++)
        {
            // Break if not enough mana
            if (CurrentPlayer.PlayerResources[i] < ManaCosts[i])
            {
                return;
            }
        }

        // Spend mana
        for (int i = 0; i < 6; i++)
        {
            CurrentPlayer.PlayerResources[i] -= ManaCosts[i];
        }

        // Attack -- placeholder card deals random damage from 0 to 20
        if (OtherPlayer.PlayerResources[5] < CardDamage)
        {
            OtherPlayer.PlayerResources[5] = 0;
        }
        else
        {
            int PotentialDamage = CardDamage - OtherPlayer.PlayerResources[4];
            if (PotentialDamage > 0) {
                OtherPlayer.PlayerResources[4] = 0;
                OtherPlayer.PlayerResources[5] -= PotentialDamage;
            } else {
                OtherPlayer.PlayerResources[4] -= CardDamage;
            }
        }

        // Modify display of other player health
        OtherPlayer.HealthSlider.value -= CardDamage;
        OtherPlayer.HealthText.text = OtherPlayer.HealthSlider.value.ToString();
    }

    internal void Update()
    {

    }

    private void OnMouseEnter()
    {
        DisplayCardInfo(transform.parent.GetComponentInParent<Player>());
    }

    private void OnMouseExit()
    {
        HideCardInfo(transform.parent.GetComponentInParent<Player>());
    }

    private void DisplayCardInfo(Player Owner) {
        Owner.AirCostText.text = ManaCosts[0].ToString();
        Owner.EarthCostText.text = ManaCosts[1].ToString();
        Owner.FireCostText.text = ManaCosts[2].ToString();
        Owner.WaterCostText.text = ManaCosts[3].ToString();
        Owner.MetalCostText.text = ManaCosts[4].ToString();
        Owner.LifeCostText.text = ManaCosts[5].ToString();
        Owner.CardName.text = Name;
        Owner.CardDescription.text = Description;
    }

    private void HideCardInfo(Player Owner) {
        Owner.AirCostText.text = 0.ToString();
        Owner.EarthCostText.text = 0.ToString();
        Owner.FireCostText.text = 0.ToString();
        Owner.WaterCostText.text = 0.ToString();
        Owner.MetalCostText.text = 0.ToString();
        Owner.LifeCostText.text = 0.ToString();
        Owner.CardName.text = "";
        Owner.CardDescription.text = "";
    }
}
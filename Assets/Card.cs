using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    // GUI Label vars, used to display card stats on screen.
    private GUIStyle GuiStyle = new GUIStyle();
    public Rect LabelRect;
    public bool ShowValue;
    StringBuilder sb = new StringBuilder();
    string[] ManaTypes = { "Air: ", "Earth: ", "Fire: ", "Water: " };
    string LabelString;

    /// <summary>
    /// Initialize card damage and costs
    /// </summary>
    internal void Start()
    {
        // Demo card damage
        CardDamage = Random.Range(10, 40);

        // Demo mana costs
        ManaCosts = new int[6] { Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), 0, 0 };

        // Set players
        CurrentPlayer = transform.parent.parent.GetComponent<Player>();
        OtherPlayer = GameObject.Find(transform.parent.parent.GetComponent<Player>().PlayerID == 0 ? "Player2" : "Player1").GetComponent<Player>();

        // Set GUI Style params
        GuiStyle.fontSize = 20;

        // Generate the tooltip string for this card.
        LabelString = LabelMaker();

        // Calculate the offset for card stats display
        LabelRect = new Rect(100 + (1400 * CurrentPlayer.PlayerID), 100, 40, 80);
    }

    /// <summary>
    /// Choose card, if appropriate
    /// </summary>
    internal void OnMouseDown()
    {
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

    private void OnMouseEnter()
    {
        ShowValue = true;
    }

    private void OnMouseExit()
    {
        ShowValue = false;
    }

    public string LabelMaker()
    {
        sb.Remove(0, sb.Length);
        sb.Append("Card Damage: ");
        sb.Append(CardDamage);
        sb.Append("\n\n");
        for (int i = 0; i < 5; i++)
        {
            if (ManaCosts[i] != 0)
            {
                sb.Append(ManaTypes[i]);
                sb.Append(ManaCosts[i]);
                sb.Append('\n');
            }
        }
        return sb.ToString();
    }

    private void OnGUI()
    {
        if (ShowValue)
        {
            GUI.Label(LabelRect, LabelString, GuiStyle);
        }
    }
}
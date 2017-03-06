using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : GameController
{
    // Internal card storage
    public GameObject[] CardArray = new GameObject[10];

    // Parameterize shield and health
    public int StartingShield = 0;
    public int MaxShield = 25;
    public int StartingHealth = 100;
    public int MaxHealth = 100;
    public int StartingMana = 5;

    // Track player mana, shield, health
    public Dictionary<int, int> PlayerResources;

    // UI sliders to track shield, health
    public Slider ShieldSlider;
    public Slider HealthSlider;

    // UI text to appear in game
    public Text WaterText, FireText, EarthText, WindText, ShieldText, HealthText;
    public Text WindCostText, EarthCostText, FireCostText, WaterCostText, MetalCostText, LifeCostText;
    public Text CardName, CardDescription;
    public GameObject CardBlocker;
    
    /// <summary>
    /// Set all player resources to defaults
    /// </summary>
    internal void Start() {
        // Initialize 0 - 3 (wind, earth, fire, water) to 0, shield and health as
        // appropriate
        PlayerResources = new Dictionary<int, int> {
            {0, StartingMana}, {1, StartingMana}, {2, StartingMana}, {3, StartingMana}, {4, StartingShield}, {5, StartingHealth}
        };

        UpdatePlayerUI();
    }

    /// <summary>
    /// Keep UI up to date
    /// </summary>
    internal void Update()
    {
        UpdatePlayerUI();
    }

    /// <summary>
    /// Sync displayed text and player resources
    /// </summary>
    public void UpdatePlayerUI() {
        WindText.text = PlayerResources[0].ToString();
        EarthText.text = PlayerResources[1].ToString();
        FireText.text = PlayerResources[2].ToString();
        WaterText.text = PlayerResources[3].ToString();

        ShieldSlider.value = PlayerResources[4];
        ShieldText.text = PlayerResources[4].ToString();

        HealthSlider.value = PlayerResources[5];
        HealthText.text = PlayerResources[5].ToString();

        return;
    }

    /// <summary>
    /// Modify mana amounts (generalzed "take damage / spend mana" function)
    /// </summary>
    /// <param name="amounts">6 element list; each contains the change in that type of resource</param>
    public void ChangeMana(int[] amounts)
    {
        // Change resources appropriately
        for (int i = 0; i < 4; i++) {
            PlayerResources[i] += amounts[i];
        }

        // Do not exceed the max shield or max health
        if ((amounts[4] > 0 && PlayerResources[4] >= MaxShield) || (amounts[5] > 0 && PlayerResources[5] >= MaxHealth)) {
            return;
        } else {
            for (int i = 4; i < 6; i++) {
                PlayerResources[i] += amounts[i];
                if (i == 4) {
                    PlayerResources[i] += 2*amounts[i];
                }
            }
        }
        

        return;
    }

}
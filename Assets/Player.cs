using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Internal card storage
    public GameObject[] CardArray = new GameObject[10];

    // Parameterize shield and health
    public int StartingShield = 0;
    public int MaxShield = 25;
    public int StartingHealth = 100;
    public int MaxHealth = 100;

    // Track player mana, shield, health
    public Dictionary<int, int> PlayerResources;

    // UI sliders to track shield, health
    public Slider ShieldSlider;
    public Slider HealthSlider;

    // UI text to appear in game
    public Text WaterText, FireText, EarthText, AirText, ShieldText, HealthText;

    /// <summary>
    /// Set all player resources to defaults
    /// </summary>
    internal void Start() {
        // Initialize 0 - 3 (air, earth, fire, water) to 0, shield and health as
        // appropriate
        PlayerResources = new Dictionary<int, int> {
            {0, 0}, {1, 0}, {2, 0}, {3, 0}, {4, StartingShield}, {5, StartingHealth}
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
        AirText.text = PlayerResources[0].ToString();
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
        // Do not exceed the max shield or max health
        if ((amounts[4] > 0 && PlayerResources[4] >= MaxShield) || (amounts[5] > 0 && PlayerResources[5] >= MaxHealth)) {
            return;
        }

        // Change resources appropriately
        for (int i = 0; i < 6; i++) {
            PlayerResources[i] += amounts[i];
        }

        return;
    }

}
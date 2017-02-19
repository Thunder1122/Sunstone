using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Internal card storage
    public GameObject[] CardArray = new GameObject[10];

    // Parameterize initial health value
    public int StartingHealth = 100;
    public int MaxHealth = 100;
    public int StartingShield = 0;
    public int MaxShield = 25;

    // Track player mana
    public Dictionary<int, int> PlayerResources;

    // UI sliders to track health, shield
    public Slider HealthSlider;
    public Slider ShieldSlider;

    public Text WaterText, FireText, EarthText, AirText, ShieldText, HealthText;

    internal void Awake()
    {
        
    }

    internal void Start() {
        // 5 is health, start at 100
        // 0-3 are Air, Earth, Fire, Water in that order
        PlayerResources = new Dictionary<int, int> {
            {0,0}, {1,0}, {2,0}, {3,0}, {4,StartingShield}, {5,StartingHealth}
        };

        UpdatePlayerUI();
    }

    internal void Update()
    {
        UpdatePlayerUI();
    }

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

    public void ChangeMana(int[] amounts)
    {
        if ((amounts[4] != 0 && PlayerResources[4] >= MaxShield) || (amounts[5] != 0 && PlayerResources[5] >= MaxHealth)) {
            return;
        }

        for (int i=0; i<6; i++) {
            PlayerResources[i] += amounts[i];
        }
        return;
    }

}
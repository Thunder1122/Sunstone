using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Card : GameController
{
    public Player Parent;
    public int ID;
    public string Name;
    public string Description;
    public int[] Costs = new int[7];
    public bool Used;

    /// <summary>
    /// Initialize card damage and costs
    /// </summary>
    internal void Start() {
        Parent = transform.parent.GetComponentInParent<Player>();

        do {
            ID = Random.Range(0, 20);
        } while (UsedIDs.ContainsKey(ID));

        UsedIDs.Add(ID, true);
        Name = CardNames[ID];
        Description = CardDescriptions[ID];

        Costs = CardCosts[ID];
        Used = false;
    }

    /// <summary>
    /// Choose card, if appropriate
    /// </summary>
    internal void OnMouseDown()
    {   
        if (MatchTurn || Used) {
            return;
        }

        // Check mana
        if (!EnoughMana(Parent)) {
            return;
        }

        int Damage;
        int Heal;
        int Resource;
        int temp;

        switch (ID) {
            case 0:
                DealDamage(4);
                SubtractMana(Costs);
                break;

            case 1:
                DealDamage(10);
                SubtractMana(Costs);
                break;

            case 2:
                DealDamage(21);
                SubtractMana(Costs);
                break;

            case 3:
                DealDamage(6);
                SubtractMana(Costs);
                break;

            case 4:
                DealDamage(12);
                SubtractMana(Costs);
                break;

            case 5:
                DealDamage(1);
                SubtractMana(Costs);
                break;

            case 6:
                Damage = Parent.PlayerResources[2];
                DealDamage(Damage);
                Parent.PlayerResources[2] = 0;
                break;

            case 7:
                Resource = Parent.PlayerResources[3];
                Heal = (int)Resource / 2;
                Damage = (int)Resource / 2;
                DealDamage(Damage);
                SubtractMana(new int[] { 0, 0, 0, Resource, 0, Heal });
                break;

            case 8:
                int PooledHealth = CurrentPlayer.PlayerResources[5] + OtherPlayer.PlayerResources[5];
                int NewHealth = (int)PooledHealth / 2;
                CurrentPlayer.PlayerResources[5] = NewHealth;
                OtherPlayer.PlayerResources[5] = NewHealth;
                SubtractMana(Costs);
                break;

            case 9:
                DealDamage(CurrentPlayer.PlayerResources[4]);
                CurrentPlayer.PlayerResources[4] = 0;
                break;

            case 10:
                Damage = Random.Range(-5, 16);
                OtherPlayer.PlayerResources[5] -= Damage;
                SubtractMana(Costs);
                break;

            case 11:
                DealDamage(8);
                SubtractMana(Costs);
                for (int i = 0; i < 4; i++) { OtherPlayer.PlayerResources[i]++; }
                break;

            case 12:
                Resource = Parent.PlayerResources[1];
                Heal = (int)Resource / 2;
                SubtractMana(new int[] { 0, Resource, 0, 0, -Heal, Heal });
                break;

            case 13:
                CurrentPlayer.PlayerResources[5] = 100;
                OtherPlayer.PlayerResources[5] = 100;
                SubtractMana(Costs);
                break;

            case 14:
                CurrentPlayer.PlayerResources[3] += 4;
                SubtractMana(Costs);
                break;

            case 15:
                CurrentPlayer.PlayerResources[1] += 4;
                SubtractMana(Costs);
                break;

            case 16:
                temp = CurrentPlayer.PlayerResources[5];
                CurrentPlayer.PlayerResources[5] = OtherPlayer.PlayerResources[5];
                OtherPlayer.PlayerResources[5] = temp;
                SubtractMana(Costs);
                break;

            case 17:
                SubtractMana(new int[] { 0, 2, 2, 0, -6, 0 });
                break;

            case 18:
                SubtractMana(Costs);
                for (int i = 0; i < 4; i++) {
                    temp = CurrentPlayer.PlayerResources[i];
                    CurrentPlayer.PlayerResources[i] = OtherPlayer.PlayerResources[i];
                    OtherPlayer.PlayerResources[i] = temp;
                }
                break;

            case 19:
                if (OtherPlayer.PlayerResources[1] < 6) {
                    CurrentPlayer.PlayerResources[1] += OtherPlayer.PlayerResources[1];
                    OtherPlayer.PlayerResources[1] = 0;
                } else {
                    CurrentPlayer.PlayerResources[1] += 6;
                    OtherPlayer.PlayerResources[1] -= 6;
                }
                break;

            default:
                Debug.Log("Ya dun fucked up");
                break;
        }

        GetComponent<SpriteRenderer>().color = Color.gray;
        Used = true;
    }

    internal void Update()
    {

    }

    private void OnMouseEnter()
    {
        if (CurrentPlayer == Parent) {
            DisplayCardInfo(Parent);
        }
    }

    private void OnMouseExit() { 
        if (CurrentPlayer == Parent) {
            HideCardInfo(Parent);
        }
    }

    private void DealDamage(int Damage) {
        int Shield = OtherPlayer.PlayerResources[4];
        while (Shield > 0 && Damage > 0) {
            OtherPlayer.PlayerResources[4]--;
            Shield--;
            Damage--;
        }
        if (Damage > 0) {
            OtherPlayer.PlayerResources[5] -= Damage;
        }

    }

    /// <summary>
    /// Modify mana amounts (generalzed "take damage / spend mana" function)
    /// </summary>
    /// <param name="amounts">6 element list; each contains the change in that type of resource</param>
    public void SubtractMana(int[] amounts) {
        // Change resources appropriately
        for (int i = 0; i < 4; i++) {
            CurrentPlayer.PlayerResources[i] -= amounts[i];
        }

        while (amounts[5] > 0 && CurrentPlayer.PlayerResources[5] < 100) {
            CurrentPlayer.PlayerResources[5]++;
            amounts[5]--;
        }

        while (amounts[4] > 0 && CurrentPlayer.PlayerResources[4] > 0) {
            CurrentPlayer.PlayerResources[4]--;
            amounts[4]--;
        }
        
        while (amounts[4] < 0 && CurrentPlayer.PlayerResources[4] < 25) {
            CurrentPlayer.PlayerResources[4]++;
            amounts[4]++;
        }

        return;
    }

    private bool EnoughMana(Player Owner) {
        for (int i = 0; i < 6; i++) {
            // Break if not enough mana
            if (Owner.PlayerResources[i] < Costs[i]) {
                return false;
            }
        }
        return true;
    }

    private void DisplayCardInfo(Player Owner) {
        Owner.WindCostText.text = Costs[0].ToString();
        if (ID == 12) {
            Owner.EarthCostText.text = Owner.PlayerResources[1].ToString();
        }
        else {
            Owner.EarthCostText.text = Costs[1].ToString();
        }
        if (ID == 6) {
            Owner.FireCostText.text = Owner.PlayerResources[2].ToString();
        }
        else {
            Owner.FireCostText.text = Costs[2].ToString();
        }
        if (ID == 7) {
            Owner.WaterCostText.text = Owner.PlayerResources[3].ToString();
        }
        else {
            Owner.WaterCostText.text = Costs[3].ToString();
        }
        if (ID == 9) {
            Owner.MetalCostText.text = Owner.PlayerResources[4].ToString();
        }
        else {
            Owner.MetalCostText.text = Costs[4].ToString();
        }
        Owner.LifeCostText.text = Costs[5].ToString();
        Owner.CardName.text = Name;
        Owner.CardDescription.text = Description;
    }

    private void HideCardInfo(Player Owner) {
        Owner.WindCostText.text = 0.ToString();
        Owner.EarthCostText.text = 0.ToString();
        Owner.FireCostText.text = 0.ToString();
        Owner.WaterCostText.text = 0.ToString();
        Owner.MetalCostText.text = 0.ToString();
        Owner.LifeCostText.text = 0.ToString();
        Owner.CardName.text = "";
        Owner.CardDescription.text = "";
    }
}
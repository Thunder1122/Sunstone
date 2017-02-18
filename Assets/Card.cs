using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int CardDamage;
    public Player CurrentPlayer;
    public GameObject Payload;          //will store a misc game object with a specific script tied to it. this lets us run custom actions and keep a singular card class.

    internal void Start()
    {
        CardDamage = Random.Range(0, 20);
        CurrentPlayer = transform.parent.GetComponent<Player>();
    }

    internal void Update()
    {

    }
}
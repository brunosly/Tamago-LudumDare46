using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggLifeCounter : MonoBehaviour
{
    GameObject owner;
    string text;
    void Update()
    {
        if (owner != null)
        {
            text = "Egg Life: " + owner.GetComponent<Piece>().actualHealth + "/" + owner.GetComponent<Piece>().startingHealth;
            this.gameObject.GetComponent<Text>().text = text;
        }
        else
        {
            owner = GameObject.Find("Egg");
        }
    }
}
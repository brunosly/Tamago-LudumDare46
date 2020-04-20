using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
    public string objectClass;
    GameObject owner;
    string text;

    void Update()
    {
        if (owner != null)
        {
            text = owner.name + "\nlife: " + owner.GetComponent<Piece>().actualHealth + "/" + owner.GetComponent<Piece>().startingHealth + "\nDamage: " + owner.GetComponent<Piece>().damage + "\nAction points: " + owner.GetComponent<Piece>().actionPoints;
            this.gameObject.GetComponent<Text>().text = text;
        }
        else
        {
            owner = GameObject.Find(objectClass);
        }
    }
}

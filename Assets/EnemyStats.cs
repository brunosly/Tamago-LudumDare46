using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class EnemyStats : MonoBehaviour
{
    public GameObject owner;
    string text;

    void Start()
    {
        if (owner != null)
        {
            text = "\nlife: " + owner.GetComponent<Piece>().actualHealth + "/" + owner.GetComponent<Piece>().startingHealth + "\nDamage: " + owner.GetComponent<Piece>().damage;
            if (owner.GetComponent<Piece>().type == PieceType.Trap)
            {
                text = "\nlife: " + owner.GetComponent<Piece>().actualHealth + "/" + owner.GetComponent<Piece>().startingHealth + "\nDamage: " + owner.GetComponent<Trap>().explosionDamage; //its 20:39 pf sunday and this is the last line of code!
            }
            this.gameObject.GetComponent<Text>().text = text;
        }

    }
}

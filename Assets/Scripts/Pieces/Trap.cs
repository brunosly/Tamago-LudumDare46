using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Enemy
{
    int createdRound;
    int explosionRound;
    public int duration;
    public int explosionRange;
    public int explosionDamage;
    Vector2Int originalPosition;

    void Start()
    {
        createdRound = GameManager.gameManagerInstance.roundCounter;
        explosionRound = createdRound + duration;
    }


    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>();

        return locations;
    }

    public override List<Vector2Int> AttackLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(samePlace);

        return locations;
    }

    public override GameObject DecideChaseTarget(List<GameObject> possibleTargets)
    {
        minusOneTurn();
        return this.gameObject;
    }

    public override GameObject DecideAttackTarget(List<GameObject> possibleTargets)
    {
        return this.gameObject;
    }

    public void minusOneTurn ()
    {
        if (explosionRound == GameManager.gameManagerInstance.roundCounter)
        {
            this.gameObject.GetComponent<Piece>().damage++;
        }
    }


    public void setOriginalPosition ()
    {
        originalPosition = GameManager.gameManagerInstance.GridForPiece(this.gameObject);
        Debug.Log(originalPosition);
    }
    
    void OnDestroy()
    {
        int usableExplosionPoints;
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        locations.Add(originalPosition);
        usableExplosionPoints = explosionRange;
        while (usableExplosionPoints > 0)
        {
            int forsize = locations.Count;
            for (int i = 0; i < forsize; i++)
            {
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int nextGridPoint = new Vector2Int(locations[i].x + dir.x, locations[i].y + dir.y);
                    if (locations.Contains(nextGridPoint) == false)
                    {
                        locations.Add(nextGridPoint);
                    }
                }
            }
            usableExplosionPoints--;
        }
        foreach (Vector2Int location in locations)
        {
            GameManager.gameManagerInstance.Explosion(explosionDamage, location);
        }
    }
    
    
}

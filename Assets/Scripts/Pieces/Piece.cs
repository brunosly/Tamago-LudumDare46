using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { Egg, DPS, Tank, Trapper, Healer, NullPiece, Enemy1, Trap };

public abstract class Piece : MonoBehaviour
{
    protected GameManager gamemanager;
    public PieceType type;
    public int startingActionPoints;
    public int startingHealth;
    public int damage;

    public int actionPoints;
    public int actualHealth;

    public int movementPoints;
    public int attackRange;

    protected Vector2Int[] nonDiagonalDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] diagonalDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};
    protected Vector2Int[] samePlace = {new Vector2Int(0,0)};

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
    public abstract List<Vector2Int> AttackLocations(Vector2Int gridPoint);

    public void RechargeActionPoints()
    {
        actionPoints = startingActionPoints;
    }

    void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
        RechargeActionPoints();
        actualHealth = startingHealth;
    }
}

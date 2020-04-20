using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public Board board;

    public GameObject egg;
    public GameObject guardian1;
    public GameObject guardian2;
    public GameObject guardian3;
    public GameObject guardian4;
    public GameObject enemyPiece1;
    public GameObject trap;
    
    public GameObject nullPieceType;

    public int boardMaxSize = 71;

    public int enemySpawnRate;
    public float baseSpawnChance;
    public int spawnChanceIncreaseRate;
    public float spawnChanceIncreasePercentage;
    public int spawnQuantityIncreaseRate;


    public PieceOnLevel[] playerPiece;
    public PieceOnLevel[] enemyPiece;

    [HideInInspector] public int roundCounter = 1;

    public List<GameObject> playerPieceWithNulls = new List<GameObject>();
    public List<GameObject> playerPieceAlive = new List<GameObject>();
    public List<GameObject> enemyPieceAlive = new List<GameObject>();

    private GameObject[,] pieces;

    private Player player;
    private Player enemy;
    private Player neutral;
    public Player currentPlayer;
    public Player otherPlayer;

    void Awake()
    {
        gameManagerInstance = this;
    }

    void Start()
    {

        pieces = new GameObject[boardMaxSize, boardMaxSize];

        player = new Player("Player");
        enemy = new Player("Enemy");
        neutral = new Player("Neutral");

        currentPlayer = player;
        otherPlayer = enemy;

        InitialSetup();
        SetupNeutralPiece();
    }

    private void InitialSetup()
    {
        foreach (PieceOnLevel playerPiece in playerPiece)
        {
            AddPiece(playerPiece.piece, player, playerPiece.col, playerPiece.row);
        }

        foreach (PieceOnLevel enemyPiece in enemyPiece)
        {
            AddPiece(enemyPiece.piece, enemy, enemyPiece.col, enemyPiece.row);
        }
        UIManager.uiManagerInstance.StartTurn();
    }

    private void SetupNeutralPiece()
    {
        NullPiece[] nullpieces = Object.FindObjectsOfType<NullPiece>();
        foreach (NullPiece neutralPiece in nullpieces)
        {
            Vector2Int gridPoint = Geometry.GridFromPoint(new Vector3(neutralPiece.transform.position.x, 0, neutralPiece.transform.position.z));
            AddPiece(nullPieceType, neutral, gridPoint.x, gridPoint.y);
        }
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        if (NoPieceAt(new Vector2Int(col, row)))
        {
            GameObject pieceObject = board.AddPiece(prefab, col, row);

            player.pieces.Add(pieceObject);
            pieces[col, row] = pieceObject;
            if (player == enemy)
            {
                enemyPieceAlive.Add(pieceObject);
                if (pieceObject.GetComponent<Piece>().type == PieceType.Trap)
                {
                    pieceObject.GetComponent<Trap>().setOriginalPosition();
                }
            }
            else
            {
                playerPieceWithNulls.Add(pieceObject);
                if(pieceObject.GetComponent<Piece>().type != PieceType.NullPiece)
                {
                    playerPieceAlive.Add(pieceObject);
                }
            }
        }
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece) 
    {
        for (int i = 0; i < boardMaxSize; i++)
        {
            for (int j = 0; j < boardMaxSize; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public bool EnemyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (currentPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public bool NoPieceAt (Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool DoesPieceBelongToPlayer(GameObject piece)
    {
        return playerPieceWithNulls.Contains(piece);
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
    }

    public void Attack(GameObject attackingPiece, Vector2Int gridPoint)
    {
        GameObject damagedPiece = PieceAtGrid(gridPoint);
        damagedPiece.GetComponent<Piece>().actualHealth -= attackingPiece.GetComponent<Piece>().damage;
        UIManager.uiManagerInstance.instantiateDamageText(damagedPiece, attackingPiece.GetComponent<Piece>().damage);
        //board.tookDamage(damagedPiece);
        if (damagedPiece.GetComponent<Piece>().actualHealth <= 0)
        {
            damagedPiece.GetComponent<Piece>().actualHealth = 0;
            pieces[gridPoint.x, gridPoint.y] = null;
            if (DoesPieceBelongToPlayer(damagedPiece))
            {
                playerPieceAlive.Remove(damagedPiece);
                if (playerPieceAlive.Count <= 1 || damagedPiece.GetComponent<Piece>().type == PieceType.Egg)
                {
                    UIManager.uiManagerInstance.GameOver();
                }
            }
            else
            {
                enemyPieceAlive.Remove(damagedPiece);
            }
            Destroy(damagedPiece);
        }
    }

    public void Explosion(int explosionDamage, Vector2Int gridPoint)
    {
        if (NoPieceAt(gridPoint) == false)
        {
            GameObject damagedPiece = PieceAtGrid(gridPoint);
            damagedPiece.GetComponent<Piece>().actualHealth -= explosionDamage;
            UIManager.uiManagerInstance.instantiateDamageText(damagedPiece, explosionDamage);
            if (damagedPiece.GetComponent<Piece>().actualHealth <= 0)
            {
                damagedPiece.GetComponent<Piece>().actualHealth = 0;
                pieces[gridPoint.x, gridPoint.y] = null;
                if (DoesPieceBelongToPlayer(damagedPiece))
                {
                    playerPieceAlive.Remove(damagedPiece);

                    if (playerPieceAlive.Count <= 1 || damagedPiece.GetComponent<Piece>().type == PieceType.Egg)
                    {
                        UIManager.uiManagerInstance.GameOver();
                    }
                }
                else
                {
                    enemyPieceAlive.Remove(damagedPiece);
                }
                Destroy(damagedPiece);
            }
        }
    }

    public void Heal(GameObject HealingPiece, Vector2Int gridPoint)
    {
        GameObject healedPiece = PieceAtGrid(gridPoint);
        healedPiece.GetComponent<Piece>().actualHealth += HealingPiece.GetComponent<Healer>().heal;
        if (healedPiece.GetComponent<Piece>().actualHealth + HealingPiece.GetComponent<Healer>().heal > healedPiece.GetComponent<Piece>().startingHealth)
        {
            healedPiece.GetComponent<Piece>().actualHealth = healedPiece.GetComponent<Piece>().startingHealth;
        }
    }

    public void Trap(GameObject trappingPiece, Vector2Int gridPoint)
    {
        AddPiece(trap, enemy, gridPoint.x, gridPoint.y);
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.MoveLocations(gridPoint);
        locations.RemoveAll(tile => tile.x < 0 || tile.x > boardMaxSize || tile.y < 0 || tile.y > boardMaxSize);
        locations.RemoveAll(tile => FriendlyPieceAt(tile));
        locations.RemoveAll(tile => EnemyPieceAt(tile));

        return locations;
    }

    public List<Vector2Int> AttackForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.AttackLocations(gridPoint);
        locations.RemoveAll(tile => tile.x < 0 || tile.x > boardMaxSize || tile.y < 0 || tile.y > boardMaxSize);
        locations.RemoveAll(tile => FriendlyPieceAt(tile));

        return locations;
    }

    public List<Vector2Int> HealForPiece(GameObject pieceObject)
    {
        Healer piece = pieceObject.GetComponent<Healer>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.HealLocations(gridPoint);
        locations.RemoveAll(tile => tile.x < 0 || tile.x > boardMaxSize || tile.y < 0 || tile.y > boardMaxSize);

        return locations;
    }

    public List<Vector2Int> TrapForPiece(GameObject pieceObject)
    {
        Trapper piece = pieceObject.GetComponent<Trapper>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        var locations = piece.TrapLocations(gridPoint);
        locations.RemoveAll(tile => tile.x < 0 || tile.x > boardMaxSize || tile.y < 0 || tile.y > boardMaxSize);
        locations.RemoveAll(tile => FriendlyPieceAt(tile));
        locations.RemoveAll(tile => EnemyPieceAt(tile));

        return locations;
    }


    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;

        if (currentPlayer == player)
        {
            roundCounter++;
            SpawnEnemies();
            UIManager.uiManagerInstance.StartTurn();
            foreach (GameObject pieceAlive in playerPieceAlive)
            {
                pieceAlive.GetComponent<Piece>().RechargeActionPoints();
            }
        } else 
        {
            EnemyTurn();
        }

    }

    public void EnemyTurn()
    {

        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject enemyPiece in enemyPieceAlive)
        {
            temp.Add(enemyPiece);
        }
        StartCoroutine(EnemyPiecePlay(0, temp));
        StartCoroutine(enemiesPlayed(temp));
    }


    IEnumerator EnemyPiecePlay(float waitTime, List<GameObject> temp)
    {
        foreach (GameObject enemyPiece in temp)
        {
            yield return new WaitForSeconds(waitTime);
            GameObject attackTarget = enemyPiece.GetComponent<Enemy>().DecideAttackTarget(enemyPiece.GetComponent<Enemy>().VerifyEnemiesIsInAttackRange(playerPieceAlive));
            if (attackTarget != null)
            {
                GameObject chaseTarget = enemyPiece.GetComponent<Enemy>().DecideChaseTarget(playerPieceAlive);
                Move(enemyPiece, enemyPiece.GetComponent<Enemy>().verifyFastestPathToTarget(chaseTarget));
                Attack(enemyPiece, GridForPiece(attackTarget));
            }
            else
            {
                GameObject chaseTarget = enemyPiece.GetComponent<Enemy>().DecideChaseTarget(playerPieceAlive);
                Move(enemyPiece, enemyPiece.GetComponent<Enemy>().verifyFastestPathToTarget(chaseTarget));
                attackTarget = enemyPiece.GetComponent<Enemy>().DecideAttackTarget(enemyPiece.GetComponent<Enemy>().VerifyEnemiesIsInAttackRange(playerPieceAlive));
                if (attackTarget != null)
                {
                    Attack(enemyPiece, GridForPiece(attackTarget));
                }
            }
        }
    }

    IEnumerator enemiesPlayed(List<GameObject> temp)
    {
        yield return new WaitForSeconds(0.25f + 0.01f);
        NextPlayer();
    }

    public void SpawnEnemies()
    {
        if (roundCounter % enemySpawnRate == 0)
        {
            float spawnChance = ChancesOfSpawningEnemy();
            var rng = new Random();

            if (Random.Range(0.0f, 100.0f) < spawnChance)
            {
                int spawnQuantity = roundCounter / spawnQuantityIncreaseRate;
                for (int i = 0; i < spawnQuantity; i++)
                {
                    int side = Random.Range(0,4);
                    if (side == 0)
                    {
                        AddPiece(enemyPiece1, enemy, Random.Range(5, 19), 1);
                    }
                    if (side == 1)
                    {
                        AddPiece(enemyPiece1, enemy, 1, Random.Range(5, 19));
                    }
                    if (side == 2)
                    {
                        AddPiece(enemyPiece1, enemy, Random.Range(5, 19), 23);
                    }
                    if (side == 3)
                    {
                        AddPiece(enemyPiece1, enemy, 23, Random.Range(5, 19));
                    }

                }
            }
        }
    }

    public float ChancesOfSpawningEnemy()
    {
        return baseSpawnChance + (spawnChanceIncreasePercentage * (roundCounter % spawnChanceIncreaseRate));
    }
}

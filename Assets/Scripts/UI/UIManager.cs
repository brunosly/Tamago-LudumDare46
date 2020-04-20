using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManagerInstance;
    public float xOffset;
    public float yOffset;

    public float xOffsetDamage;
    public float yOffsetDamage;

    public GameObject damageText;

    public GameObject board;

    public GameObject movementMenuDPSPrefab;

    public GameObject movementMenuTankPrefab;

    public GameObject movementMenuHealerPrefab;

    public GameObject movementMenuTrapperPrefab;

    public GameObject passTurnButtonPrefab;

    public Text roundCounterText;

    public GameObject gameOverScreen;

    GameObject movementMenu;

    GameObject passTurnButton;

    GameObject movingPiece;

    GameObject enemyInformationinst;

    public GameObject enemyInformationPrefab;

    void Awake()
    {
        uiManagerInstance = this;
    }

    private void Update()
    {
        enemyInformation();
    }

    public void OpenPieceControlMenu(GameObject piece)
    {
        movingPiece = piece;
        
        if (piece.GetComponent<Piece>().type == PieceType.DPS)
        {
            movementMenu = Instantiate(movementMenuDPSPrefab, piece.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
        }

        if (piece.GetComponent<Piece>().type == PieceType.Tank)
        {
            movementMenu = Instantiate(movementMenuTankPrefab, piece.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
        }

        if (piece.GetComponent<Piece>().type == PieceType.Trapper)
        {
            movementMenu = Instantiate(movementMenuTrapperPrefab, piece.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
        }

        if (piece.GetComponent<Piece>().type == PieceType.Healer)
        {
            movementMenu = Instantiate(movementMenuHealerPrefab, piece.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
        }


    }

    public void moveButtonPressed()
    {
        Destroy(movementMenu);
        MoveSelector move = board.GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
    }

    public void attackButtonPressed()
    {
        Destroy(movementMenu);
        AttackSelector attack = board.GetComponent<AttackSelector>();
        attack.EnterState(movingPiece);
    }
    public void healButtonPressed()
    {
        Destroy(movementMenu);
        HealSelector move = board.GetComponent<HealSelector>();
        move.EnterState(movingPiece);
    }

    public void StartTurn()
    {
        passTurnButton = Instantiate(passTurnButtonPrefab, new Vector3(79.5f, 56.6f, 0), Quaternion.identity, this.gameObject.transform);
        roundCounterText.text = "Round:" + GameManager.gameManagerInstance.roundCounter.ToString();
    }

    public void passTurnButtonPressed()
    {
        passTurnButton.GetComponent<AudioSource>().Play(0);
        Destroy(passTurnButton);
        GameManager.gameManagerInstance.NextPlayer();
    }

    public void trapButtonPressed()
    {
        Destroy(movementMenu);
        TrapSelector trapper = board.GetComponent<TrapSelector>();
        trapper.EnterState(movingPiece);
    }

    public void instantiateDamageText(GameObject target, int damage)
    {
        if (damage > 0) { 
        GameObject instDamageText = Instantiate(damageText, target.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
        instDamageText.GetComponentInChildren<Text>().text = damage.ToString();
        }
    }

    public void enemyInformation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Enemy>() != null)
            {
                if( enemyInformationinst == null)
                {
                    enemyInformationinst = Instantiate(enemyInformationPrefab, hit.transform.position, Quaternion.Euler(new Vector3(30, 45, 0)));
                    enemyInformationinst.GetComponentInChildren<EnemyStats>().owner = hit.transform.gameObject;
                }
            }
            else
            {
                Destroy(enemyInformationinst);
                enemyInformationinst = null;
            }
        }
        else
        {
            Destroy(enemyInformationinst);
            enemyInformationinst = null;
        }

    }
    public void GameOver()
    {
        Instantiate(gameOverScreen, new Vector3 (0,0,0) , Quaternion.Euler(new Vector3(30, 45, 0)));
    }

    public void goToTileSelectorGambiarrator()
    {
        TileSelector tileSelector = board.GetComponent<TileSelector>();
        tileSelector.EnterState();
    }

}

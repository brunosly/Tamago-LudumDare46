using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSelector : MonoBehaviour
{
    public static HealSelector healSelectorInstance;

    public GameObject healLocationPrefab;
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;
    private GameObject heallingPiece;
    private List<Vector2Int> heallingLocations;
    private List<GameObject> locationHighlights;

    void Awake()
    {
        healSelectorInstance = this;
    }

    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, new Vector3(Geometry.PointFromGrid(new Vector2Int(0, 0)).x, 0.1f, Geometry.PointFromGrid(new Vector2Int(0, 0)).z), Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = new Vector3(Geometry.PointFromGrid(gridPoint).x, 0.01f, Geometry.PointFromGrid(gridPoint).z);

            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gameManagerInstance.PieceAtGrid(gridPoint) == heallingPiece)
                {
                    CancelState();
                    return;
                }
                if (!heallingLocations.Contains(gridPoint))
                {
                    return;
                }
                if (GameManager.gameManagerInstance.EnemyPieceAt(gridPoint) == true || GameManager.gameManagerInstance.FriendlyPieceAt(gridPoint))
                {
                    GameManager.gameManagerInstance.Heal(heallingPiece, gridPoint);
                }
                else
                {
                    return;
                }
                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelState();
            return;
        }
    }


    public void EnterState(GameObject piece)
    {
        heallingPiece = piece;
        this.enabled = true;
        heallingLocations = GameManager.gameManagerInstance.HealForPiece(heallingPiece);
        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in heallingLocations)
        {
            GameObject highlight;
            highlight = Instantiate(healLocationPrefab, new Vector3(Geometry.PointFromGrid(loc).x, 0.05f, Geometry.PointFromGrid(loc).z), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        heallingPiece.GetComponent<Piece>().actionPoints--;
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(heallingPiece);
        heallingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }

    private void CancelState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(heallingPiece);
        heallingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}

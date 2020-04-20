using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public static MoveSelector moveSelectorInstance;

    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    void Awake()
    {
        moveSelectorInstance = this;
    }

    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, new Vector3(Geometry.PointFromGrid(new Vector2Int(0, 0)).x, 0.65f, Geometry.PointFromGrid(new Vector2Int(0, 0)).z), Quaternion.identity, gameObject.transform);
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
            tileHighlight.transform.position = new Vector3(Geometry.PointFromGrid(gridPoint).x, 0.2f, Geometry.PointFromGrid(gridPoint).z);

            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gameManagerInstance.PieceAtGrid(gridPoint) == movingPiece)
                {
                    CancelState();
                    return;
                }
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }
                if (GameManager.gameManagerInstance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.gameManagerInstance.Move(movingPiece, gridPoint);
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
        movingPiece = piece;
        this.enabled = true;
        moveLocations = GameManager.gameManagerInstance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            highlight = Instantiate(moveLocationPrefab, new Vector3(Geometry.PointFromGrid(loc).x, 0.05f, Geometry.PointFromGrid(loc).z), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        movingPiece.GetComponent<Piece>().actionPoints--;
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(movingPiece);
        movingPiece = null;
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
        GameManager.gameManagerInstance.DeselectPiece(movingPiece);
        movingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }

}

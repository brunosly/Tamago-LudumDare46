using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject rounds;

    private void Start()
    {
        rounds.GetComponent<Text>().text = "Game Over \n \n Press to Restart \n \n You survived " + GameManager.gameManagerInstance.roundCounter.ToString() + " rounds!";
    }

    void Update()
    {
        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene("Gameplay");
        }
        Destroy(GameObject.Find("Original Canvas"));
    }
}

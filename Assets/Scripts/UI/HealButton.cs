using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(UIManager.uiManagerInstance.healButtonPressed);
    }
}
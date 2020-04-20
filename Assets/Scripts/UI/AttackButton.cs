using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(UIManager.uiManagerInstance.attackButtonPressed);
    }
}

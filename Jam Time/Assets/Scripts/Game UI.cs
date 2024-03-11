using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public TextMeshProUGUI ItemPromptText;
    public Image healthBar;

    public GameObject dashIcon;
    public GameObject glideIcon;
    public GameObject wallStickIcon;

    public float barMax;


    void Awake() { instance = this; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PromptItemPlacement(bool itemAlreadyPlaced)
    {
        // display text
        ItemPromptText.gameObject.SetActive(true);

        // update text
        if (itemAlreadyPlaced)
            ItemPromptText.text = "Pickup Ability: E";
        else
            ItemPromptText.text = "Place Ability: E";
    }

    public void RemoveItemPrompt()
    {
        // disable text
        ItemPromptText.gameObject.SetActive(false);
    }


    public void updateHealthBar(int value)
    {
        healthBar.fillAmount = (float)value / barMax;
    }

    public void UpdateProgressionUI(AlterStateManager alter, bool toggle)
    {

        if (alter.type == AlterStateManager.AlterType.dash)
            dashIcon.SetActive(toggle);
        else if (alter.type == AlterStateManager.AlterType.wallStick)
            wallStickIcon.SetActive(toggle);
        else if (alter.type == AlterStateManager.AlterType.glide)
            glideIcon.SetActive(toggle);
    }
}

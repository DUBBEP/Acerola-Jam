using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public TextMeshProUGUI itemPromptText;
    public Image healthBar;
    public Image fadeOutScreen;
    public GameObject endGamePrompt;
    

    public GameObject dashIcon;
    public GameObject glideIcon;
    public GameObject wallStickIcon;

    public float barMax = 100f;

    void Awake() { instance = this; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UIPrompt(string message)
    {
        // display text
        itemPromptText.gameObject.SetActive(true);

        // update text
        itemPromptText.text = message;
    }

    public void RemovePrompt()
    {
        // disable text
        itemPromptText.gameObject.SetActive(false);
    }


    public void updateHealthBar(int value)
    {
        Debug.Log("updating HealthBar");
        healthBar.fillAmount = (float)value / barMax;
    }

    public void UpdateProgressionUI(AlterStateManager alter, bool toggle)
    {

        if (alter.type == AlterStateManager.AlterType.pivotDash)
            dashIcon.SetActive(toggle);
        else if (alter.type == AlterStateManager.AlterType.wallStick)
            wallStickIcon.SetActive(toggle);
        else if (alter.type == AlterStateManager.AlterType.glide)
            glideIcon.SetActive(toggle);
    }

}

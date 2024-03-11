using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Info")]
    public int maxHp;
    public int curHp;
    public bool dead;

    [Header("Components")]
    public PlayerStateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Heal(int health)
    {
        // update health
        curHp += health;

        // update healthbar UI
        GameUI.instance.updateHealthBar(curHp);

    }

    public void TakeDamage(int damage)
    {
        //update health
        curHp -= damage;

        // update healthbar UI
        GameUI.instance.updateHealthBar(curHp);

        // switch playerstate to knockback state
        stateManager.SwitchState(stateManager.knockBackState);

        if (curHp < 0)
        {
            Die();
        }
    }

    void Die()
    {
        curHp = 0;

        // take control away from player

        // remove player

        // initiate respawn sequence

    }


}

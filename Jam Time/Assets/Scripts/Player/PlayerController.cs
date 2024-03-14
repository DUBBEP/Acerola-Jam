using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [Header("Info")]
    public int maxHp;
    public int curHp;
    public bool dead = false;
    private bool respawningPlayer = false;
    private bool invincible = false;
    private float invincibleTime = 0;


    [Header("Components")]
    Rigidbody2D rb;
    PlayerStateManager stateManager;
    TrailRenderer trail;


    [Header("Parameters")]
    public float lowerVelBound;
    public float upperVelBound;
    public float baseOrthoSize;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = GetComponent<PlayerStateManager>();
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleTime >= 0)
        {
            invincible = true;
            invincibleTime -= Time.deltaTime;
        }

        else if (invincibleTime < 0)
            invincible = false;
        
        if (Mathf.Abs(rb.velocity.x) > 20 || Mathf.Abs(rb.velocity.y) > 40)
            SmoothTrailTime(1);
        else
            SmoothTrailTime(0);
    }



    public void Heal(int health)
    {
        // update health
        curHp += health;

        if (curHp > maxHp)
            curHp = maxHp;

        // update healthbar UI
        GameUI.instance.updateHealthBar(curHp);

    }

    public void TakeDamage(int damage, float recoveryTime)
    {
        if (invincible)
            damage = 0;
        else 
            invincibleTime = recoveryTime;


        //update health
        curHp -= damage;

        // update healthbar UI
        GameUI.instance.updateHealthBar(curHp);

        // switch playerstate to knockback state
        stateManager.SwitchState(stateManager.damageState);

        if (curHp <= 0)
            Die();
    }

    void Die()
    {
        // set health to zero
        curHp = 0;

        // take control away from player
        dead = true;



        // initiate respawn sequence
        if (!respawningPlayer)
        {
            StartCoroutine(RespawnPlayer(maxHp, GameManager.instance.spawnPoint, 3f));
            respawningPlayer = true;
        }
    }

    public IEnumerator RespawnPlayer(int health, Transform respawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameUI.instance.fadeOutScreen.gameObject.SetActive(true);
        transform.position = respawnPoint.position;
        curHp = health;
        GameUI.instance.updateHealthBar(health);
        dead = false;
        yield return new WaitForSeconds(delay / 2f);
        GameUI.instance.fadeOutScreen.gameObject.SetActive(false);
        respawningPlayer = false;

    }

    void SmoothTrailTime(float target)
    {
        bool increase;

        if (trail.time < target)
            increase = true;
        else
            increase = false;

        if (increase && trail.time < target)
            trail.time += Time.deltaTime * 2;
        else if (!increase && trail.time > target)
            trail.time -= Time.deltaTime * 2;
    }

}

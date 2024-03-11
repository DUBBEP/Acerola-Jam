using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [Header("Info")]
    public int maxHp;
    public int curHp;
    public bool dead;
    private bool respawningPlayer;

    [Header("Components")]
    Rigidbody2D rb;
    PlayerStateManager stateManager;
    TrailRenderer trail;

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

    public void TakeDamage(int damage)
    {
        //update health
        curHp -= damage;

        // update healthbar UI
        GameUI.instance.updateHealthBar(curHp);

        // switch playerstate to knockback state
        stateManager.SwitchState(stateManager.knockBackState);

        if (curHp < 0)
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
            StartCoroutine(RespawnPlayer(maxHp, 3f));
            respawningPlayer = true;
        }

    }

    IEnumerator RespawnPlayer(int health, float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = GameManager.instance.spawnPoint.position;
        curHp = health;
        dead = false;
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

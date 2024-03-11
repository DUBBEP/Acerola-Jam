using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public enum Obstacle
    {
        spike,
        fallingSpike,
        acid,
        pillarTrap
    }

    public Obstacle type;
    public int damageValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (type == Obstacle.spike)
                player.TakeDamage(damageValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (type == Obstacle.acid)
                player.TakeDamage(damageValue);
            else if (type == Obstacle.fallingSpike)
                player.TakeDamage(damageValue); 
        }
    }
}

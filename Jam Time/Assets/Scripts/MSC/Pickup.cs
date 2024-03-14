using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D col;

    public enum PickupType
    {
        heal
    }

    public PickupType type;
    public int value;
    bool pickupConsumed;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (GameManager.instance.playerIsDead && pickupConsumed)
            StartCoroutine(RespawnPickup());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (type == PickupType.heal)
                player.Heal(value);

            sr.enabled = false;
            col.enabled = false;
            pickupConsumed = true;
        }
    }

    IEnumerator RespawnPickup()
    {
        pickupConsumed = false;
        yield return new WaitForSeconds(3f);
        Debug.Log("Respawning Pickup");
        sr.enabled = true;
        col.enabled = true;
    }
}

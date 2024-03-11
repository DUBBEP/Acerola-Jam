using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    public float spikeFallSpeed = 3f;


    private Rigidbody2D spike;
    private bool spikeFalling;


    // Start is called before the first frame update
    void Start()
    {
        spike = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spikeFalling)
            spike.gravityScale = spikeFallSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            spikeFalling = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    public float spikeFallSpeed = 3f;


    private Rigidbody2D spike;
    private Transform spikeTransform;
    private bool spikeFalling;


    // Start is called before the first frame update
    void Start()
    {
        spike = GetComponentInChildren<Rigidbody2D>();
        spikeTransform = GetComponentInChildren<Obstacles>().transform;
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
        {
            spikeFalling = true;
            StartCoroutine(RecallSpike(120f));
        }

    }


    IEnumerator RecallSpike(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Recalling Spike");
        spikeFalling = false;
        spike.gravityScale = 0f;
        spikeTransform.position = this.transform.position;
        spike.velocity = Vector2.zero;
    }
}

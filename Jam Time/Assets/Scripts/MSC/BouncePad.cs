using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public enum PadType
    {
        strong,
        smooth
    }

    public PadType type;
    public float bounceForce;
    public float pauseControlTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            PlayerStateManager psm = collision.GetComponent<PlayerStateManager>();

            if (type == PadType.strong)
            {
                rb.velocity = Vector2.zero;
                psm.pauseControlTime = pauseControlTime;
            }
            else if (type == PadType.smooth)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            Debug.Log("adding force");

            rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
            psm.SwitchState(psm.fallState);
        }
    }
}

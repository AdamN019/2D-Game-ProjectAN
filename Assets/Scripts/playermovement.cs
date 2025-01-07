using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float speed = 5.0f;
    public float jumpForce = 8.0f;
    public float airControlForce = 10.0f;
    public float airControlMax = 1.5f;
    Vector2 boxExtents;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxExtents = GetComponent<BoxCollider2D>().bounds.extents;

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0.0f)
        {
            
            rigidbody.linearVelocity = new Vector2(h * speed, 0.0f);
        }

        //check if we are on the ground
       
        Vector2 bottom = new Vector2(transform.position.x, transform.position.y - boxExtents.y);

        Vector2 hitBoxSize = new Vector2(boxExtents.x * 2.0f, 0.05f);

        RaycastHit2D result = Physics2D.BoxCast(bottom, hitBoxSize, 0.0f, new Vector3(0.0f, -1.0f), 0.0f, 1 << LayerMask.NameToLayer("ground"));

        bool grounded = result.collider != null && result.normal.y > 0.9f;

        if (grounded)
        {
            if (Input.GetAxis("Jump") > 0.0f)
            {
                rigidbody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                rigidbody.linearVelocity = new Vector2(speed * h, rigidbody.linearVelocity.y);

            }
        }

        else
        {
            //move slightly in the air
            float vx = rigidbody.linearVelocity.x;
            if (h * vx < airControlMax)
            {
                rigidbody.AddForce(new Vector2(h * airControlForce, 0));
            }
        }
        }
    }


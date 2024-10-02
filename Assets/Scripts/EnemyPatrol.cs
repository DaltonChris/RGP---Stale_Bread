using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Dalton Christopher
// Ethereal Escape Enemy Patrol Base
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]    Transform GroundPos;
    [SerializeField]    Transform EyesPos;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField]    Rigidbody2D _rb;

    const string LEFT = "left";
    const string RIGHT = "right";

    float baseCastDist = 0.1f;
    string facingDirection;
    Vector3 baseScale;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        facingDirection = RIGHT;
        baseScale = transform.localScale;
    }

    void Update()
    {
        
        
    }


    private void FixedUpdate()
    {
        float vX = moveSpeed;
        if(facingDirection == LEFT)
        {
            vX = -moveSpeed;
        }
        _rb.velocity = new Vector2(vX, _rb.velocity.y);

        if (IsHittingWall() || IsNearEdge() || IsHittingEnemy() || IsHittingEnemyLow())
        {
            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
        }
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;
        if (newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == RIGHT)
        {
            newScale.x = baseScale.x;
        }
        transform.localScale = newScale;
        facingDirection = newDirection;
    }


    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        if(facingDirection == RIGHT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }
        Vector3 targetPos = GroundPos.position;
        targetPos.x -= castDist;

        Debug.DrawLine(GroundPos.position, targetPos, Color.green);
        
        if(Physics2D.Linecast(GroundPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }
    bool IsNearEdge()
    {
        bool val = true;
        float castDist = baseCastDist;
 
        Vector3 targetPos = GroundPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(GroundPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(GroundPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }

    bool IsHittingEnemy()
    {
        bool val = true;
        float castDist = baseCastDist * 2;
        if (facingDirection == RIGHT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }
        Vector3 targetPos = EyesPos.position;
        targetPos.x -= castDist;

        Debug.DrawLine(EyesPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(EyesPos.position, targetPos, 1 << LayerMask.NameToLayer("Enemy")))
        {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }

    bool IsHittingEnemyLow()
    {
        bool val = false;

        // Not needed 
        
        return val;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(34);
        }
    }
}

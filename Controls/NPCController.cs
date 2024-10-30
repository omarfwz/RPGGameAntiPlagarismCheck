using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    Rigidbody2D rBody;
    Animator anim;
    public float speed = 1F;
    public const float speedC = 1F;
    private Vector2 movement;
    private Vector2 tPoint;
    private bool gotToPoint;
    private bool moveActive;

    private void Awake()
    {
        rBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }
    public void SetMovement(Vector2 move)
    {
        movement = move;
    }

    public void SetDirection(int i)
    {
        anim.SetFloat("Direction", i);
    }

    public void PlayAnimation(string s)
    {
        //
    }

    public IEnumerator MoveToPoint(Vector2 point)
    {
        tPoint = point;
        gotToPoint = false;
        if(point.y > rBody.position.y)
        {
            SetMovement(new Vector2(0, 1));
            while (!gotToPoint)
            {
                yield return new WaitForSeconds(0.2F);
                if (rBody.position == point || point.y < rBody.position.y)
                {
                    gotToPoint = true;
                    SetMovement(new Vector2(0, 0));
                }
            }
        } 
        else if (point.y < rBody.position.y)
        {
            SetMovement(new Vector2(0, -1));
            while (!gotToPoint)
            {
                yield return new WaitForSeconds(0.2F);
                if (rBody.position == point || point.y > rBody.position.y)
                {
                    gotToPoint = true;
                    SetMovement(new Vector2(0, 0));
                }
            }
        }
        else if (point.x > rBody.position.x)
        {
            SetMovement(new Vector2(1, 0));
            while (!gotToPoint)
            {
                yield return new WaitForSeconds(0.2F);
                if (rBody.position == point || point.x < rBody.position.x)
                {
                    gotToPoint = true;
                    SetMovement(new Vector2(0, 0));
                }
            }
        }
        else if (point.x < rBody.position.x)
        {
            SetMovement(new Vector2(-1, 0));
            while (!gotToPoint)
            {
                yield return new WaitForSeconds(0.2F);
                if (rBody.position == point || point.x > rBody.position.x)
                {
                    gotToPoint = true;
                    SetMovement(new Vector2(0, 0));
                }
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        rBody.MovePosition(rBody.position += movement * speed * Time.deltaTime);
        
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        if (speed == 0)
        {
            anim.SetFloat("Speed", 0);
        }
        else
        {
            anim.SetFloat("Speed", movement.sqrMagnitude);
        }


        if (movement.y == 1)
        {
            anim.SetFloat("Direction", 4);
        }
        else if (movement.y == -1)
        {
            anim.SetFloat("Direction", 1);
        }
        else if (movement.x == 1)
        {
            anim.SetFloat("Direction", 3);
        }
        else if (movement.x == -1)
        {
            anim.SetFloat("Direction", 2);
        }
    }
}

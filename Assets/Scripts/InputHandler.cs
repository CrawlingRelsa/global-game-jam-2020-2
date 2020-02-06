using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 resolution;
    public bool isTouch;
    private static Vector2 axisPad;
    private static bool actionTapped;


    private Vector2 startingPoint;
    private Vector2 currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        axisPad = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t;
                if ((t = Input.GetTouch(i)).position.x < resolution.x / 2)
                {
                    HandleLeftPad(t);
                }
                else
                {
                    HandleTap(t);
                }
            }
        }
        else
        {
            axisPad.x = Input.GetAxisRaw("Horizontal");
            axisPad.y = Input.GetAxisRaw("Vertical");
            actionTapped = Input.GetButtonDown("Fire1");
        }

    }

    public static Vector2 GetAxis()
    {
        return axisPad;
    }

    public static bool GetActionTap()
    {
        return actionTapped;
    }

    void HandleLeftPad(Touch left)
    {

        if (left.phase == TouchPhase.Began)
        {
            startingPoint = left.position;
        }
        if (left.phase == TouchPhase.Moved)
        {
            currentPoint = left.position;
        }
        if (left.phase == TouchPhase.Ended)
        {
            startingPoint = Vector2.zero;
            currentPoint = Vector2.zero;
        }
        axisPad = (currentPoint - startingPoint).normalized;
        Debug.Log(axisPad);
    }

    void HandleTap(Touch right)
    {
        Debug.Log(right.phase);
        if (right.phase == TouchPhase.Began)
        {
            actionTapped = true;
        }
        if (right.phase == TouchPhase.Ended)
        {
            actionTapped = false;
        }
    }
}

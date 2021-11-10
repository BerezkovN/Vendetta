using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillScript : MonoBehaviour
{
    private Vector3 hillDirectionVector;
    private Vector3 pointA;
    private Vector3 pointB;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Make method ChangeDirection so I don't have to call every frame, but I don't have time so govnocode for life.
            if (collision.gameObject.transform.position.x > pointA.x && collision.gameObject.transform.position.x < pointB.x)
                collision.gameObject.GetComponent<Movement>().directionVector = hillDirectionVector;
            else
                collision.gameObject.GetComponent<Movement>().directionVector = new Vector3(1, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pointA = gameObject.transform.GetChild(0).position;
        pointB = gameObject.transform.GetChild(1).position;

        hillDirectionVector = (pointB - pointA).normalized;
    }
}

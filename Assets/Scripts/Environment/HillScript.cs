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
            if (collision.gameObject.transform.position.x > pointA.x && collision.gameObject.transform.position.x < pointB.x)
                collision.gameObject.GetComponent<PlayerMovement>().directionVector = hillDirectionVector;
            else
                collision.gameObject.GetComponent<PlayerMovement>().directionVector = new Vector3(1, 0, 0);
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

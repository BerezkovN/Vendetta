using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillScript : MonoBehaviour
{
    [SerializeField] float Degrees;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Degrees), Mathf.Sin(Mathf.Deg2Rad * Degrees), 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerMovement>().directionVector = new Vector3(1, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
        {
            Debug.LogError("Set player's tag to Player");
        }

        BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();

        thisCollider.size = new Vector2(thisCollider.size.x - playerCollider.size.x, thisCollider.size.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    //I just have no clue how PlayerInfo works so I have to get a reference
    [SerializeField] public GameObject Player;

    private Movement playerMovement;
    private Vector3 ladderDirection;
    private Vector3 pointA;
    private Vector3 pointB;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = Player.GetComponent<Movement>();
        pointA = gameObject.transform.GetChild(0).position;
        pointB = gameObject.transform.GetChild(1).position;

        ladderDirection = (pointB - pointA).normalized;
    }

    protected override void OnClickInternal(PlayerInfo playerInfo)
    {
        playerMovement.isOnLadder = true;
        Player.GetComponent<Movement>().directionVector = ladderDirection;

        if (gameObject.transform.position.y > Player.transform.position.y)
            playerMovement.PutOnLadder(pointA);
        else
            playerMovement.PutOnLadder(pointB);

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerMovement.isOnLadder)
        {
            if (Player.transform.position.y < pointA.y || Player.transform.position.y > pointB.y) 
            { 
                playerMovement.isOnLadder = false;
                playerMovement.directionVector = new Vector3(1, 0, 0);
                playerMovement.GetComponent<Animator>().Play("Idle");
            }


        }
    }
}

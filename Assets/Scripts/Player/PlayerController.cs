using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private Movement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isOnLadder)
        {
            float verticalDirection = Input.GetAxisRaw(Constants.VerticallAxis);

            playerMovement.Climb(verticalDirection);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
                playerMovement.CallState(Movement.States.Sit);
            else if (Input.GetKeyUp(KeyCode.S))
                playerMovement.CallState(Movement.States.Stand);

            float horizontalDirection = Input.GetAxisRaw(Constants.HorizontalAxis);
            playerMovement.Move(horizontalDirection);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{

    protected override void OnClickInternal()
    {
        gameObject.transform.position += new Vector3(0.5f,0,0);
    }
}

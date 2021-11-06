using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public GameObject Key;
    public bool KeyPrompt = true;

    private GameObject createdKey;

    protected override void OnTriggerEnter()
    {
        createdKey = Instantiate(Key);
        createdKey.transform.position = this.gameObject.transform.position + Vector3.up;
    }

    protected override void OnTriggerExit()
    {
        GameObject.Destroy(createdKey);
    }

}

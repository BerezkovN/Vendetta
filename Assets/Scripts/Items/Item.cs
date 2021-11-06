using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : 
    Interactable
{
    public GameObject Key;
    public bool KeyPrompt = true;

    private GameObject createdKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        createdKey = Instantiate(Key);
        createdKey.transform.position = this.gameObject.transform.position + Vector3.up;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Destroy(createdKey);
    }
}
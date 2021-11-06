using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit();
    }

    protected virtual void OnTriggerEnter()
    {

    }

    protected virtual void OnTriggerExit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

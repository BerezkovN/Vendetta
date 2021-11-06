using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Key Key;
    [SerializeField] GameObject? CreatedKey;

    private bool _hidden = false;
    private SpriteRenderer _keyRenderer;
    private void Awake()
    {
        Initialize();
        CreatedKey.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_hidden)
        {
            return;
        }
        CreatedKey.SetActive(true);
        OnTriggerEnter2DInternal(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_hidden)
        {
            return;
        }
        CreatedKey.SetActive(false);
        OnTriggerExit2DInternal(collision);
    }
    
    [ContextMenu("Initialize")]
    public void Initialize()
    {
        GameObject? buf = CreatedKey;
        if (Key != null)
        {
            CreatedKey = new GameObject(Key.name);
            CreatedKey.transform.parent = gameObject.transform;
            _keyRenderer = CreatedKey.AddComponent<SpriteRenderer>();
            _keyRenderer.sprite = Key.SpriteTexture;
            if (buf != null)
            {
                CreatedKey.transform.position = buf.transform.position;
                DestroyImmediate(buf);
            }
            else
            {
                CreatedKey.transform.position = gameObject.transform.position + Vector3.up;
            }
        }
        else if (buf != null)
        {
            Destroy(buf);
        }
        
    }

    public void Click()
    {
        _keyRenderer.sprite = Key.SpriteTexturePressed;
        ClickInternal();
    }
    public void ClickExit()
    {
        _keyRenderer.sprite = Key.SpriteTexture;
        ClickExitInternal();
    }

    protected void HideKey()
    {
        _hidden = true;
        CreatedKey.SetActive(false);
    }

    protected virtual void OnTriggerEnter2DInternal(Collider2D collision) { /* intenionally unimplemented */ }
    protected virtual void OnTriggerExit2DInternal(Collider2D collision) { /* intenionally unimplemented */ }
    protected virtual void ClickInternal() { /* intenionally unimplemented */ }
    protected virtual void ClickExitInternal() { /* intenionally unimplemented */ }



}
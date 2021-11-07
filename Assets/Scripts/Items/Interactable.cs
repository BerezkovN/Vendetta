using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public Key Key;
    [SerializeField] protected GameObject? CreatedKey;

    private bool _hidden = false;
    private SpriteRenderer _keyRenderer;
    private void Awake()
    {
        Initialize();
        CreatedKey.SetActive(false);
    }
    public void TriggerEnter(Collider2D collision)
    {
        if(_hidden)
        {
            return;
        }
        _keyRenderer.sprite = Key.SpriteTexture;
        CreatedKey.SetActive(true);
        OnTriggerEnter2DInternal(collision);
    }
    
    public void TriggerExit(Collider2D collision)
    {
        if (_hidden)
        {
            return;
        }
        _keyRenderer.sprite = Key.SpriteTexture;
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
        InitializeInternal();
    }

    public bool IsHidden()
    {
        return _hidden;
    }

    public void OnClick(PlayerInfo playerInfo)
    {
        if(_hidden)
        {
            return;
        }
        _keyRenderer.sprite = Key.SpriteTexturePressed;
        OnClickInternal(playerInfo);
    }
    public void OnClickExit(PlayerInfo playerInfo)
    {
        if (_hidden)
        {
            return;
        }
        _keyRenderer.sprite = Key.SpriteTexture;
        OnClickExitInternal(playerInfo);
    }

    protected void HideInteractionKey()
    {
        _hidden = true;
        CreatedKey.SetActive(false);
    }

    protected virtual void OnTriggerEnter2DInternal(Collider2D collision) { /* intenionally unimplemented */ }
    protected virtual void OnTriggerExit2DInternal(Collider2D collision) { /* intenionally unimplemented */ }
    protected virtual void OnClickInternal(PlayerInfo playerInfo) { /* intenionally unimplemented */ }
    protected virtual void OnClickExitInternal(PlayerInfo playerInfo) { /* intenionally unimplemented */ }
    protected virtual void InitializeInternal() { /* intenionally unimplemented */ }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Scriptable Objects/Create Key", order = 1)]
public class Key : ScriptableObject
{
    [SerializeField] public string KeyName;
    [SerializeField] public Sprite SpriteTexture;
    [SerializeField] public Sprite SpriteTexturePressed;
}
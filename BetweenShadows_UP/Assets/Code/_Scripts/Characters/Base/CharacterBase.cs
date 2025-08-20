using System;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterBase : MonoBehaviour
{
    public CharacterStats stats { get; private set; }

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {
        
    }

    protected void Update()
    {
        
    }
}

using System;
using UnityEngine;

// Base Class for any character in this game

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

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void LateUpdate()
    {
    }
}

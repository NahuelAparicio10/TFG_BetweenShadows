using System;
using UnityEngine;

public class RootMotionRelay : MonoBehaviour
{
    public event Action<Vector3, Quaternion> OnRootMotion;
    
    private Animator _anim;

    void Awake() => _anim = GetComponent<Animator>();

    void OnAnimatorMove()
    {
        OnRootMotion?.Invoke(_anim.deltaPosition, _anim.deltaRotation);
    }
}

using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    protected Animator _animator;
    public Animator Animator => _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public virtual void PlayTargetAnimation(string targetAnimation, float timeToFade)
    {
        _animator.CrossFade(targetAnimation, timeToFade);
    }
}

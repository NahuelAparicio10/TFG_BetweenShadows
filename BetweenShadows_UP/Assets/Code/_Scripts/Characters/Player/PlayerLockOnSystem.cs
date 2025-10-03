using UnityEngine;

public class PlayerLockOnSystem : MonoBehaviour
{
    private bool _isLockedOn = false;
    public bool IsLockedOn => _isLockedOn;

    private PlayerContext _ctx;

    public void Initialize(PlayerContext ctx)
    {
        _ctx = ctx;
    }

    public void SetIsLocked(bool isLocked)
    {
        
        _isLockedOn = isLocked;
        
        _ctx.Animation.SetIsLocked(_isLockedOn);
    }
}

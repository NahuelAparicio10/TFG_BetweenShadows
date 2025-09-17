using System;
using UnityEngine;

public class PlayerMovement
{
    private Rigidbody _rb;
    private Transform _transform;

    public void GetReferences(GameObject player)
    {
        _rb = player.GetComponent<Rigidbody>();
        _transform = player.transform;
    }
    
    public void HandleAllMovement()
    {
        
    }

    private void ApplyRotation(Vector3 dir, float speed)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir);
        _rb.MoveRotation(Quaternion.Slerp(_transform.rotation, targetRot, speed * Time.deltaTime));
    }

    #region  RootMotion

    

    #endregion

}

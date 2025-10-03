using UnityEngine;

public class TestObectInteractuable : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        Debug.Log("Interacting miau");
    }
}

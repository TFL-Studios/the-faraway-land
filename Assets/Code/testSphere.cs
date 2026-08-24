using UnityEngine;

public class testSphere : MonoBehaviour, IInteractable
{
    public bool Interact()
    {
        Debug.Log(this.name);
        return true;
    }
}

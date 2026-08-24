using UnityEngine;

public class testCube : MonoBehaviour, IInteractable
{
    public bool Interact()
    {
        Debug.Log(this.name);
        return true;
    }
}

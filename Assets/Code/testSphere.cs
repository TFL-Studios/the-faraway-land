using UnityEngine;

public class testSphere : Interactable
{
    public override bool Interact()
    {
        Debug.Log(this.name);
        return true;
    }
}

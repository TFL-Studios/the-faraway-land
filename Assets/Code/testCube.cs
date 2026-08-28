using UnityEngine;

public class testCube : Interactable
{
    public override bool Interact()
    {
        Debug.Log(this.name);
        return true;
    }
}

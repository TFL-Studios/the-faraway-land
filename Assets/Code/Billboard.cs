using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void OnEnable()
    {
        GameObject.FindAnyObjectByType<BillboardController>().RegisterBillboard(this); // TODO: change to gamemanager
    }

    private void OnDisable()
    {
        GameObject.FindAnyObjectByType<BillboardController>().UnregisterBillboard(this); // TODO: change to gamemanager
    }
}

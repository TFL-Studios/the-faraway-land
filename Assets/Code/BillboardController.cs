using System.Collections.Generic;
using UnityEngine;

public class BillboardController : MonoBehaviour
{
    private List<Billboard> _billboards = new List<Billboard>();

    private void Update()
    {
        for (int index = 0; index < this._billboards.Count; index++)
        {
            Transform billboard = this._billboards[index].transform;
            billboard.LookAt((2 * billboard.position) - this.transform.position);
            Vector3 eulerAngles = this._billboards[index].transform.rotation.eulerAngles;
            this._billboards[index].transform.rotation = Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z);
        }
    }

    public bool RegisterBillboard(Billboard billboard)
    {
        if (this._billboards.Contains(billboard))
        {
            return false;
        }

        this._billboards.Add(billboard);
        return true;
    }

    public bool UnregisterBillboard(Billboard billboard)
    {
        if (!this._billboards.Contains(billboard))
        {
            return false;
        }

        this._billboards.Remove(billboard);
        return true;
    }
}

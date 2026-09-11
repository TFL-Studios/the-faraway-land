using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Items> _itens;
    [SerializeField] private Transform _icon;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Items _item;
    
    private void Update()
    {
        if (InputHandler.Instance.CrouchInput.WasPressed)
        {
            this.AddItem(_item);
            Debug.Log("Item adicionado");
        }
        
        if (InputHandler.Instance.SprintInput.WasPressed)
        {
            this.RemoveItem(_item);
            Debug.Log("Item removido");
        }
    }

    public void AddItem(Items item)
    {
        this._itens.Add(item);
        this.UpdateList();
    }

    public void RemoveItem(Items item)
    {
        this._itens.Remove(item);
        this.UpdateList();
    }

    public void UpdateList()
    {
        for (int i = this._icon.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(this._icon.GetChild(i).gameObject);
        }

        for (int i = 0; i < this._itens.Count; i++)
        {
            Vector3 itemGot = new Vector3(this._icon.transform.position.x + 100f * i, this._icon.transform.position.y, this._icon.transform.position.z);
            GameObject.Instantiate(this._itemPrefab, itemGot, Quaternion.identity, this._icon).GetComponent<Image>().sprite = this._itens[i].itemIcon;
        }
    }
}

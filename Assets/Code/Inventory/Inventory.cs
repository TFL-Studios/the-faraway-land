using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Itens> itens;
    public Transform icon;
    public GameObject itemPrefab;
    public Itens item;
    void Start()
    {
        
    }

    
    void Update()
    {
       if( InputHandler.Instance.EntradaAgachamento.FoiPressionada)
        {
            AddItem(item);
            Debug.Log("Item adicionado");

        }

       if(InputHandler.Instance.EntradaCorrida.FoiPressionada)
        {
            RemoveItem(item);
            Debug.Log("Item removido");
        }

    }


    public void AddItem(Itens item)
    {
        itens.Add(item);
        UpdateList();


    }

    public void RemoveItem(Itens item)
    {
        itens.Remove(item);
        UpdateList();
    }

    public void UpdateList()
    {

        for(int i = icon.childCount - 1; i  >= 0; i--)
        {
            Destroy(icon.GetChild(i).gameObject);
        }

        for (int i = 0; i < itens.Count; i++)
        {
            Vector3 itemGot;

            itemGot = new Vector3(icon.transform.position.x + 100f * i, icon.transform.position.y, icon.transform.position.z);
            Instantiate(itemPrefab, itemGot, Quaternion.identity, icon).GetComponent<Image>().sprite = itens[i].itemIcon;
            
        }




    }
}

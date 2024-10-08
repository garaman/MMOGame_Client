using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    public List<UI_Inventory_Item> Items { get; } = new List<UI_Inventory_Item>();
    int _slotMaxCount = 28;
    public override void Init()
    {
        Items.Clear();

        GameObject grid = transform.Find("ItemGrid").gameObject;
        foreach( Transform child in grid.transform )
        {
            Destroy( child.gameObject );
        }

        for(int i = 0; i < _slotMaxCount; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Inventory_Item",grid.transform);
            UI_Inventory_Item item = go.GetOrAddComponent<UI_Inventory_Item>();
            Items.Add( item );
        }

        RefreshUI();
    }


    public void RefreshUI()
    {
        if (Items.Count == 0) { return; }

        for (int i = 0; i < _slotMaxCount; i++)
        {
            Items[i].SetItem(null);
        }

        List<Item> items = Managers.Inven.Items.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        foreach(Item item in items) 
        {
            if(item.Slot < 0 || item.Slot >= _slotMaxCount) { continue; }
            
            Items[item.Slot].SetItem(item);            
        }
    }

}

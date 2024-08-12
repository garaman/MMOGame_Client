using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public List<UI_Shop_Inventory_Item> Items { get; } = new List<UI_Shop_Inventory_Item>();
    public List<UI_Shop_Item> ShopItems { get; } = new List<UI_Shop_Item>();
    int _slotMaxCount = 28;
    int _shopslotMaxCount = 20;
    public override void Init()
    {
        Items.Clear();
        ShopItems.Clear();

        GameObject grid = transform.Find("InventoryItemGrid").gameObject;
        GameObject shopGrid = transform.Find("ShopItemGrid").gameObject;

        foreach( Transform child in grid.transform )
        {
            Destroy( child.gameObject );
        }
        foreach (Transform child in shopGrid.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _slotMaxCount; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Shop_Inventory_Item",grid.transform);
            UI_Shop_Inventory_Item item = go.GetOrAddComponent<UI_Shop_Inventory_Item>();
            Items.Add( item );
        }

        for (int i = 0; i < _shopslotMaxCount; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Shop_Item", shopGrid.transform);
            UI_Shop_Item item = go.GetOrAddComponent<UI_Shop_Item>();
            ShopItems.Add(item);
        }

        if (ShopItems.Count == 0) { return; }

        List<ItemData> shopItem = Managers.Data.ItemDict.Values.ToList();

        int shopSlot = 0;
        foreach (ItemData item in shopItem)
        {
            if (item.itemType == ItemType.Consumable) { return; }
            ShopItems[shopSlot].SetItem(item);
            shopSlot++;
        }


        RefreshUI();
    }


    public void RefreshUI()
    {
        if (Items.Count == 0) { return; }

        List<Item> items = Managers.Inven.Items.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        for (int i = 0; i < _slotMaxCount; i++)
        {
            Items[i].SetItem(null);
        }

        foreach (Item item in items)
        {
            if (item.Slot < 0 || item.Slot >= _slotMaxCount) { continue; }

            Items[item.Slot].SetItem(item);
        }

    }

}

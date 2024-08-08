using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Inventory_Item : UI_Base
{
    [SerializeField]
    Image _icon;
    [SerializeField]
    Image _equipFrame;

    public int ItemDbId { get; private set; }
    public int TemplateId { get; private set; }
    public int Count { get; private set; }
    public bool Equipped { get; private set; }

    public override void Init()
    {
        _icon.gameObject.BindEvent((e) =>
        {
            if (Equipped == true) { Debug.Log("착용중인 장비는 판매가 불가합니다."); return; }

            C_SellItem sellPacket = new C_SellItem();
            sellPacket.ItemDbId = ItemDbId;            

            Managers.Network.Send(sellPacket);
        }, Define.UIEvent.DoubleClick);
    }

    public void SetItem(Item item)
    {
        if (item == null)
        {
            ItemDbId = 0;
            TemplateId = 0;
            Count = 0;
            Equipped = false;
            _icon.gameObject.SetActive(false);
            _equipFrame.gameObject.SetActive(false);
        }
        else
        {
            ItemDbId = item.ItemDbId;
            TemplateId = item.TemplateId;
            Count = item.Count;
            Equipped = item.Equipped;

            Data.ItemData itemData = null;
            Managers.Data.ItemDict.TryGetValue(TemplateId, out itemData);

            Sprite icon = Managers.Resource.Load<Sprite>(itemData.iconPath);
            _icon.sprite = icon;

            _icon.gameObject.SetActive(true);
            _equipFrame.gameObject.SetActive(Equipped);
        } 
    }
}

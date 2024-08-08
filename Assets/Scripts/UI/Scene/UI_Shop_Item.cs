using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Item : UI_Base
{
    [SerializeField]
    Image _icon;
    [SerializeField]
    Image _equipFrame;

    public int TemplateId { get; private set; }

    public override void Init()
    {
        _icon.gameObject.BindEvent((e) =>
        {
            C_BuyItem buyPacket = new C_BuyItem();
            buyPacket.TemplateId = TemplateId;

            Managers.Network.Send(buyPacket);
        }, Define.UIEvent.DoubleClick);
    }

    public void SetItem(ItemData itemData)
    {
        if (itemData == null)
        {            
            TemplateId = 0;                    
            _icon.gameObject.SetActive(false);
            _equipFrame.gameObject.SetActive(false);
        }
        else
        {         
            TemplateId = itemData.id;            

            Sprite icon = Managers.Resource.Load<Sprite>(itemData.iconPath);
            _icon.sprite = icon;

            _icon.gameObject.SetActive(true);            
        } 
    }
}

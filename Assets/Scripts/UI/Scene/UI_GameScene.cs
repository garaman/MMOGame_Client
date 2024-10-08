using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_Stat StatUI { get; private set; }
    public UI_Inventory InventoryUI { get; private set; }
    public UI_Shop ShopUI { get; private set; }

    public override void Init()
    {
        base.Init();

        StatUI = GetComponentInChildren<UI_Stat>();
        InventoryUI = GetComponentInChildren<UI_Inventory>();
        ShopUI = GetComponentInChildren<UI_Shop>();

        StatUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);     
        ShopUI.gameObject.SetActive(false);
    }
}

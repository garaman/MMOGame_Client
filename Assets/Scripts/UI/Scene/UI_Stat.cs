using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : UI_Base
{
    enum Images
    {
        Slot_Helmet,
        Slot_Armor,
        Slot_Boots,
        Slot_Weapon,
        Slot_Shield
    }

    enum Texts
    {
        NameText,
        Attack,
        Defence
    }

    bool _init = false;
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(Texts));

        _init = true;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_init == false) { return; }

        Image helmetImg = Get<Image>((int)Images.Slot_Helmet).GetComponentsInChildren<Image>()[1];
        Image armorImg = Get<Image>((int)Images.Slot_Armor).GetComponentsInChildren<Image>()[1];
        Image bootsImg = Get<Image>((int)Images.Slot_Boots).GetComponentsInChildren<Image>()[1];
        Image weaponImg = Get<Image>((int)Images.Slot_Weapon).GetComponentsInChildren<Image>()[1];
        Image shieldImg = Get<Image>((int)Images.Slot_Shield).GetComponentsInChildren<Image>()[1];

        helmetImg.enabled = false;
        armorImg.enabled = false;
        bootsImg.enabled = false;
        weaponImg.enabled = false;
        shieldImg.enabled = false;

        foreach (Item item in Managers.Inven.Items.Values)
        {
            if (item.Equipped == false) { continue; }

            ItemData itemData = null;
            Managers.Data.ItemDict.TryGetValue(item.TemplateId, out itemData);
            Sprite icon = Managers.Resource.Load<Sprite>(itemData.iconPath);

            if (item.ItemType == ItemType.Weapon)
            {
                weaponImg.enabled = true;
                weaponImg.sprite = icon;
            }
            else if (item.ItemType == ItemType.Armor)
            {
                Armor armor = (Armor)item;
                switch (armor.ArmorType)
                {
                    case ArmorType.Helmet:
                        helmetImg.enabled = true;
                        helmetImg.sprite = icon;
                        break;
                    case ArmorType.Armor:
                        armorImg.enabled = true;
                        armorImg.sprite = icon;
                        break;
                    case ArmorType.Boots:
                        bootsImg.enabled = true;
                        bootsImg.sprite = icon;
                        break;
                }
            }
        }


        MyPlayerController player = Managers.Object.MyPlayer;
        player.RefreshAddionalStat();

        Get<GameObject>((int)Texts.NameText).GetComponentInChildren<Text>().text = player.name;
        int totalDamage = player.Stat.Attack + player.WeaponDamage;
        Get<GameObject>((int)Texts.Attack).GetComponentInChildren<Text>().text = $"{totalDamage}(+{player.WeaponDamage})";
        Get<GameObject>((int)Texts.Defence).GetComponentInChildren<Text>().text = $"{player.ArmorDefence}";

    }
}

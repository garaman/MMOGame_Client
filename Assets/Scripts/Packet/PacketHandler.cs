using Google.Protobuf.Protocol;
using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;

        Managers.Object.Add(enterGamePacket.Player, myPlayer: true);        
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;        

        Managers.Object.Clear();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.Object.Add(obj, myPlayer: false);
        }
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.FindbyId(movePacket.ObjectId);
        if(go == null) { return; }
        
        if(Managers.Object.MyPlayer.Id == movePacket.ObjectId) { return; } 

        BaseController bc = go.GetComponent<BaseController>();
        if(bc == null) { return; }

        bc.PosInfo = movePacket.PosInfo;
    }

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;
        
        GameObject go = Managers.Object.FindbyId(skillPacket.ObjectId);
        if (go == null) { return; }

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null) 
        {
            cc.UseSkill(skillPacket.Info.SkillId);
        }
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changePacket = packet as S_ChangeHp;

        GameObject go = Managers.Object.FindbyId(changePacket.ObjectId);
        if (go == null) { return; }

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null)
        {
            cc.Hp = changePacket.Hp;
        }

    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        GameObject go = Managers.Object.FindbyId(diePacket.ObjectId);
        if (go == null) { return; }

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null)
        {
            cc.Hp = 0;
            cc.OnDead();
        }
    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {        
        Debug.Log("S_ConnectedHandler");
        C_Login loginPacket = new C_Login();

        //Debug.Log(SystemInfo.deviceUniqueIdentifier);
        string path = Application.dataPath;
        loginPacket.UniqueId = path.GetHashCode().ToString();
        Managers.Network.Send(loginPacket);   
    }

    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = packet as S_Login;
        Debug.Log($"Login : {loginPacket.LoginOk}");

        // TODO : 로비 UI, 캐릭터 선택
        if (loginPacket.Players == null || loginPacket.Players.Count == 0)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{UnityEngine.Random.Range(0,10000).ToString("00000")}";
            Managers.Network.Send(createPacket);
        }
        else
        {
            // 무조건 첫번째 로그인으로.
            LobbyPlayerInfo info = loginPacket.Players[0];
            C_EnterGame enterGamePacket = new C_EnterGame();
            enterGamePacket.Name = info.Name;
            Managers.Network.Send(enterGamePacket);
        }
    }

    public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
    {
        S_CreatePlayer createOkPacket = packet as S_CreatePlayer;

        if (createOkPacket.Player == null)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{UnityEngine.Random.Range(0, 10000).ToString("00000")}";
            Managers.Network.Send(createPacket);
        }
        else
        {            
            C_EnterGame enterGamePacket = new C_EnterGame();
            enterGamePacket.Name = createOkPacket.Player.Name;
            Managers.Network.Send(enterGamePacket);
        }

    }

    public static void S_ItemListHandler(PacketSession session, IMessage packet)
    {
        S_ItemList itemListPacket = packet as S_ItemList;

        Managers.Inven.Clear();

        foreach (ItemInfo itemInfo in itemListPacket.Items)
        {
            Item item = Item.MakeItem(itemInfo);
            Managers.Inven.Add(item);
        }

        if (Managers.Object.MyPlayer != null)
        {
            Managers.Object.MyPlayer.RefreshAddionalStat();
        }
    }

    public static void S_AddItemHandler(PacketSession session, IMessage packet)
    {
        S_AddItem itemListPacket = packet as S_AddItem;
        
        foreach (ItemInfo itemInfo in itemListPacket.Items)
        {
            Item item = Item.MakeItem(itemInfo);
            Managers.Inven.Add(item);
        }

        Debug.Log("아이템을 획득했습니다.");

        if (Managers.Object.MyPlayer != null)
        {
            Managers.Object.MyPlayer.RefreshAddionalStat();
        }

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        gameSceneUI.InventoryUI.RefreshUI();
        gameSceneUI.StatUI.RefreshUI();
    }

    public static void S_EquipItemHandler(PacketSession session, IMessage packet)
    {
        S_EquipItem equipItemPacket = packet as S_EquipItem;

        Item item = Managers.Inven.Get(equipItemPacket.ItemDbId);
        if(item == null) { return; }

        item.Equipped = equipItemPacket.Equipped;
        Debug.Log("아이템을 착용 변경.");

        if (Managers.Object.MyPlayer != null)
        {
            Managers.Object.MyPlayer.RefreshAddionalStat();
        }

        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        gameSceneUI.InventoryUI.RefreshUI();
        gameSceneUI.StatUI.RefreshUI();

    }

    public static void S_ChangeStatHandler(PacketSession session, IMessage packet)
    {
        S_ChangeStat changeStatPacket = packet as S_ChangeStat;

        // TODO
    }

    public static void S_PingHandler(PacketSession session, IMessage packet)
    {
        C_Pong pongPacket = new C_Pong();
        Debug.Log("[Server] PingCheck");
        Managers.Network.Send(pongPacket);
    }

    public static void S_ChangeRoomHandler(PacketSession session, IMessage packet)
    {
        S_ChangeRoom changePacket = packet as S_ChangeRoom;

        if (changePacket.ChangeState == false) { return; }

        Managers.Map.LoadMap(changePacket.RoomId);
    }

    public static void S_InteractHandler(PacketSession session, IMessage packet)
    {
        S_Interact interPacket = packet as S_Interact;        
        GameObject go = Managers.Object.FindbyId(interPacket.NpcId);
        if (go == null) { return; }
        NpcController nc = go.GetComponent<NpcController>();
        if (nc == null) { return; }
        /*
        bool check = interPacket.Active;

        if(check) 
        {
            Debug.Log("Npc 주변에 Player가 있습니다.");          
             nc.ActiveInteract();           
        }
        else
        {
            nc.DisActiveInteract();
        }
        */
    }

    public static void S_BuyItemHandler(PacketSession session, IMessage packet)
    {
        S_BuyItem buyPacket = packet as S_BuyItem;

        if (buyPacket.Success == false) 
        {
            Debug.Log("인벤토리가 가득차서 실패.");
        }

    }

    public static void S_SellItemHandler(PacketSession session, IMessage packet)
    {
        S_SellItem sellPacket = packet as S_SellItem;
        
        Item item = Managers.Inven.Get(sellPacket.ItemDbId);
        if (item != null)
        {
            Managers.Inven.Remove(sellPacket.ItemDbId);
            Debug.Log("판매완료.");
        }
        
        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;        
        gameSceneUI.ShopUI.RefreshUI();
        gameSceneUI.InventoryUI.RefreshUI();

    }
}

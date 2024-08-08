using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    bool _moveKeyPressed = false;    
    public int WeaponDamage {  get; private set; }
    public int ArmorDefence {  get; private set; }
    
    public NpcController TargetNpc { get; set; }

    UI_GameScene gameSceneUI;

    protected override void Init()
    {
        base.Init();        
        gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        RefreshAddionalStat();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    protected override void UpdateController()
    {
        GetUIKeyInput();

        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Moving:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        if (_moveKeyPressed)
        {
            State = CreatureState.Moving;
            return;
        }

        if (Input.GetKey(KeyCode.A) && _coSkillCoolTime == null)
        {
            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 2;
            Managers.Network.Send(skill);

            _coSkillCoolTime = StartCoroutine("CoInputCoolTime", 0.2f);
        }
    }

    Coroutine _coSkillCoolTime;
    IEnumerator CoInputCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        _coSkillCoolTime = null;
    }

    void GetUIKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UI_Inventory inventoryUI = gameSceneUI.InventoryUI;

            if (inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.gameObject.SetActive(false);
            }
            else
            {
                inventoryUI.gameObject.SetActive(true);
            }
            inventoryUI.RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {            
            UI_Stat statUI = gameSceneUI.StatUI;

            if (statUI.gameObject.activeSelf)
            {
                statUI.gameObject.SetActive(false);
            }
            else
            {
                statUI.gameObject.SetActive(true);
            }
            statUI.RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            C_ChangeRoom changePacket = new C_ChangeRoom();

            changePacket.RoomId = 2;

            Managers.Network.Send(changePacket);
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            if(TargetNpc == null) { return; }            
            UI_Shop ShopUI = gameSceneUI.ShopUI;

            if (ShopUI.gameObject.activeSelf)
            {
                ShopUI.gameObject.SetActive(false);
            }
            else
            {
                ShopUI.gameObject.SetActive(true);
            }
            ShopUI.RefreshUI();
        }
    }
    void GetDirInput()
    {
        _moveKeyPressed = true;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            _moveKeyPressed = false;
        }
    }

    protected override void MoveToNextPos()
    {
        if (_moveKeyPressed == false)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag();
            return;
        }        

        Vector3Int destPos = CellPos;

        switch (Dir)
        {
            case MoveDir.Up:
                destPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                destPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                destPos += Vector3Int.right;
                break;
        }

        if (Managers.Map.CanGo(destPos))
        {
            if (Managers.Object.FindCreature(destPos) == null)
            {
                CellPos = destPos;
            }
        }

        CheckUpdatedFlag();
    }

    protected override void CheckUpdatedFlag()
    {
        if (_updated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
        }
    }

    public void RefreshAddionalStat()
    {
        WeaponDamage = 0;
        ArmorDefence = 0;

        foreach (Item item in Managers.Inven.Items.Values)
        {
            if (item.Equipped == false) { continue; }

            switch (item.ItemType)
            {
                case ItemType.Weapon:
                    WeaponDamage += ((Weapon)item).Damage;
                    break;
                case ItemType.Armor:
                    ArmorDefence += ((Armor)item).Defence;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Npc"))
        {
            TargetNpc = collision.gameObject.GetComponent<NpcController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetNpc != null) { TargetNpc = null; }
    }
}

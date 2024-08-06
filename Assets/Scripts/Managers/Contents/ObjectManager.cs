using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    public MyPlayerController MyPlayer {  get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    
    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Add(ObjectInfo info, bool myPlayer = false)
    {
        if(MyPlayer != null && MyPlayer.Id == info.ObjectId) { return; }
        if(_objects.ContainsKey(info.ObjectId)) { return; }

        GameObjectType objectType = GetObjectTypeById(info.ObjectId);

        if(objectType == GameObjectType.Player)
        {
            if (myPlayer)
            {
                GameObject go = Managers.Resource.Instantiate("Creature/MyPlayer");
                go.name = info.Name;
                _objects.Add(info.ObjectId, go);

                MyPlayer = go.GetComponent<MyPlayerController>();
                MyPlayer.Id = info.ObjectId;
                MyPlayer.PosInfo = info.PosInfo;
                MyPlayer.Stat.MergeFrom(info.StatInfo);
                MyPlayer.SyncPos();
            }
            else
            {
                GameObject go = Managers.Resource.Instantiate("Creature/Player");
                go.name = info.Name;
                _objects.Add(info.ObjectId, go);

                PlayerController pc = go.GetComponent<PlayerController>();
                pc.Id = info.ObjectId;
                pc.PosInfo = info.PosInfo;
                pc.Stat.MergeFrom(info.StatInfo);
                pc.SyncPos(info.PosInfo.MoveDir);
            }
        }
        else if(objectType == GameObjectType.Monster)
        {
            MonsterData monsterData = null;
            Managers.Data.MonsterDict.TryGetValue(info.TemplateId, out monsterData);
            GameObject go = Managers.Resource.Instantiate(monsterData.prefab);
            go.name = monsterData.name;
            _objects.Add(info.ObjectId, go);

            MonsterController mc = go.GetComponent<MonsterController>();
            mc.Id = info.ObjectId;
            mc.PosInfo = info.PosInfo;
            mc.Stat = info.StatInfo;
            mc.SyncPos(info.PosInfo.MoveDir);
        }
        else if(objectType == GameObjectType.Projectile)
        {
            Skill skillData = null;
            Managers.Data.SkillDict.TryGetValue(info.TemplateId, out skillData);
            GameObject go = Managers.Resource.Instantiate(skillData.projectileInfo.prefab);
            go.name =skillData.name;
            _objects.Add(info.ObjectId, go);            

            ArrowController ac = go.GetComponent<ArrowController>();
            ac.PosInfo = info.PosInfo;
            ac.Stat = info.StatInfo;
            ac.SyncPos(info.PosInfo.MoveDir);
        }
        else if (objectType == GameObjectType.Npc)
        {            
            NpcData npcData = null;
            Managers.Data.NpcDict.TryGetValue(info.TemplateId, out npcData);
            GameObject go = Managers.Resource.Instantiate(npcData.prefab);
            go.name = info.Name;
            _objects.Add(info.ObjectId, go);

            NpcController nc = go.GetComponent<NpcController>();
            nc.PosInfo = info.PosInfo;            
            nc.SyncPos(info.PosInfo.MoveDir);
        }
    }

    public void Add(int id, GameObject obj)
    {
        _objects.Add(id, obj);
    }

    public void Remove(int id)
    {
        if (MyPlayer != null && MyPlayer.Id == id) { return; }
        if (_objects.ContainsKey(id) == false) { return; }

        GameObject go = FindbyId(id);
        if (go == null) { return; }
        _objects.Remove(id);

        Managers.Resource.Destroy(go);
    }

    public GameObject FindbyId(int id)
    {
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject FindCreature(Vector3Int cellPos)
    {
        foreach (GameObject obj in _objects.Values)
        {
            CreatureController cc = obj.GetComponent<CreatureController>();
            if( cc == null) {  continue; }

            if(cc.CellPos == cellPos)
                return obj;
        }

        return null;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject obj in _objects.Values )
        {
            Managers.Resource.Destroy(obj);
        }
        _objects.Clear();
        MyPlayer = null;
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{    
    public Dictionary<int, Data.Skill> SkillDict { get; private set; } = new Dictionary<int, Data.Skill>();
    public Dictionary<int, Data.ItemData> ItemDict { get; private set; } = new Dictionary<int, Data.ItemData>();
    public Dictionary<int, Data.MonsterData> MonsterDict { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.NpcData> NpcDict { get; private set; } = new Dictionary<int, Data.NpcData>();

    public void Init()
    {       
       SkillDict = LoadJson<Data.SkillData, int, Data.Skill>("SkillData").MakeDict();
       ItemDict = LoadJson<Data.ItemLoader, int, Data.ItemData>("ItemData").MakeDict();
       MonsterDict = LoadJson<Data.MonsterLoader, int, Data.MonsterData>("MonsterData").MakeDict();
       NpcDict = LoadJson<Data.NpcLoader, int, Data.NpcData>("NpcData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		string textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}").ToString();
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(textAsset);
    }
}

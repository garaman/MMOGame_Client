﻿using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{ 
    #region Skill
    [Serializable]
    public class Skill
    {
        public int id;
        public string name;
        public float cooldown;
        public int damage;
        public SkillType skillType;
        public ProjectileInfo projectileInfo;
    }

    public class ProjectileInfo
    {
        public string name;
        public float speed;
        public int range;
        public string prefab;
    }


    [Serializable]
    public class SkillData : ILoader<int, Skill>
    {
        public List<Skill> skills = new List<Skill>();

        public Dictionary<int, Skill> MakeDict()
        {
            Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
            foreach (Skill Skill in skills)
                dict.Add(Skill.id, Skill);
            return dict;
        }
    }
    #endregion

    #region Item
    [Serializable]
    public class ItemData
    {
        public int id;
        public string name;
        public ItemType itemType;
        public string iconPath;
    }
    [Serializable]
    public class WeaponData : ItemData
    {
        public WeaponType weaponType;
        public int damage;
    }
    [Serializable]
    public class ArmorData : ItemData
    {
        public ArmorType armorType;
        public int defence;
    }
    [Serializable]
    public class ConsumableData : ItemData
    {
        public ConsumableType consumableType;
        public int maxCount;
    }

    [Serializable]
    public class ItemLoader : ILoader<int, ItemData>
    {
        public List<WeaponData> weapons = new List<WeaponData>();
        public List<ArmorData> armors = new List<ArmorData>();
        public List<ConsumableData> consumables = new List<ConsumableData>();

        public Dictionary<int, ItemData> MakeDict()
        {
            Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();

            foreach (ItemData item in weapons)
            {
                item.itemType = ItemType.Weapon;
                dict.Add(item.id, item);
            }
            foreach (ItemData item in armors)
            {
                item.itemType = ItemType.Armor;
                dict.Add(item.id, item);
            }
            foreach (ItemData item in consumables)
            {
                item.itemType = ItemType.Consumable;
                dict.Add(item.id, item);
            }
            return dict;
        }
    }
    #endregion

    #region Monster

    [Serializable]
    public class MonsterData
    {
        public int id;
        public string name;
        public string prefab;
        public StatInfo stat;        
    }

    [Serializable]
    public class MonsterLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
            {
                dict.Add(monster.id, monster);
            }

            return dict;
        }
    }

    #endregion

    #region Npc

    [Serializable]
    public class NpcData
    {
        public int id;
        public string name;
        public NpcType npcType;
        public string prefab;
    }

    [Serializable]
    public class NpcLoader : ILoader<int, NpcData>
    {
        public List<NpcData> npcs = new List<NpcData>();

        public Dictionary<int, NpcData> MakeDict()
        {
            Dictionary<int, NpcData> dict = new Dictionary<int, NpcData>();
            foreach (NpcData npc in npcs)
            {
                dict.Add(npc.id, npc);
            }

            return dict;
        }
    }

    #endregion
}
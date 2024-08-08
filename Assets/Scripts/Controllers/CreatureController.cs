using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : BaseController
{
    HpBar _hpbar;
   
    public override StatInfo Stat
    {
        get { return base.Stat; }
        set
        {
            base.Stat = value;            
            UpdateHpBar();
        }
    }
    public override int Hp
    {
        get { return Stat.Hp; }
        set 
        {
            base.Hp = value;
            UpdateHpBar();
        }
    }
   
    protected void AddHPBar()
    {
        GameObject go = Managers.Resource.Instantiate("UI/HpBar", transform);
        go.transform.localPosition = new Vector3(0, 0.1f, 0);
        go.name = "HpBar";
        _hpbar = go.GetComponent<HpBar>();
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        if (_hpbar == null) { return; }

        float ratio = 0.0f;
        if (Stat.MaxHp > 0)
        {
            ratio = ((float)Hp / Stat.MaxHp);            
        }
        _hpbar.SetHpBar(ratio);

        //Debug.Log($"{this.gameObject.name} / {Hp}");
    }

    protected override void Init()
    {
        base.Init();        
        AddHPBar();        
    }
 
    public virtual void OnDamaged()
    {

    }

    public virtual void OnDead()
    {
        State = CreatureState.Dead;

        GameObject effect = Managers.Resource.Instantiate("Effect/Effect");
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("DeathEffect");
        GameObject.Destroy(effect, 0.25f);
    }

    public virtual void UseSkill(int skillId)
    {
       
    }
}

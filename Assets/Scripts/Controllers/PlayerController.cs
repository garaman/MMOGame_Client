using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class PlayerController : CreatureController
{
    protected Coroutine _coSkill;
    protected bool _rangeSkill = false;

    protected override void Init()
    {
        base.Init();
    }
    protected override void UpdateAnimation()
    {
        if(_animator== null || _spriteRenderer == null) {  return; }    

        if (State == CreatureState.Idle)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play("Idle_Back");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Idle_Front");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Idle_Side");
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Idle_Side");
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Moving)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play("Walk_Back");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("Walk_Front");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("Walk_Side");
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("Walk_Side");
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Skill)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    _animator.Play( _rangeSkill ? "Attack_Weapon_Back" : "Attack_Back" );
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Front" : "Attack_Front");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Side" : "Attack_Side");
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play(_rangeSkill ? "Attack_Weapon_Side" : "Attack_Side");
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Dead)
        {

        }
        else
        {

        }
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }


    public void UseSkill(int skillId)
    {
        if (skillId == 1)
        {
            _coSkill = StartCoroutine("CoStartPunch");
        }
    }

    protected virtual void CheckUpdatedFlag()
    {
        
    }

    IEnumerator CoStartPunch()
    {        // 대기 시간
        _rangeSkill = false;
        State = CreatureState.Skill;
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }

    IEnumerator CoStartShootArrow()
    {
        GameObject go = Managers.Resource.Instantiate("Skill/Arrow");
        ArrowController ac = go.GetComponent<ArrowController>();
        ac.Dir = Dir;
        ac.CellPos = CellPos;

        // 대기 시간
        _rangeSkill = true;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coSkill = null;
    }

    
}

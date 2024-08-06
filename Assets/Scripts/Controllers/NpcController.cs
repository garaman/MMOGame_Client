using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : BaseController
{
    [SerializeField]
    SpriteRenderer sprite;

    public bool Active { get; private set; }
    public GameObject target;
    protected override void Init()
    {
        base.Init();
        DisActiveInteract();
    }

    private void Update()
    {
        
    }

    public void ActiveInteract()
    {
        sprite.enabled = true;
        Active = true;
    }
    public void DisActiveInteract()
    {
        sprite.enabled = false;
        Active = false;
    }
}

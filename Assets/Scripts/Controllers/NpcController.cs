using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : BaseController
{
    [SerializeField]
    SpriteRenderer sprite;

    public bool Active { get; private set; }

    protected override void Init()
    {
        base.Init();
        DisActiveInteract();
    }

    private void Update()
    {
        
    }

    void ActiveInteract()
    {
        sprite.enabled = true;
        Active = true;
    }
    void DisActiveInteract()
    {
        sprite.enabled = false;
        Active = false;
    }

    public void ActiveNpc()
    {
        UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {            
            MyPlayerController mpc = collision.GetComponent<MyPlayerController>();
            if (mpc != null)
            {
                ActiveInteract();
                mpc.TargetNpc = this;
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MyPlayerController mpc = collision.GetComponent<MyPlayerController>();
            if (mpc != null)
            {
                DisActiveInteract();
                mpc.TargetNpc = null;
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    //UI_LoginScene _sceneUI;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Screen.SetResolution(640, 480, false);

        //_sceneUI = Managers.UI.ShowSceneUI<UI_LoginScene>();
        Managers.UI.ShowSceneUI<UI_Title>();
        Managers.Sound.Play("Bgm/Main", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}

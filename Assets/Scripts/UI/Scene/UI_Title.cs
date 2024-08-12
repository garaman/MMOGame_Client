using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Title : UI_Scene
{
    enum Buttons
    {
        StartButton
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
    }

    public void OnClickStartButton(PointerEventData eventData)
    {
        Managers.UI.ShowSceneUI<UI_LoginScene>().GetComponent<Canvas>().sortingOrder = 5;
        //this.gameObject.SetActive(false);
    }
}

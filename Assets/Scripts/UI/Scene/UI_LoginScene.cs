using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    enum GameObjects
    {
        AccountInput,
        PasswordInput
    }

    enum Buttons
    {
        CreateBtn,
        LoginBtn
    }

    public override void Init()
    {
        base.Init();
        

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CreateBtn).gameObject.BindEvent(OnClickCreateButton);
        GetButton((int)Buttons.LoginBtn).gameObject.BindEvent(OnClickLoginButton);

    }

    public void OnClickCreateButton(PointerEventData eventData)
    {
        string account = GetObject((int)GameObjects.AccountInput).GetComponent<InputField>().text;
        string password = GetObject((int)GameObjects.PasswordInput).GetComponent<InputField>().text;

        CreateAccountPacketReq packet = new CreateAccountPacketReq()
        {
            AccountName = account,
            Password = password
        };

        Managers.Web.SendPostRequest<CreateAccountPacketRes>("account/create", packet, (res) =>
        {
            Debug.Log("CreateOk");

            GetObject((int)GameObjects.AccountInput).GetComponent<InputField>().text = "";
            GetObject((int)GameObjects.PasswordInput).GetComponent<InputField>().text = "";
        });
    }

    public void OnClickLoginButton(PointerEventData eventData)
    {
        string account = GetObject((int)GameObjects.AccountInput).GetComponent<InputField>().text;
        string password = GetObject((int)GameObjects.PasswordInput).GetComponent<InputField>().text;

        LoginAccountPacketReq packet = new LoginAccountPacketReq()
        {
            AccountName = account,
            Password = password
        };

        Managers.Web.SendPostRequest<LoginAccountPacketRes>("account/login", packet, (res) =>
        {
            Debug.Log(res.LoginOk);
            GetObject((int)GameObjects.AccountInput).GetComponent<InputField>().text = "";
            GetObject((int)GameObjects.PasswordInput).GetComponent<InputField>().text = "";

            if (res.LoginOk)
            {
                Managers.Network.AccountId = res.AccountId;
                Managers.Network.Token = res.Token;

                UI_SelectServerPopup poup = Managers.UI.ShowPopupUI<UI_SelectServerPopup>();
                poup.SetServer(res.ServerList);
            }
        });
    }



}

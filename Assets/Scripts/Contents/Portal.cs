using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;

    private int potalId = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MyPlayerController mpc = collision.GetComponent<MyPlayerController>();
            if (mpc != null)
            {
                Effect.SetActive(true);
                mpc.TargetPortal = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.CompareTag("Player"))
        {
            MyPlayerController mpc = collision.GetComponent<MyPlayerController>();
            if (mpc != null)
            {
                Effect.SetActive(false);
                mpc.TargetPortal = null;
            }
        }
    }

    public void ActivePortal()
    {
        C_ChangeRoom changePacket = new C_ChangeRoom();

        changePacket.RoomId = potalId;

        Managers.Network.Send(changePacket);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler 
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDoubleClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    private const float doubleClickTime = 0.3f;
    private float lastClickTime = 0f;
    private bool isSingleClick = false;
    public void OnPointerClick(PointerEventData eventData)
	{
        float timeSinceLastClick = Time.time - lastClickTime;
        
        if (timeSinceLastClick <= doubleClickTime)
        {
            isSingleClick = false;
            if (OnDoubleClickHandler != null)
                OnDoubleClickHandler.Invoke(eventData);
        }
        else
        {
            isSingleClick = true;
            if (OnClickHandler != null)
                OnClickHandler.Invoke(eventData);
        }
        if (isSingleClick == false) { isSingleClick = false; }
        lastClickTime = Time.time;
	}

	public void OnDrag(PointerEventData eventData)
    {
		if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
	}
}

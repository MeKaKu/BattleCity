using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApkBtCon : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private ApkUICon apkui;
    private void Awake()
    {
        apkui = transform.parent.gameObject.GetComponent<ApkUICon>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(name);
        if (name == "f")
        {
            apkui.setFire(true);
        }
        else
        {
            apkui.SetDir(name, true);
        }
        //throw new NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (name == "f")
        {
            apkui.setFire(false);
        }
        else
        {
            apkui.SetDir(name, false);
        }
        //throw new NotImplementedException();
    }
}

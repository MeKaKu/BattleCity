using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyCon : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IDragHandler
{
    private Vector3 orgPos;
    private Vector3 curPos;
    private GameObject img;
    private float R = 60;
    private float r = 22.5f;
    private ApkUICon apkui;

    private void Awake()
    {
        //GetComponent<Image>().rectTransform.localPosition = Vector3.zero;
        //orgPos = transform.position;
        //orgPos = Camera.main.WorldToScreenPoint(orgPos);
        //
        //orgPos = GetComponent<Image>().rectTransform.localPosition;
        orgPos = GetComponent<Image>().rectTransform.anchoredPosition * Screen.width / 711;
        img = transform.Find("j").gameObject;
        apkui = transform.parent.gameObject.GetComponent<ApkUICon>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("drog" + eventData);
        curPos = eventData.position;
        //Debug.Log(curPos);
        //Debug.Log(GetDir());
        apkui.SetDirI(GetDir());
        //throw new NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("down" + eventData);
        curPos = eventData.position;
        //orgPos = -GetComponent<Image>().rectTransform.localPosition;
        //Debug.Log(curPos);
        apkui.SetDirI(GetDir());
        //Debug.Log(orgPos);
        //throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("up" + eventData);
        //orgPos = eventData.position;
        //curPos = eventData.position;
        //Debug.Log(curPos);
        apkui.SetDirI(-1);
        Image image = img.GetComponent<Image>();
        image.rectTransform.localPosition = Vector3.zero;
        //img.transform.position = orgPos;
        //throw new NotImplementedException();
    }

    private int GetDir()
    {
        int dir = -1;
        Vector3 temp = curPos - orgPos;
        temp = temp.normalized;
        float x = temp.x, y = temp.y;
        if (x > 0 && y > 0)
        {
            dir = x > y ? 3 : 0;
        }
        else if (x > 0 && y < 0)
        {
            dir = x > -y ? 3 : 1;
        }
        else if (x < 0 && y > 0)
        {
            dir = -x > y ? 2 : 0;
        }
        else if (x < 0 && y < 0)
        {
            dir = -x > -y ? 2 : 1;
        }
        Image image = img.GetComponent<Image>();
        image.rectTransform.localPosition = temp * (R - r);
        return dir;
    }

}

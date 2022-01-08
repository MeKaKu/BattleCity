using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApkUICon : MonoBehaviour
{
    public bool fire,prefire;
    public bool up, down, left, right;
    private void Awake()
    {
        //transform.position = transform.parent.position;
    }
    public void SetDir(string dir,bool fg)
    {
        //up = down = left = right = false;
        switch (dir)
        {
            case "w": up = fg;break;
            case "s": down = fg;break;
            case "a": left = fg;break;
            case "d": right = fg;break;
        }
    }
    public void SetDirI(int dir)
    {
        up = down = left = right = false;
        switch (dir)
        {
            case 0: up = true; break;
            case 1: down = true; break;
            case 2: left = true; break;
            case 3: right = true; break;
        }
    }

    private void UnsetDir(int dir)
    {
        switch (dir)
        {
            case 0: up = false; break;
            case 1: down = false; break;
            case 2: left = false; break;
            case 3: right = false; break;
        }
    }

    public void setFire( bool fg)
    {
        //bool curFire = true;
        //if (curFire != prefire)
        //{
        //    fire = true;
        //}
        fire = fg;
    }
}

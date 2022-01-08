using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //按键设置
    [Header("----- Key Settings -----")]
    public string keyUp = "w"; //上
    public string keyDown = "s"; //下
    public string keyLeft = "a"; //左
    public string keyRight = "d"; //右
    public string keyFire = "space"; //开火

    [Header("----- AI Setting -----")]
    public bool useAI = false;
    public int level = 0;

    [Header("----- Signals -----")]
    //信号
    public int moveDirection = -1; //移动方向，上0，下1，左2，右3
    public bool isFire = false; //是否开火

    private int timeCount = 0; //AI
    public int preDirection = 1;
    public bool canMove = false;

    public GameObject apkuiPre;
    private ApkUICon ApkInput;
    private bool IsApkUser = false;
    // Start is called before the first frame update
    private void Awake()
    {
        IsApkUser = (Application.platform == RuntimePlatform.Android);
        //IsApkUser = true;
    }
    void Start()
    {
        if(IsApkUser)
        {
            GameObject apkui = GameObject.Find("Canvas/apkui");
            if(apkui == null)
            {
                apkui = Instantiate(apkuiPre, GameObject.Find("Canvas").transform);
                apkui.name = "apkui";
            }
            //Debug.Log(apkui.transform.parent.gameObject.name);
            if(!useAI) ApkInput = GameObject.Find("Canvas/apkui").GetComponent<ApkUICon>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useAI)
        {
            ByAI();
        }
        else
        {
            if (!IsApkUser)
                ByPlayer();
            else
                ApkPlayer();
        }
    }

    //用户操作
    private void ByPlayer()
    {
        if (Input.GetKeyDown(keyFire))
        {
            isFire = true;
        }
        if (Input.GetKey(keyUp))
        {
            moveDirection = 0;
        }
        else if (Input.GetKey(keyDown))
        {
            moveDirection = 1;
        }
        else if (Input.GetKey(keyLeft))
        {
            moveDirection = 2;
        }
        else if (Input.GetKey(keyRight))
        {
            moveDirection = 3;
        }
        else
        {
            moveDirection = -1;
        }
        canMove = !Barrier();
    }

    private void ApkPlayer()
    {
        isFire = ApkInput.fire;
        if (ApkInput.up)
        {
            moveDirection = 0;
        }
        else if (ApkInput.down)
        {
            moveDirection = 1;
        }
        else if (ApkInput.left)
        {
            moveDirection = 2;
        }
        else if (ApkInput.right)
        {
            moveDirection = 3;
        }
        else
        {
            moveDirection = -1;
        }
        canMove = !Barrier();
    }

    //AI操作
    private void ByAI()
    {
        if (Random.Range(-1, 5) <= level && Barrier() && timeCount == 4)
        {
            moveDirection = Random.Range(0, 256) / 16;
            moveDirection %= 4;
            if(moveDirection == 0)
            {
                moveDirection = Random.Range(0, 64) / 8;
                moveDirection %= 4;
            }
        }
        preDirection = moveDirection;
        canMove = !Barrier();
        int num = Random.Range(-5, 20);
        if (num < level && timeCount == 0)
        {
            isFire = true;
        }
        timeCount++;
        timeCount %= 5;
    }

    public bool Barrier()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        if (moveDirection == 0) pos.y += 0.75f;
        else if(moveDirection == 1) pos.y -= 0.75f;
        if (moveDirection == 2) pos.x -= 0.75f;
        else if (moveDirection == 3) pos.x += 0.75f;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(moveDirection<2?.5f:.25f, moveDirection<2?.25f:.5f), 0, LayerMask.GetMask("wall", "redwall", "tank","water"));
        //foreach (var collider in colliders)
        //{
        //    Debug.Log(collider.gameObject.name);
        //}
        //Debug.Log(colliders.Length);
        return colliders.Length > 0;
    }
}

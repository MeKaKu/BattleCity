using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    public int pid = 0;
    public Sprite[] images; //贴图列表 //4个方向8张贴图//32(p1)+32(p2)+32(enemy)
    public float unitLen = 90; //单位长度
    public int walkStep = 5; //移动一步所需时间 walkStep*Time.deltaTime
    public float stepLen = 0.5f; //移动步长

    private PlayerInput pi; //用户输入
    [SerializeField]
    private int walkCount = 0; //移动计数
    private int turnCount = 0; //转向计数
    private SpriteRenderer render; //贴图renderer
    private int index = 0; //贴图索引
    public int dir = 0; //方向索引，上0，下1，左2，右3
    private Vector3[] vecs = { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0) }; //方向向量 上下左右
    public bool doubleFire = false;
    public GameObject bulletPrefab; //子弹预制体
    public GameObject bullet; //子弹对象
    private GameObject bullet2;
    public GameObject invincibilityPrefab; //护盾预制体
    private GameObject invincibility; //护盾对象
    private float invincibleTime = 15.0f; //无敌时间
    private float birthInvincibleTime = 3.0f; //出生无敌时间
    //private bool isInvincible = false; //是否无敌
    public int level = 0; //子弹等级
    public int health = 1; //生命值（0,4]
    public int offset;
    public int suffix;
    public bool isRed; //是否是红色坦克
    public Sprite[] redImages;
    private int redcnt = 0;
    private Color32[] colors = { new Color32(255, 255, 255, 255), new Color32(211, 255, 189, 255), new Color32(226, 212, 157, 255), new Color32(255, 96, 255, 255) };
    public GameObject bombPrefab;
    private bool beKilled = false; //被杀死
    private int ind = 0;

    public GameObject smallScorePrefab;
    //public int intervalTime = 30;
    //private int intervalCnt = 0;

    private new AudioSource[] audio;


    private void Awake()
    {
        //获取用户输入组件
        pi = GetComponent<PlayerInput>();
        //audio
        audio = GetComponents<AudioSource>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //调整坦克的大小
        render = GetComponent<SpriteRenderer>();
        float scalex = unitLen / 30;
        float scaley = unitLen / 30;
        render.transform.localScale = new Vector3(scalex, scaley, 1);
        render.sprite = images[offset * 8 + suffix * 8];
        //出生无敌护盾
        if (!pi.useAI)
        {
            invincibility = Instantiate(invincibilityPrefab, transform);
            Invoke("DisInvincible", birthInvincibleTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        if (pi.isFire && bullet == null)
        {
            Fire();
            //if (doubleFire)
            //{
            //    Invoke("Fire", 0.1f);
            //}
        }
        else if(doubleFire && pi.isFire && bullet != null && bullet2 == null)
        {
            bullet2 = bullet;
            Fire();
        }
        pi.isFire = false;
    }
    private void Walk()
    {
        //转向
        if (pi.moveDirection >= 0 && walkCount == 0 && turnCount == 0)
        {
            if(dir == pi.moveDirection)
            {
                if (pi.canMove)// && dir == pi.moveDirection
                {
                    walkCount = walkStep;
                }
            }
            else
            {
                dir = pi.moveDirection;
                turnCount = walkStep >> 2;
            }
            index = dir << 1;
            //贴图在images数组中的索引
            ind = offset * 8 + suffix * 8 + index;
            render.sprite = images[ind]; //更改贴图
        }
        ind = offset * 8 + suffix * 8 + index;
        //移动中
        if (walkCount > 0)
        {
            index ^= 1;
            //向指定方向移动
            //if (!pi.Barrier()) transform.position += vecs[dir] * (stepLen / walkStep);
            //else transform.position += vecs[dir] * 0.01f;

            transform.position += vecs[dir] * (stepLen / walkStep);
            render.sprite = images[ind]; //更改贴图
            //transform.position = new Vector3(myToFive(transform.position.x), myToFive(transform.position.y), 0);
            walkCount -= 1;
        }
        else
        {
            //transform.position = new Vector3(myToFive(transform.position.x, 10), myToFive(transform.position.y, 10), 0);
        }
        //高防坦克颜色变化
        if (pi.useAI)
        {
            render.color = colors[health - 1];
            //Debug.Log(health);
        }
        //红色坦克颜色闪烁
        if (isRed)
        {
            if (++redcnt > 2)
            {
                render.sprite = redImages[ind - 64];
            }
            redcnt %= 6;
        }
        if(turnCount == 0)
        {
            //index ^= 1;
        }
        else
        {
            turnCount--;
        }
        
    }
    
    //将x就近取0.05的倍数 //(坐标位置吸附到单位大小为0.05的网格上面)
    public float myToFive(float x, int pram = 100)
    {
        int sg = x > 0 ? 1 : -1;
        int y = (int)(Mathf.Abs(x) * pram);
        y = (y + 4) / 5 * 5;
        return y * sg * 1.0f / pram;
    }
    private void Fire()
    {
        Vector3 pos = transform.position + vecs[dir] * 0.6f;
        bullet = Instantiate(bulletPrefab, pos, transform.rotation);
        BulletController bulletCon = bullet.GetComponent<BulletController>();
        bulletCon.dir = dir;
        bulletCon.level = level;
        bulletCon.belongEnemy = pi.useAI;
        if (!pi.useAI)
        {
            audio[0].PlayOneShot(audio[0].clip);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //拾取道具    //只有玩家可以拾取道具
        if (!pi.useAI && LayerMask.LayerToName(collision.gameObject.layer).Equals("tool"))
        {
            ToolController toolCon = collision.gameObject.GetComponent<ToolController>();
            //Debug.Log("Get Tool..."+toolCon.index);
            switch (toolCon.index)
            {
                case 0: //定时器
                    SendMessageUpwards("StopAllEnemy"); //静止所有敌人
                    break;
                case 1: //五角星
                    if (suffix < 3)
                    {
                        suffix++;
                        if (suffix == 1) level = 1;
                        else if (suffix == 2) doubleFire = true;
                        else if (suffix == 3) health = 2;
                        PlayerPrefs.SetInt("player" + pid + "Suffix", suffix);
                    }
                    break;
                case 2: //无敌护盾
                    if(invincibility == null)
                    {
                        invincibility = Instantiate(invincibilityPrefab, transform);
                    }
                    else
                    {
                        CancelInvoke("DisInvincible"); //取消旧的延时调用，刷新无敌护盾持续时间
                    }
                    Invoke("DisInvincible", invincibleTime);//延时取消无敌护盾
                    break; 
                case 3: //炸弹
                    SendMessageUpwards("DestroyAllEnemy"); //摧毁所有敌人
                    break;
                case 4: //铲子
                    SendMessageUpwards("ReinforceBase"); //加固基地
                    break;
                case 5: //坦克
                    SendMessageUpwards("AddLife",pid); //增加生命值
                    break;
                default: //
                    Debug.Log("Unkown Tool...>_<...");
                    break;

            }
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer).Equals("bullet"))
        {//被击中
            if(invincibility == null && (!pi.useAI || !collision.gameObject.GetComponent<BulletController>().belongEnemy))
            { //友军可以互相伤害，但是敌人不行
                if (health > 1)
                {
                    audio[1].Play();
                    health--;
                    suffix = 0;
                    level = 0;
                    if(!pi.useAI) PlayerPrefs.SetInt("player" + pid + "Suffix", 0);
                }
                else
                {
                    beKilled = true;
                    Die();
                }
            }
        }
    }
    public void Die()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
        bomb.GetComponent<BombController>().len = 5;
        if(invincibility != null)
        {
            CancelInvoke("DisInvincible");
            //DisInvincible();
        }
        if (isRed && beKilled)
        {
            SendMessageUpwards("GenerateTool"); //向上传递产生道具的消息
        }

        if (pi.useAI)
        { //敌人死亡,显示得分
            //Vector3 pos = transform.position;
            GameObject smallScore = Instantiate(smallScorePrefab, GameObject.Find("Canvas").transform);
            smallScore.transform.position = transform.position;
            smallScore.GetComponent<Text>().text = (100 * (offset - 7)) + "";
            SendMessageUpwards("AddScore", (100 * (offset - 7)));
            //smallScore.GetComponent<SmallScoreUIController>().pos = transform.position;
        }
        else
        {
            PlayerPrefs.SetInt("player" + pid + "Suffix", 0);
        }

        Destroy(gameObject);
    }
    private void DisInvincible()
    {
        Destroy(invincibility);
    }
}

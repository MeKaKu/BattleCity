using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform enemyFloder;
    private bool stoped = false;
    private float stopTime = 20.0f;

    private Vector3[] birthPlace = { new Vector3(-6, 6, 0), new Vector3(0, 6, 0), new Vector3(6, 6, 0) };
    public int createTime = 30*5;
    public int maxEnemyNum = 5;
    public GameObject tankPrefab; //坦克预制体
    public GameObject birthPrefab; //出生特效预制体
    private GameObject birth;
    public GameObject enemy{ private set; get; }
    private GameLogic game;
    private int timeCnt = 0;
    private int enemyNum = 0;
    private int[] myRandom = { -1, -1, -1, -1 };

    public int tankNum = 20;
    private int restNum = 20;
    //UI数据
    public int[] tankCnt = { 0, 0, 0, 0 };

    
    public GameObject enemyImgPrefab;
    
    private GameObject[] images;

    
    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<GameLogic>();
        enemyFloder = transform.Find("Enemies");
        restNum = tankNum;
        int cnt = 0;
        while (cnt < 4)
        {
            int x = Random.Range(0, 4 + cnt * 5) % 4;
            if (myRandom[x] == -1) myRandom[x] = cnt++;
        }
        images = new GameObject[tankNum];
        
        for(int i = 0; i < tankNum; i++)
        {
            images[i] = Instantiate(enemyImgPrefab, GameObject.Find("Canvas/ui").transform);
            images[i].transform.position = new Vector3(-10 + (i % 3) * 1f, 6 - (i / 3) * 1f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyFloder.childCount);
        if(game.state == GameLogic.STATE.running)
        {
            bool flag = true; //是否成功生成敌人
            enemyNum = enemyFloder.childCount;
            if (restNum > 0 && timeCnt == 5 && enemyNum < maxEnemyNum)
            {
                enemyNum++;
                flag = CreateEnemy();
            }
            if (flag)
            {
                timeCnt++;
                timeCnt %= createTime;
            }
            //enemyNum = enemyFloder.childCount;
            if (enemyNum + restNum == 0)
            {
                game.state = GameLogic.STATE.none;
                Invoke("WinGame", 4f);
            }
        }
    }

    private void WinGame()
    {
        game.state = GameLogic.STATE.win;
    }

    //生成敌人
    private bool CreateEnemy()
    {
        int index = Random.Range(0, 3);
        //判断出生点是否有坦克
        if(Physics2D.OverlapBoxAll(new Vector2(birthPlace[index].x, birthPlace[index].y), new Vector2(.9f, .9f), 0, LayerMask.GetMask("tank")).Length > 0)
        {
            return false;
        }
        int type = Random.Range(0, myRandom[Random.Range(0, 4)] + Random.Range(1, 5)) % 4;
        enemy = Instantiate(tankPrefab, transform.Find("Enemies"));
        enemy.transform.position = birthPlace[index];
        PlayerInput pi = enemy.GetComponent<PlayerInput>();
        pi.useAI = true;
        pi.moveDirection = 1;
        TankController tc = enemy.GetComponent<TankController>();
        tc.dir = 1;
        tc.offset = type + 8;
        //tc.walkStep = 10;
        //tc.stepLen = .5f;
        tc.isRed = (type != 3 && Random.Range(0, 3) == 1);
        switch (type)
        {
            case 0: //最弱坦克
                
                break;
            case 1: //高速坦克
                tc.walkStep = 4;
                break;
            case 2: //高速炮弹坦克
                tc.level = 5;
                break;
            case 3: //高防坦克
                tc.health = 4;
                break;
            default:
                Debug.Log("No Such Type Enemy...QAQ...");
                break;
        }
        enemy.SetActive(false);
        birth = Instantiate(birthPrefab, transform);
        birth.transform.position = birthPlace[index];
        Invoke("ActiveEnemy", 2);

        //数据统计
        tankCnt[type]++;
        restNum--;
        images[restNum].SetActive(false);
        return true;
    }

    private void ActiveEnemy()
    {
        Destroy(birth);
        enemy.SetActive(true);
        if (stoped)
        {
            PlayerInput pi = enemy.GetComponent<PlayerInput>();
            pi.enabled = false;
            pi.moveDirection = -1;
        }
    }

    //继续所有敌人的行动
    private void ResumAllEnemy()
    {
        for (int i = 0; i < enemyFloder.childCount; i++)
        {
            PlayerInput pi = enemyFloder.GetChild(i).gameObject.GetComponent<PlayerInput>();
            pi.enabled = true;
            pi.moveDirection = pi.preDirection;
        }
        stoped = false;
    }

    /// <summary>
    /// 消息函数
    /// </summary>
    //停止所有敌人
    private void StopAllEnemy()
    {
        if (stoped)
        {
            CancelInvoke("ResumAllEnemy");
        }
        else
        {
            stoped = true;
            for (int i = 0; i < enemyFloder.childCount; i++)
            {
                PlayerInput pi = enemyFloder.GetChild(i).gameObject.GetComponent<PlayerInput>();
                pi.enabled = false;
                pi.moveDirection = -1;
                //pi.isFire = false;
            }
        }
        Invoke("ResumAllEnemy", stopTime);
    }

    //摧毁所有敌人
    private void DestroyAllEnemy()
    {
        //while (enemyFloder.childCount > 0)
        //{
        //    enemyFloder.GetChild(0).gameObject.GetComponent<TankController>().Die();
        //}
        for (int i = 0; i < enemyFloder.childCount; i++)
        {
            if(enemyFloder.GetChild(i).gameObject.activeInHierarchy)
            enemyFloder.GetChild(i).gameObject.GetComponent<TankController>().Die();
        }
        //enemyFloder.GetChild(0).gameObject.GetComponent<TankController>().Die();
        //Debug.Log(enemyFloder.childCount);
    }

}

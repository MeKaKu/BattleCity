using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int[] life = { 3, 0 }; //玩家剩余生命值
    private float reinforceTime = 30; //基地加固时长
    public GameObject toolPrefab; //工具预制体
    public int[] playerNum = { 0, 0 }; //玩家坦克存在数目
    private GameLogic game;
    public GameObject[] player;
    public GameObject tankPrefab;
    public int score = 0;
    public GameObject totalScorePrefab;
    private GameObject totalScore;
    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<GameLogic>();
        player = new GameObject[2];
        life[0] = PlayerPrefs.GetInt("player0Life");
        if (PlayerPrefs.GetInt("playerNum") == 2)
        {
            life[1] = PlayerPrefs.GetInt("player1Life");
        }
        else life[1] = 0;
        totalScore = Instantiate(totalScorePrefab, GameObject.Find("Canvas/ui").transform);
        totalScore.transform.position = new Vector3(9.63f, 4.25f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.Find.childCount);
        if (game.state == GameLogic.STATE.running)
        {
            PlayerHandle(0);
            PlayerHandle(1);
            if (life[0] + life[1] + playerNum[0] + playerNum[1] == 0)
            {
                game.state = GameLogic.STATE.defeat;
            }
        }
        if (game.state == GameLogic.STATE.computing || game.state == GameLogic.STATE.ending)
        {
            for (int i = 0; i < 2; i++)
            {
                if (player[i] != null)
                {
                    player[i].GetComponent<PlayerInput>().enabled = false;
                    player[i].GetComponent<PlayerInput>().moveDirection = -1;
                }
            }
        }
    }

    //玩家
    private void PlayerHandle(int pid)
    {
        playerNum[pid] = transform.Find("Player" + pid).childCount;
        if (playerNum[pid] < 1)
        {
            if (life[pid] > 0)
            {
                life[pid]--;
                PlayerPrefs.SetInt("player" + pid + "Life", life[pid]);
                player[pid] = Instantiate(tankPrefab, transform.Find("Player" + pid));
                player[pid].transform.position = new Vector3(-2 + 4 * pid, -6, 0);
                TankController tc = player[pid].GetComponent<TankController>();
                if (pid == 1)
                {
                    //设置玩家2的键位
                    PlayerInput pi = player[pid].GetComponent<PlayerInput>();
                    pi.keyUp = "up"; //上
                    pi.keyDown = "down"; //下
                    pi.keyLeft = "left"; //左
                    pi.keyRight = "right"; //右
                    pi.keyFire = "return"; //开火
                    tc.offset = 4;
                    tc.pid = 1;
                }
                tc.suffix = PlayerPrefs.GetInt("player" + pid + "Suffix");
                if (tc.suffix >= 1) tc.level = 1;
                if (tc.suffix >= 2) tc.doubleFire = true;
                if (tc.suffix == 3) tc.health = 2;
            }
        }
        playerNum[pid] = transform.Find("Player" + pid).childCount;
    }

    //取消加固基地
    private void UnreinforceBase()
    {
        transform.Find("Map/part").gameObject.SetActive(false);
        transform.Find("Map/part_0").gameObject.SetActive(true);
        
    }

    //生成道具
    private void GenerateTool()
    {
        int x = Random.Range(-6, 6);
        int y = Random.Range(-5, 6);
        Vector2 pos = new Vector2(x + 0.5f, y + 0.5f);
        Transform tools = transform.Find("Tools");
        //while (tools.childCount > 0) //销毁之前产生的道具
        //{
        //    Destroy(tools.GetChild(0).gameObject);
        //}
        for(int i = 0; i < tools.childCount; i++)
        {
            Destroy(tools.GetChild(i).gameObject);
        }
        //实例化新道具
        GameObject tool = Instantiate(toolPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), tools.rotation, tools);
    }

    //加固基地
    private void ReinforceBase()
    {
        GameObject part0 = transform.Find("Map/part_0").gameObject;
        GameObject part = transform.Find("Map/part").gameObject;
        if(part.activeInHierarchy == true) //已经加固了，延长时间
        {
            CancelInvoke("UnreinforceBase");
        }
        else //加固
        {
            part0.SetActive(false);
            part.SetActive(true);
            for(int i = 0; i < part.transform.childCount; i++)
            {
                part.transform.GetChild(i).gameObject.SetActive(true);
            }
            Invoke("UnreinforceBase", reinforceTime);
        }
        
    }

    //增加生命值
    private void AddLife(int pid)
    {
        life[pid]++;
        PlayerPrefs.SetInt("player" + pid + "Life", life[pid]);
    }


    private void AddScore(int _score)
    {
        score += _score;
        totalScore.transform.Find("score").GetComponent<Text>().text = "" + score;
    }

    private void BaseDestroy()
    {
        game.state = GameLogic.STATE.defeat;
        for(int i = 0; i < 2; i++)
        {
            if (player[i] != null)
            {
                player[i].GetComponent<PlayerInput>().enabled = false;
                player[i].GetComponent<PlayerInput>().moveDirection = -1;
            }
        }
    }
}

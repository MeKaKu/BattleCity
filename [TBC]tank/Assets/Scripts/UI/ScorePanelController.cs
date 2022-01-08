using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelController : MonoBehaviour
{
    private  GameObject mainCon;//主控
    private EnemyController enemyCon;//
    private int index = 0;
    private int cnt = 0;
    private int timeCnt = 1;
    private new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        mainCon = GameObject.Find("GameController");
        enemyCon = mainCon.GetComponent<EnemyController>();
        transform.Find("TotalScore").GetComponent<Text>().text = "SCORE : " + mainCon.GetComponent<PlayerController>().score;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeCnt == 0)
        {//active
            if(index<4) Act();
            else if(index == 4)
            {
                index++;
                transform.Find("total").gameObject.SetActive(true);
                transform.Find("TotalNum").gameObject.SetActive(true);
                transform.Find("TotalNum").gameObject.GetComponent<Text>().text = "" + enemyCon.tankNum;

                Invoke("WinGame", 1.5f);

            }
        }
        timeCnt++;
        timeCnt %= 4;
    }

    private void Act()
    {
        if(cnt >= enemyCon.tankCnt[index])
        {
            index++;
            cnt = 0;
            if (index < 4)
            {
                transform.Find("arrow" + index).gameObject.SetActive(true);
                transform.Find("NumText" + index).gameObject.SetActive(true);
                transform.Find("ScoreText" + index).gameObject.SetActive(true);
            }
            return;
        }
        //计算【index】
        cnt++;
        transform.Find("NumText" + index).gameObject.GetComponent<Text>().text = "" + cnt;
        transform.Find("ScoreText" + index).gameObject.GetComponent<Text>().text = (100 * (index+1) * cnt) + " pts";
        audio.Play();
    }

    private void WinGame()
    {
        mainCon.GetComponent<GameLogic>().state = GameLogic.STATE.next;
    }
}

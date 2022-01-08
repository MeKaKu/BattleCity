using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public GameObject panelPrefab;
    public GameObject stageTextPrefab;
    public GameObject scorePanelPrefab;
    public GameObject gameOverTextPrefab;
    public GameObject tipPanelPrefab;
    private GameObject[] panel = { null, null };
    private GameObject stageText;
    private GameObject canvas;
    private GameObject gameOverText;
    private GameObject tipPanel;
    public float speed = .45f;

    public enum STATE
    {
        idle,
        running,
        pause,
        win,
        computing,
        next,//
        ending,
        defeat,
        overing,
        restart,
        none,
    }
    public STATE state = STATE.idle;
    // Start is called before the first frame update
    void Start()
    {
        IniteEnv();
        SetVariable();

        Invoke("StartGame", 13.0f / speed / 25 + .25f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //Debug.Log("Pause!!!");
            //UnityEditor.EditorApplication.isPaused = true;
            //Time.timeScale = 0;
            //Application.Quit();
            SceneManager.LoadScene(0);
        }
        if (state == STATE.idle)
        {
            panel[0].transform.position += new Vector3(0, speed, 0);
            panel[1].transform.position -= new Vector3(0, speed, 0);
        }
        else if(state == STATE.win)
        {
            state = STATE.computing;
            //计算得分 score UI
            GameObject scorePanel = Instantiate(scorePanelPrefab, canvas.transform);
            scorePanel.transform.position = Vector3.zero;

        }
        else if(state == STATE.defeat)
        {
            state = STATE.overing;
            //game over UI
            gameOverText = Instantiate(gameOverTextPrefab, canvas.transform);
            gameOverText.transform.position = new Vector3(0, -8, 0);
        }
        else if(state == STATE.next)
        {
            state = STATE.ending;
            Invoke("LoadNextStage", 13.0f / speed / 60 + .25f);

        }
        else if(state == STATE.overing)
        {
            gameOverText.transform.position += new Vector3(0, .3f, 0);
            if (Mathf.Abs(gameOverText.transform.position.y - 2) < .4f)
            {
                state = STATE.restart;
            }
        }
        else if(state == STATE.restart)
        {
            state = STATE.none;
            Invoke("RestartGame", .6f);
        }
        else if(state == STATE.ending)
        {
            panel[0].transform.position -= new Vector3(0, speed, 0);
            panel[1].transform.position += new Vector3(0, speed, 0);
        }
    }

    private void StartGame()
    {
        state = STATE.running;
        //panel[0].SetActive(false);
        //panel[1].SetActive(false);
        stageText.SetActive(false);
    }

    //下一关
    private void LoadNextStage()
    {
        PlayerController pc = GetComponent<PlayerController>();
        for (int i = 0; i < 2; i++)
        {
            PlayerPrefs.SetInt("player" + i + "Life", pc.life[i] + pc.playerNum[i]);
            //if (pc.player[i] != null)
            //{
            //    PlayerPrefs.SetInt("player" + i + "Suffix", pc.player[i].GetComponent<TankController>().suffix);
            //}
            //else
            //{
            //    PlayerPrefs.SetInt("player" + i + "Suffix", 0);
            //}
        }
        int sceneNum = (PlayerPrefs.GetInt("sceneNum") + 1)%PlayerPrefs.GetInt("maxSceneNum");
        PlayerPrefs.SetInt("sceneNum", sceneNum);
        SceneManager.LoadScene(sceneNum);
    }

    //重新开始
    private void RestartGame()
    {
        PlayerPrefs.SetInt("sceneNum", 0);
        SceneManager.LoadScene(0);
    }

    //初始化
    private void IniteEnv()
    {
        //设置帧率
        Application.targetFrameRate = 35;
        //初始化黑色背景图片
        GameObject backGround = GameObject.Find("black");
        SpriteRenderer render = backGround.GetComponent<SpriteRenderer>();
        float scalex = 1300 / render.sprite.rect.width;
        float scaley = 1300 / render.sprite.rect.height;
        backGround.transform.localScale = new Vector3(scalex, scaley, 1);
        backGround.transform.position = Vector3.zero;
    }

    //设置变量
    private void SetVariable()
    {
        state = STATE.idle;
        canvas = GameObject.Find("Canvas");

        panel[0] = Instantiate(panelPrefab, canvas.transform);
        panel[1] = Instantiate(panelPrefab, canvas.transform);
        panel[0].transform.position = new Vector3(0, 0, 1);
        panel[1].transform.position = new Vector3(0, 0, 1);

        stageText = Instantiate(stageTextPrefab, canvas.transform);
        stageText.transform.position = Vector3.zero;
        stageText.GetComponent<Text>().text += "  " + PlayerPrefs.GetInt("sceneNum");

        tipPanel = Instantiate(tipPanelPrefab, GameObject.Find("Canvas/ui").transform);
        if (PlayerPrefs.GetInt("playerNum") == 1)
        {
            tipPanel.transform.Find("player2Info").gameObject.SetActive(false);
        }
        tipPanel.transform.position = new Vector3(9.63f, -3.25f, 0);

        
    }
}

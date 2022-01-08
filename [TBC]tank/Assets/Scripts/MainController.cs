using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    private int state = 0;
    public GameObject mainPlane;
    public GameObject grayPlane;
    private int maxSceneNum = 11; //
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0; //关闭垂直同步
        Application.targetFrameRate = 35;//设置帧率
        //mainPlane = GameObject.Find("Canvas/mainPlane");
        //grayPlane = GameObject.Find("Canvas/grayPlane");
        //Debug.Log(mainPlane.name);
        //mainPlane.transform.position = Vector3.zero;
        if(Application.platform == RuntimePlatform.Android)
        {
            GameObject.Find("Canvas/mainPanel/p2Button").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //Debug.Log("Pause!!!");
            //UnityEditor.EditorApplication.isPaused = true;
            //Time.timeScale = 0;
            Application.Quit();
            //SceneManager.LoadScene(0);
        }
        if (state == 0)
        {
            mainPlane.transform.position += new Vector3(0, .25f, 0);
            if(Mathf.Abs(mainPlane.transform.position.y) <= .26f)
            {
                state = 1;
                mainPlane.transform.position = Vector3.zero;
            }
        }
        else if(state == 2)
        {
            //...
            grayPlane.transform.position -= new Vector3(0, .45f, 0);
            if (Mathf.Abs(grayPlane.transform.position.y) <= .46f)
            {
                state = 1;
                grayPlane.transform.position = Vector3.zero;
                Invoke("LoadScene", .1f);
            }
        }
    }

    public void ButtonCliked(bool isTwoPlayer)
    {
        if(state == 2)
        {
            return;
        }
        state = 2;
        //PlayerPrefs.SetInt("ttt", 233);
        //SceneManager.LoadScene("Scenes/scene01");
        GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("playerNum", isTwoPlayer ? 2 : 1);
        PlayerPrefs.SetInt("sceneNum", 1);

        PlayerPrefs.SetInt("player0Life", 3);
        PlayerPrefs.SetInt("player1Life", isTwoPlayer ? 3 : 0);

        PlayerPrefs.SetInt("maxSceneNum", maxSceneNum);

        for(int i=0;i<2;i++) PlayerPrefs.SetInt("player" + i + "Suffix", 0);
        //SceneManager.LoadScene("Scenes/scene" + 1);
        //Debug.Log(PlayerPrefs.GetInt("playerNum"));
    }

    public void ExitButtonCliked()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();//退出游戏
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}

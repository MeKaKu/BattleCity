using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    public int pid = 0;
    private Text lifeText;
    // Start is called before the first frame update
    void Start()
    {
        lifeText = transform.Find("playerLifeText").gameObject.GetComponent<Text>();
        //Debug.Log(lifeText.text + ":::" + pid);
        //Debug.Log(PlayerPrefs.GetInt("player" + pid + "Life"));
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = " " + PlayerPrefs.GetInt("player" + pid + "Life");
        //Debug.Log(PlayerPrefs.GetInt("player" + pid + "Life"));
    }
}

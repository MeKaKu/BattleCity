using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("stageText").gameObject.GetComponent<Text>().text = " " + PlayerPrefs.GetInt("sceneNum");

        //Debug.Log(PlayerPrefs.GetFloat("sceneNum"));
        
    }

}

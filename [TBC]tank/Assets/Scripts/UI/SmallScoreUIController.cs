using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallScoreUIController : MonoBehaviour
{
    //public Vector3 pos;
    //private float standardScreenWidth = 400;
    //private Text text;
    // Start is called before the first frame update
    void Start()
    {
        //text = GetComponent<Text>();

        

        Invoke("DisText", 1);
    }

    private void DisText()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = "" + score;
        //float scalex = Screen.width / standardScreenWidth;
        //transform.localScale = new Vector3(scalex, scalex, 1);
        //transform.position = pos;
    }
}

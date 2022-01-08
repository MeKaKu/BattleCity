using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolController : MonoBehaviour
{
    public Sprite[] images;
    public int index = 0; //道具的类型
    public int maxCnt;
    public GameObject smallScorePrefab;
    private int deadTime = 450;
    private int cnt = 0;
    //private bool show = true;
    private SpriteRenderer render;
    // Start is called before the first frame update
    public void Start()
    {
        index = Random.Range(0, images.Length);
        render = GetComponent<SpriteRenderer>();
        render.sprite = images[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (++cnt > maxCnt)
        {
            render.enabled = !render.enabled;
            cnt = 0;
        }
        deadTime--;
        if(deadTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("tank"))
        {
            if (!collision.gameObject.GetComponent<PlayerInput>().useAI) {
                GameObject smallScore = Instantiate(smallScorePrefab, GameObject.Find("Canvas").transform);
                smallScore.transform.position = transform.position;
                smallScore.GetComponent<Text>().text = 500 + "";
                SendMessageUpwards("AddScore", 500);
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public Sprite[] images;
    public int[] intervals;
    public int len;
    public SpriteRenderer render { get; private set; }
    public int cnt{ get; private set; } = 0;
    //private int cnt = 0;
    public int index { get; private set; } = 0;
    public float ds { get; private set; } = 0.05f;
    // Start is called before the first frame update
    public void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = images[0];
        if(len == 5)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (index < len)
        {
            gameObject.transform.localScale += new Vector3(ds, ds, 0);
        }
        if (cnt == intervals[index])
        {
            index++;
            if (index >= len)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                render.sprite = images[index];
            }
        }
        cnt++;
    }
}

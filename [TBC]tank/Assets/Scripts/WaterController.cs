using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public Sprite[] images;
    private SpriteRenderer render;
    private int index = 0;
    private int maxCnt = 15;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (++count > maxCnt)
        {
            render.sprite = images[index ^= 1];
            count = 0;
        }
    }
}

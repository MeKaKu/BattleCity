using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkler : MonoBehaviour
{
    public Sprite[] images;
    private SpriteRenderer render;
    private int index = 0;
    public int maxCnt = 1;
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
            index = (index + 1) % images.Length;
            render.sprite = images[index];
            count = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Sprite[] images;
    private int index = 0;
    private SpriteRenderer render;
    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (index == 0 && LayerMask.LayerToName(collision.gameObject.layer).Equals("bullet"))
        {
            render.sprite = images[index ^= 1];
            SendMessageUpwards("BaseDestroy");
        }
    }
}

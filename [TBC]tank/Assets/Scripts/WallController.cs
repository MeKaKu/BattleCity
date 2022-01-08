using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Wall Collision :: " + collision.gameObject.name);
        if(LayerMask.LayerToName(collision.gameObject.layer).Equals("bullet"))
        {
            //SendMessageUpwards("MyOnTriggerEnter", collision);
            BulletController bulletCon = collision.gameObject.GetComponent<BulletController>();
            float width = 0.125f;
            float height = 0.25f;
            if(bulletCon.level >= 1)
            {
                width *= 2.0f;
            }
            if (bulletCon.dir <= 1)
            {
                float temp = width;
                width = height;
                height = temp;
            }
            Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(width, height), 0, LayerMask.GetMask("redwall"));
            //if(!bulletCon.belongEnemy) Debug.Log(">_<::" + colliders.Length);
            foreach (Collider2D collider in colliders)
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}

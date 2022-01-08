using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteWallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Wall Collision :: " + collision.gameObject.layer);
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("bullet"))
        {
            BulletController bulletCon = collision.gameObject.GetComponent<BulletController>();
            if(bulletCon.level == 1)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

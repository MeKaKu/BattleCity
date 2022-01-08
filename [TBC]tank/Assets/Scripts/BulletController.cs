using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Sprite[] images; //贴图列表
    public float unitLen = 18; //单位长度
    private SpriteRenderer render; //贴图renderer
    public float stepLen = 0.1f; //移动步长
    private float deltaLen = 0.1f;
    public int level = 0;
    public int dir = 0;
    private Vector3[] vecs = { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0) }; //方向向量 上下左右
    public GameObject bombPrefab;
    public bool belongEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        float scalex = unitLen / render.sprite.rect.width;
        //float scaley = unitLen / render.sprite.rect.height;
        render.transform.localScale = new Vector3(scalex, scalex, 1);

        render.sprite = images[dir];
    }

    // Update is called once per frame
    public void Update()
    {
        transform.position += vecs[dir] * ((level>0?deltaLen:0) + stepLen);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("tank"))
        {
            //if (belongEnemy == collision.gameObject.GetComponent<PlayerInput>().useAI)
            //{
            //    return;
            //}
            if(belongEnemy && collision.gameObject.GetComponent<PlayerInput>().useAI)
            { //敌人的子弹不会击中敌人
                return;
            }
        }
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("bullet"))
        {//子弹相互抵消不产生爆炸
            //敌人的子弹不相互抵消？？？
            if (belongEnemy && collision.gameObject.GetComponent<BulletController>().belongEnemy)
            {
                return;
            }
        }
        else
        {
            Bomb();
        }
        //Debug.Log("Bullet Collision!");
        Destroy(gameObject);
    }

    private void Bomb()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
        //bomb.GetComponent<BombController>().len = 3;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 12;
    Animator bulletAinmator;//关联动画器
    bool canMove = true;//子弹是否可以移动

    void Start()
    {
        bulletAinmator = GetComponent<Animator>();
    }

    void Update()
    {
        //向左发射子弹
        if (canMove)//修改成子弹可以移动的时候再移动
        {
            this.transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//子弹碰到地面、主角后销毁
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            canMove = false;//关闭子弹移动，要不然子弹边移动边销毁动画很怪

            // ⭐增加判空！没有Animator组件就不要调用SetTrigger，防止空引用报错
            if (bulletAinmator != null)
            {
                bulletAinmator.SetTrigger("Destory");//触发销毁动画
            }
            else
            {
                //如果子弹没有动画组件，直接销毁，兜底逻辑
                Destroy(this.gameObject);
            }
        }
    }

    public void Destroy()//放在销毁动画最后一帧再销毁（动画事件调用这个函数）
    {
        Destroy(this.gameObject);
    }
}

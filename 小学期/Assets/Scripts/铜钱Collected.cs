using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 铜钱Collected : MonoBehaviour
{// Start is called before the first frame update
    Animator ainm;//新建变量：动画器
    void Start()
    {
        ainm = GetComponent<Animator>();//赋值，让他等于本脚本附加对象的动画器
    }

    // Update is called once per frame
    void Update()
    {

    }

    //使用OnTriggerEnter2D方法（当本脚本所附加对象与其他任何物体发生2d触发时会执行的指令）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))//如果发生碰撞的对象标签是"Player"
        {
            ainm.SetTrigger("isCollected");//让动画器触发碰撞参数"isCollected"
        }
    }

    void DestroySelf()//新建方法，用于销毁自己
    {
        Destroy(gameObject);
    }
}

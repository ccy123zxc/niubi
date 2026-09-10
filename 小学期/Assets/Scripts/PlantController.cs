using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public GameObject bullet;//子弹预制体
    public Transform attackPoint;//子弹出生点
    public IsDead deadPoint;//用于获取deadpoint的IsDead脚本的信息

    [Header("音效")]
    public AudioClip shootSound;//射击音效
    public AudioClip deadSound;//死亡音效
    AudioSource plantSound;//音频源

    bool isPlantDead;//判断豌豆射手是否死亡
    bool deadTriggerFired = false; //死亡锁：确保死亡动画只触发一次
    bool inAttack = false;//用来判断角色是否在攻击范围内
    bool attackCD = false;//用来判断豌豆射手攻击是否CD
    Animator plantAnimator;//用来关联动画器

    void Start()
    {
        plantAnimator = GetComponent<Animator>();//动画器赋值
        plantSound = GetComponent<AudioSource>();//音频源赋值
    }

    void Update()
    {
        isPlantDead = deadPoint.isDead;//读取死亡状态

        // 如果已经死亡，并且还没播放过死亡动画
        if (isPlantDead && !deadTriggerFired)
        {
            plantAnimator.SetTrigger("Dead");//运行死亡动画
            DeadSound();//播放死亡声音
            deadTriggerFired = true; 
        }

        if (isPlantDead) return;
    }

    private IEnumerator WaitAndAttack()
    {
        if (attackCD || !inAttack || isPlantDead) yield break;

        plantAnimator.SetTrigger("Attack");//运行攻击动画
        attackCD = true;//打开攻击CD
        yield return new WaitForSeconds(1);//CD为2秒
        attackCD = false;//2秒后关闭CD

        //玩家还在范围内，继续循环攻击
        if (inAttack && !isPlantDead)
        {
            StartCoroutine(WaitAndAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//判断角色是否在攻击范围内
    {
        if (collision.CompareTag("Player") && !isPlantDead)
        {
            inAttack = true;
            StartCoroutine(WaitAndAttack());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//判断角色是否在攻击范围外
    {
        if (collision.CompareTag("Player"))
        {
            inAttack = false;
        }
    }

    public void BulletShoot()//编写发射子弹事件，后面要放在发射动画帧运行
    {
        GameObject bult = Instantiate(bullet);//实例化子弹
        ShootSound();//播放射击声音
        bult.transform.position = attackPoint.position;//子弹位置在attackpoint
        bult.transform.eulerAngles = attackPoint.eulerAngles;//子弹角度和attackpoint一致
    }

    public void KillPlant()
    {
        Destroy(this.gameObject);//销毁自己
    }

    void ShootSound()
    {
        plantSound.clip = shootSound;
        plantSound.Play();
    }

    void DeadSound()
    {
        plantSound.clip = deadSound;
        plantSound.Play();
    }
}

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

    bool isPlantDead;
    bool inAttack = false;//用来判断角色是否在攻击范围内
    bool attackCD = false;//用来判断豌豆射手攻击是否CD
    Animator plantAnimator;//用来关联动画器

    // Start is called before the first frame update
    void Start()
    {
        plantAnimator = GetComponent<Animator>();//动画器赋值
        plantSound = GetComponent<AudioSource>();//音频源赋值
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackCD)//如果攻击没CD则运行攻击
        {
            StartCoroutine(WaitAndAttack());
        }

        isPlantDead = deadPoint.isDead;
        if (isPlantDead)
        {
            if (plantAnimator != null)
            {
                plantAnimator.SetTrigger("Dead");
            }
            DeadSound();
        }
    }

    private IEnumerator WaitAndAttack()
    {
        if (inAttack)//如果在攻击范围内
        {
            plantAnimator.SetTrigger("Attack");//运行攻击动画
            attackCD = true;//打开攻击CD
            yield return new WaitForSeconds(2);//CD为2秒（可自行修改攻击间隔）
            attackCD = false;//关闭攻击CD
        }
    }

    public void BulletShoot()//编写发射子弹事件，后面要放在发射动画帧运行
    {
        GameObject bult = Instantiate(bullet);
        bult.transform.position = attackPoint.position;
        bult.transform.eulerAngles = attackPoint.eulerAngles;
    }

    private void OnTriggerEnter2D(Collider2D collision)//判断角色进入攻击范围
    {
        if (collision.CompareTag("Player"))
        {
            inAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//判断角色是否在攻击范围外
    {
        if (collision.CompareTag("Player"))
        {
            inAttack = false;
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    void ShootSound()
    {
        if (plantSound != null && shootSound != null)
        {
            plantSound.clip = shootSound;
            plantSound.Play();
        }
    }

    void DeadSound()
    {
        if (plantSound != null && deadSound != null)
        {
            plantSound.clip = deadSound;
            plantSound.Play();
        }
    }
}

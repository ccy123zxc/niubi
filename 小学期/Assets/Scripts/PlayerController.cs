using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//使用新的命名空间


public class PlayerController : MonoBehaviour
{
    [Header("移动")]//4.新建一个标题栏：移动，表示这些变量是移动相关的变量
    public float moveSpeed;//1.声明变量，移动速度

    [Header("跳跃")]//5.新建跳跃相关的变量
    public float jumpForce;//5.声明变量：跳跃强度
    bool canDoubleJump;//12.声明变量：是否能进行二次跳跃

    [Header("组件")]//4.新建一个标题栏：组件，表示这些变量是组件相关的变量
    public Rigidbody2D PlayerRigidbody;//2.声明变量：刚体2D组件类型的变量，
                                       //方便我们获取使用或修改刚体2D组件里的信息
    public Text healthText;//新建变量，让他等于生命值UI.Text组件
    public Text keyText;
    [Header("key收集")]
    public int keyCount = 0;
    public int needToTalKey = 3;
    Collider2D playerCollider;//角色碰撞器
    [Header("动画器")]
    Animator anim;//新建标题，并且声明变量：私密的动画器【anim】
    [Header("地面检测")]//7.新建标题栏：地面检测
    bool isGrounded;//9.声明私密变量：是否碰撞到地面
    public Transform groundCheckpoint;//声明变量：地面碰撞检测点
    public LayerMask GroundLayer;//8.声明变量：地面图层
    [Header("UI控制")]
    public GameObject gameOverUI;
    public GameObject gameEndUI;

    [Header("音频效果")]
    public AudioClip jumpSound;
    public AudioClip collectedSound;
    public AudioClip hitSound;
    public AudioClip deadSound;
    public AudioClip winSound;
    AudioSource playerSound;

    int health = 3;//新建变量生命值
    int key = 0;
    bool hitCD = false;//新建变量：受伤CD
    bool isInSpikes = false;//新建变量：代表是否在陷阱中的状态
    bool canWin = false;

    void Start()
    {
        anim = GetComponent<Animator>();//在start方法里，给anim赋值。让anim等于player的动画器。
        playerCollider = GetComponent<CapsuleCollider2D>();//变量赋值
        gameOverUI.gameObject.SetActive(false);//在游戏开始时，关闭激活UI面板
        gameEndUI.gameObject.SetActive(false);
        playerSound = GameObject.Find("PlayerSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health > 0)
        {
            PlayerMove();
        }
        isGrounded = Physics2D.OverlapCircle(groundCheckpoint.position, .2F, GroundLayer);

        if (isGrounded)
            canDoubleJump = true;

        //关联动画器里的参数，让moveSpeed=角色刚体x轴的移动速度。isGround=代码中的isGrounded变量。
        anim.SetFloat("moveSpeed", Mathf.Abs(PlayerRigidbody.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
        //关联动画器"jumpSpeed"参数=角色y轴移动速度。这里不需要使用绝对值，就不需要使用Mathf.Abs方法
        //补充：Math.Abs是取数字绝对值的方法。因为moveSpeed我们只需要读取正数，所以使用了这个方法
        anim.SetFloat("jumpSpeed", PlayerRigidbody.velocity.y);

        healthText.text = "x " + health;//让组件里的text内容为x空格+生命值
                                        //双引号里面是小写x和空格
        keyText.text = key + " / 3";
        if (!hitCD)//如果在受伤CD外
        {
            StartCoroutine(WaitAndHit());//使用StartCoroutine语句执行减生命值协程方法
        }
    }
    //在Update外面新建触发2d方法。让主角与香蕉发生碰撞时，生命值+1
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("通关key"))
        {
            key = key + 1;
        }
        if (collision.CompareTag("铜钱"))
        {
            health = health + 1;
            CollectedSound();
        }
        if (collision.CompareTag("Spikes"))//如果与陷阱尖刺发生碰撞
        {
            isInSpikes = true;//陷阱中状态为真
        }
        if (collision.CompareTag("通关key"))
        {
            keyCount++;
            Destroy(collision.gameObject);
            if (keyCount >= needToTalKey)
            {
                canWin = true;
            }
            CollectedSound();
        }
        if (collision.CompareTag("End"))
        {
            if (canWin)
            {
                Destroy(PlayerRigidbody);
                WinSound();
                gameEndUI.gameObject.SetActive(true);
            }
        }
    }
    //新建碰撞离开2D方法
    //OnTriggerEnter2D：是当碰撞器碰撞那一瞬间触发的一次指令
    //OnTriggerExit2D:是碰撞器离开时那一瞬间触发的一次指令
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))//如果离开尖刺
        {
            isInSpikes = false;//陷阱中状态为假
            hitCD = false;//将受伤CD关闭
        }
    }

    //新建协程（IEnumerator）方法WaitAndHit（）
    //让角色在碰撞到陷阱时受伤一次，并且每在陷阱呆一秒就受伤一次
    //协程可以在特定的时间点暂停自己的执行，然后再稍后的时间点恢复执行
    //例如，可以使用协程来实现一些需要等待几秒钟或数分钟才能完成的操作，而不必阻塞主线程，时程序保持响应
    private IEnumerator WaitAndHit()
    {
        if (isInSpikes)
        {
            health -= 1;
            anim.SetTrigger("Hit");//在减生命值的同时，运行一次受伤动画
            HitSound();
            health = (health < 0) ? 0 : health;//使用三元运算符，来判断health受否小于0
                                               //如果是，将其设置为0，否则保持不变
            hitCD = true;
            yield return new WaitForSeconds(1);
            hitCD = false;
        }
    }

    void PlayerDeath()
    {
        if (health == 0)
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, 3);//让角色跳一下死
            Destroy(playerCollider);//销毁主角碰撞体
            anim.SetTrigger("Death");//打开死亡动画
            gameOverUI.gameObject.SetActive(true);//让主角死的时候，打开gameOver面板
            DeadSound();
        }
    }

    void PlayerMove()
    {
        PlayerRigidbody.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), PlayerRigidbody.velocity.y);

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheckpoint.position, .2F, GroundLayer);

        if (isGrounded)
            canDoubleJump = true;

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpForce);
                JumpSound();
            }
            else
            {
                if (canDoubleJump)
                {
                    PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpForce);
                    canDoubleJump = false;
                    JumpSound();
                }
            }
        }
    }
    void JumpSound()
    {
        playerSound.clip = jumpSound;
        playerSound.Play();
    }
    void CollectedSound()
    {
        playerSound.clip = collectedSound;
        playerSound.Play();
    }
    void HitSound()
    {
        playerSound.clip = hitSound;
        playerSound.Play();
    }
    void DeadSound()
    {
        playerSound.clip = deadSound;
        playerSound.Play();
    }
    void WinSound()
    {
        playerSound.clip = winSound;
        playerSound.Play();
    }
}

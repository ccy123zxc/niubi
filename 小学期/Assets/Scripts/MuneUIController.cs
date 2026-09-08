using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MuneUIController : MonoBehaviour
{
    RectMask2D UImask;//声明变量：遮罩组件
    float paddingNum = 750;//声明变量，遮罩渐变开始值

    Button againBtn;//声明变量：两个按钮
    Button quitBtn;

    private void Awake()
    {
        againBtn = transform.GetChild(0).GetComponent<Button>();//代表游戏开始按钮是第0位子级，并且获取子级的Button组件
        quitBtn = transform.GetChild(1).GetComponent<Button>();//同上
        againBtn.onClick.AddListener(PlayGame);//当按钮被按下时，运行方法PlayAgain
        quitBtn.onClick.AddListener(QuitGame);
    }

    void Start()
    {
        UImask = GetComponent<RectMask2D>();//赋值
    }

    void Update()
    {
        UImask.padding = new Vector4(0, paddingNum, 0, 0);//让Bottom的数值=paddingNum
        paddingNum -= 1;//让paddingNum数值慢慢变小
        paddingNum = (paddingNum < 0) ? 0 : paddingNum;//让paddingNum最小值为0
    }

    void PlayGame()
    {
        SceneManager.LoadSceneAsync("first");
    }

    void QuitGame()
    {
        Application.Quit();//退出游戏软件
        Debug.Log("退出游戏");//由于在引擎内测试可能无法退出游戏软件，所以在控制台输出让我们知道游戏退出了。
    }
}
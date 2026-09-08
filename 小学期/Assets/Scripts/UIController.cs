using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    RectMask2D UImask;
    float paddingNum = 750;

    Button againBtn;
    Button quitBtn;
    Button menuBtn;


    private void Awake()
    {
        againBtn = transform.GetChild(0).GetComponent<Button>();
        quitBtn = transform.GetChild(1).GetComponent<Button>();
        menuBtn = transform.GetChild(2).GetComponent<Button>();

        againBtn.onClick.AddListener(PlayAgain);
        quitBtn.onClick.AddListener(QuitGame);
        menuBtn.onClick.AddListener(GoToMenu);
    }
    void Start()
    {
        UImask = GetComponent<RectMask2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UImask.padding = new Vector4(0, paddingNum, 0, 0);
        paddingNum = paddingNum - 1;
        paddingNum = (paddingNum < 0) ? 0 : paddingNum;
    }

    void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void QuitGame()
    {
        Application.Quit();
        Debug.Log("ÍË³öÓÎÏ·");
    }
    void GoToMenu()
    {
        SceneManager.LoadSceneAsync("Main Mune");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//新加命名空间;
using UnityEngine.EventSystems;//添加命名空间
using System;

public class UISoundController : MonoBehaviour, IPointerEnterHandler//使用接口
{
    public AudioClip buttonSelect;//用来放置音频按钮被选择时的声音
    public AudioClip buttonPress;//用来放置音频按钮被按下时的声音

    AudioSource audioSource;//代表音频源组件
    Button button;

    void Awake()//在awake里赋值
    {
        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        button.onClick.AddListener(SoundPlay);//当按钮被按下时执行SoundPlay()方法
    }

    private void SoundPlay()//播放按下音频
    {
        audioSource.clip = buttonPress;
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)//当按钮被鼠标进入时，播放进入音频
    {
        audioSource.clip = buttonSelect;
        audioSource.Play();
    }
}
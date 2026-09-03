using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip buttonClickSfx;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    public void PlayButtonClick()
    {
        if (buttonClickSfx == null)
        {
            Debug.LogError("音效未赋值！");
            return;
        }
        Debug.Log("播放按钮音效");
        audioSource.PlayOneShot(buttonClickSfx);
    }
}
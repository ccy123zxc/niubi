using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartMenu()
    {
        SoundManager.Instance.PlayButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
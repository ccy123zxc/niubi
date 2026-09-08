using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TinyBtnSfx : MonoBehaviour
{
    private Button _btn;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(PlaySfx);
    }

    void PlaySfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonClick();
        }
    }

    private void OnDestroy()
    {
        if (_btn != null)
        {
            _btn.onClick.RemoveListener(PlaySfx);
        }
    }
}
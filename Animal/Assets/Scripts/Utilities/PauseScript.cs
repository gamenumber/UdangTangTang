using UnityEngine;
using UnityEngine.UI;

public class PauseResume : MonoBehaviour
{
    private bool _ispaused = false;
    public Button PauseResumeButton;
    public Text ButtonText;
    public Sprite PauseSprite;
    public Sprite ResumeSprite;

    private void Start()
    {
        PauseResumeButton.onClick.AddListener(TogglePause);
        UpdateButtonImage();
    }

    public void TogglePause()
    {
        _ispaused = !_ispaused; // 퍼즈와 재개를 번갈아가며 변경

        if (_ispaused)
        {
            Time.timeScale = 0;
            // 일시정지 시 해야 할 작업들을 여기에 추가할 수 있습니다.
        }
        else
        {
            Time.timeScale = 1;
            // 게임 계속하기 시 해야 할 작업들을 여기에 추가할 수 있습니다.
        }

        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        PauseResumeButton.image.sprite = _ispaused ? ResumeSprite : PauseSprite;
    }
}

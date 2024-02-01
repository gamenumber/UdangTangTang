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
        _ispaused = !_ispaused; // ����� �簳�� �����ư��� ����

        if (_ispaused)
        {
            Time.timeScale = 0;
            // �Ͻ����� �� �ؾ� �� �۾����� ���⿡ �߰��� �� �ֽ��ϴ�.
        }
        else
        {
            Time.timeScale = 1;
            // ���� ����ϱ� �� �ؾ� �� �۾����� ���⿡ �߰��� �� �ֽ��ϴ�.
        }

        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        PauseResumeButton.image.sprite = _ispaused ? ResumeSprite : PauseSprite;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DarkenImage : MonoBehaviour
{
	public Image image; // ������ Image ������Ʈ�� �ν����Ϳ��� �Ҵ����ּ���.
	public Slider darknessSlider; // ��ӱ⸦ ������ Slider ������Ʈ�� �ν����Ϳ��� �Ҵ����ּ���.

	private void Start()
	{
		// �����̴��� ���� ��ӱ⿡ �ݿ�
		darknessSlider.value = GetNormalizedDarkness(image.color);

		// �����̴� �� ���� �� ��ӱ� ���� �Լ� ȣ��
		darknessSlider.onValueChanged.AddListener(UpdateDarkness);
	}

	private void UpdateDarkness(float sliderValue)
	{
		// �����̴� ������ ��ӱ⸦ �����Ͽ� �̹����� ���� ������Ʈ
		SetImageDarkness(sliderValue);
	}

	private void SetImageDarkness(float darkness)
	{
		Color imageColor = new Color(darkness, darkness, darkness, image.color.a);
		image.color = imageColor;
	}

	// �̹����� R, G, B ���� ���������� ����Ͽ� ��ӱ⸦ ����ϴ� �Լ�
	private float GetNormalizedDarkness(Color color)
	{
		return (color.r + color.g + color.b) / 3.0f;
	}
}

using UnityEngine;
using UnityEngine.UI;

public class DarkenImage : MonoBehaviour
{
	public Image image; // 조절할 Image 컴포넌트를 인스펙터에서 할당해주세요.
	public Slider darknessSlider; // 어둡기를 조절할 Slider 컴포넌트를 인스펙터에서 할당해주세요.

	private void Start()
	{
		// 슬라이더의 값을 어둡기에 반영
		darknessSlider.value = GetNormalizedDarkness(image.color);

		// 슬라이더 값 변경 시 어둡기 조절 함수 호출
		darknessSlider.onValueChanged.AddListener(UpdateDarkness);
	}

	private void UpdateDarkness(float sliderValue)
	{
		// 슬라이더 값으로 어둡기를 조절하여 이미지의 색상 업데이트
		SetImageDarkness(sliderValue);
	}

	private void SetImageDarkness(float darkness)
	{
		Color imageColor = new Color(darkness, darkness, darkness, image.color.a);
		image.color = imageColor;
	}

	// 이미지의 R, G, B 값을 종합적으로 사용하여 어둡기를 계산하는 함수
	private float GetNormalizedDarkness(Color color)
	{
		return (color.r + color.g + color.b) / 3.0f;
	}
}

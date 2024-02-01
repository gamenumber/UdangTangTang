
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	public GameObject Page1;
	public GameObject Page2;
	public GameObject Page3;

	public GameObject Option;

	public GameObject Title;
	public GameObject Explainbutton;

	public void OnClickGameQuit()
	{
		Application.Quit();
	}

	public void OnClickGameStart()
	{
		GlobalManager.Instance.LoadScene("StageSelect");
	}

	public void OnClickgoingMain()
	{
		Page1.gameObject.SetActive(false);
		Page2.gameObject.SetActive(false);
		Page3.gameObject.SetActive(false);
		Option.gameObject.SetActive(false);
		Title.gameObject.SetActive(true);
		Explainbutton.gameObject.SetActive(true);
	}


	public void OnClickPage1()
	{
		Explainbutton.gameObject.SetActive(false);
		Title.gameObject.SetActive(false);
		Page1.gameObject.SetActive(true);
		Page2.gameObject.SetActive(false);
		Page3.gameObject.SetActive(false);
	}

	public void OnClickPage2()
	{
		Page1.gameObject.SetActive(false);
		Page2.gameObject.SetActive(true);
		Page3.gameObject.SetActive(false);
	}

	public void OnClickPage3()
	{
		Page1.gameObject.SetActive(false);
		Page2.gameObject.SetActive(false);
		Page3.gameObject.SetActive(true);
	}

	public void OnClickoption()
	{
		Option.gameObject.SetActive(true);
		Title.gameObject.SetActive(false);
		Explainbutton.gameObject.SetActive(false);
	}

}

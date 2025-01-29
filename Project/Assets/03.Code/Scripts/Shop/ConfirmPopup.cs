using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour
{
	[Header("참조가 비어있으면 Reset을 눌러주세요.")]
	public Button yesButton;
	public Button noButton;
	public SuccessPopup successPopup;
	public FailPopup failPopup;

	private void Reset()
	{
		yesButton = transform.Find("Buttons/YesButton").GetComponent<Button>();
		noButton = transform.Find("Buttons/NoButton").GetComponent<Button>();
		successPopup = FindObjectOfType<SuccessPopup>(true);
		failPopup = FindObjectOfType<FailPopup>(true);
	}
	
	private void OnEnable()
	{
		yesButton.onClick.AddListener(() =>
		{
			ClosePopup();
		});
		noButton.onClick.AddListener(()=> 
		{
			ClosePopup();
		});
	}

	private void OnDisable()
	{
		yesButton.onClick.RemoveAllListeners();
		noButton.onClick.RemoveAllListeners();
	}

	public void OpenPopup()
	{
		gameObject.SetActive(true);
	}
	
	public void ClosePopup()
	{
		gameObject.SetActive(false);
	}
}

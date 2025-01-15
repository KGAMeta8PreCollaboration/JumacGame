using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour
{
	public LocalPlayer localPlayer;
	
	private Button _button;
	
	private void Start()
	{
		localPlayer = FindObjectOfType<LocalPlayer>();
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		print("Jump");
		if (localPlayer == null)
			localPlayer = FindObjectOfType<LocalPlayer>();
		localPlayer?.Jump();
	}
}

using Firebase.Database;
using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
	private TextMeshProUGUI _goldText;

	private void Awake()
	{
		_goldText = GetComponentInChildren<TextMeshProUGUI>();
		PullGold();
		GameManager.Instance.FirebaseManager.LogInUsersRef.Child($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").ValueChanged += GoldChanged;
	}
	
	private async void PullGold()
	{
		DataSnapshot goldData = await GameManager.Instance.FirebaseManager.LogInUsersRef.Child
			($"{GameManager.Instance.FirebaseManager.User.UserId}/gold").GetValueAsync(); 
		_goldText.text = goldData.Value.ToString();
	}	
	
	private void GoldChanged(object sender, ValueChangedEventArgs e)
	{
		_goldText.text = e.Snapshot.Value.ToString();
	}
}

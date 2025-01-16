using TMPro;
using UnityEngine;

public class LeaderBoardPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI rankText;

	public void SetData(string name, int score, int rank)
	{
		nameText.text = name;
		scoreText.text = score.ToString();
		rankText.text = rank.ToString();
	}
}

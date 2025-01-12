using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame.RGLight
{
	public class RGLightManager : Singleton<RGLightManager>
	{
		[SerializeField] private GameObject _introPanel;
		public float introTime;

		protected override void Awake()
		{
			base.Awake();
			StartCoroutine(IntroCoroutine());
		}

		private IEnumerator IntroCoroutine()
		{
			_introPanel.SetActive(true);
			yield return new WaitForSeconds(introTime);
			_introPanel.SetActive(false);
		}

		private void EndGame()
		{
			SceneManager.LoadScene("SEOLobbyTest");
		}

		public void GameResult(bool isSuccess)
		{
			if (isSuccess)
			{
				OnSuccess();
			}
			else
			{
				OnDefeat();
			}
		}

		private void OnSuccess()
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("����", "��Ʈ���� ����ֳ�", EndGame);
		}

		private void OnDefeat()
		{
			PopupManager.Instance.PopupOpen<AlarmPopup>().SetPopup("����", "��Ʈ���� �����ϳ�", EndGame);
		}
	}
}

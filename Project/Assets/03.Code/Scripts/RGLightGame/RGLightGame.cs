using Minigame.RGLight;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RGLightGame : MonoBehaviour
{
	public Action endSentenceAction;

	public RectMask2D mask;
	public float startMaskPos;
	public float endMaskPos;
	[SerializeField] private GameObject sentencePrefab;
	private GameObject _sentence;

	public RGLightManager RGLightManager { get; private set; }

	public IEnumerator ControllReadSentence()
	{
		yield return new WaitForSeconds(5f);
		StartCoroutine(ReadSentence2());
	}

	public IEnumerator ReadSentence2()
	{
		float duration = RGLightManager.player.PlayerDistanceTracker.GetSentenceSpeed();
		print("대사 말하기 속도" + duration);

		float[] letterPositions = { 160, 160, 170, 150, 230, 140, 180, 170, 170, 150 };
		float[] weight = { 1, 1, 3f, 2f, 1.5f, 1, 1, 1, 1, 3f };

		float totalWeight = 0;
		foreach (float w in weight) totalWeight += w;

		float baseDuration = duration / totalWeight;
		float[] letterDurations = new float[weight.Length];
		for (int i = 0; i < weight.Length; i++)
			letterDurations[i] = baseDuration * weight[i];

		Vector4 startPadding = new Vector4(startMaskPos, 0, 0, 0);
		mask.padding = startPadding;
		_sentence.SetActive(true);

		Sfx sfx = RGLightManager.player.PlayerDistanceTracker.GetYoungheeSentenceSpeed();
		AudioManager.Instance.PlaySfx(sfx);
		print($"현재 영희 대사 말하기 속도 : {sfx}");

		float currentLeft = startMaskPos;

		for (int i = 0; i < letterPositions.Length; i++)
		{
			float targetLeft = currentLeft + letterPositions[i];
			float elapsedTime = 0f;

			while (elapsedTime < letterDurations[i])
			{
				elapsedTime += Time.deltaTime;

				float progress = Mathf.Clamp01(elapsedTime / letterDurations[i]);
				mask.padding = new Vector4(Mathf.Lerp(currentLeft, targetLeft, progress), 0, 0, 0);

				yield return null;
			}

			currentLeft = targetLeft;
		}

		mask.padding = new Vector4(endMaskPos, 0, 0, 0);
		_sentence.SetActive(false);

		if (!RGLightManager.IsEndGame) endSentenceAction?.Invoke();
	}

	public void Init(RGLightManager manager)
	{
		RGLightManager = manager;

		Transform canvas = GameObject.Find("Canvas").transform;
		_sentence = Instantiate(sentencePrefab, canvas);
		_sentence.SetActive(false);
		mask = _sentence.transform.Find("Mask").GetComponent<RectMask2D>();
	}
}


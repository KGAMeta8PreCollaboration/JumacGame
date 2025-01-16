using System.Collections;
using UnityEngine;

public class Raser : Skill
{
    public float circleSrcTime;
    public float circleSrcMax;

    [SerializeField] private GameObject _circleIndicateSrcPrefab;
    [SerializeField] private GameObject _circleIndicateDesPrefab;

    private GameObject _circleIndicateSrc;
    private Younghee _younghee;

    public override void UseSkill()
    {
        SpawnCircle(_younghee.RGLightManager.player.PlayerRay.CalcSpawnPoint());
    }

    public void SpawnCircle(Vector3 spawnPoint)
    {
        _circleIndicateSrc = Instantiate(_circleIndicateSrcPrefab, spawnPoint, Quaternion.identity);
        StartCoroutine(CircleSrcCoroutine());
    }

    private IEnumerator CircleSrcCoroutine()
    {
        float startTime = 0f;
        Vector3 startSize = Vector3.zero;
        Vector3 maxSize = new Vector3(circleSrcMax, circleSrcMax, circleSrcMax);

        while (startTime < circleSrcTime)
        {
            startTime += Time.deltaTime;

            float progress = Mathf.Clamp01(startTime / circleSrcTime);

            _circleIndicateSrc.transform.localScale = Vector3.Lerp(startSize, maxSize, progress);

            yield return null;
        }

        _circleIndicateSrc.transform.localScale = maxSize;
    }

    public override void Init(Younghee younghee)
    {
        _younghee = younghee;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : Skill
{
    [Header("Settings")]
    public float squareGrowTime = 1f;
    public float squareMaxHeight = 5.7f;
    public float squareWidth = 1f;

    [Header("Prefabs")]
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private GameObject _effectPrefab;

    private Younghee _younghee;

    public override void Init(Younghee younghee)
    {
        _younghee = younghee;
    }

    public override void UseSkill()
    {
        List<Vector3> outerCellCenters = _younghee.RGLightManager.CageManager.cage.GetOuterCellCenters();

        float topZ = _younghee.RGLightManager.CageManager.cage.transform.position.z +
                     _younghee.RGLightManager.CageManager.cage.height / 2f;
        List<Vector3> topPoints = outerCellCenters.FindAll(point => Mathf.Approximately(point.z, topZ));

        List<Vector3> oddPoints = new List<Vector3>
        {
            topPoints[0],
            topPoints[2],
            topPoints[4]
        };
        List<Vector3> evenPoints = new List<Vector3>
        {
            topPoints[1],
            topPoints[3]
        };

        List<Vector3> selectedPoints;

        float random = UnityEngine.Random.Range(0, 100f);
        if (random <= 50) selectedPoints = oddPoints;
        else selectedPoints = evenPoints;

        StartCoroutine(ExecuteSkillAtPoints(selectedPoints));
    }

    private IEnumerator ExecuteSkillAtPoints(List<Vector3> points)
    {
        bool skillComplete = false;

        foreach (var point in points)
        {
            StartCoroutine(StrikeSquare(point, () =>
            {
                skillComplete = true;
            }));
        }

        yield return new WaitUntil(() => skillComplete == true);
        _younghee.skillDone.Add(true);
    }

    private IEnumerator StrikeSquare(Vector3 target, Action onComplete)
    {
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject square = Instantiate(_squarePrefab, target, rotation);

        //yield return StartCoroutine(ExpandSquare(square, squareWidth, squareMaxHeight, squareGrowTime));
        //test
        yield return new WaitForSeconds(squareGrowTime);
        //test
        Destroy(square);

        Vector3 NewP = new Vector3(target.x, 0.8f, target.z);
        GameObject effect = Instantiate(_effectPrefab, NewP, Quaternion.Euler(-90f, 0f, 0f));
        AudioManager.Instance.PlaySfx(Sfx.RGLPGun);


        yield return new WaitForSeconds(1.5f);
        Destroy(effect);

        onComplete?.Invoke();
    }

    private IEnumerator ExpandSquare(GameObject square, float squareWidth, float squareMaxHeight, float squareGrowTime)
    {
        square.transform.localScale = new Vector3(squareWidth, 0f, 1f);
        Vector3 maxSize = new Vector3(squareWidth, squareMaxHeight, 1f);

        float elapsedTime = 0f;

        while (elapsedTime < squareGrowTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / squareGrowTime);
            float newHeight = Mathf.Lerp(0f, squareMaxHeight, progress);
            square.transform.localScale = new Vector3(squareWidth, newHeight, 1f);

            yield return null;
        }

        square.transform.localScale = maxSize;

        yield return new WaitForSeconds(1f);
    }
}


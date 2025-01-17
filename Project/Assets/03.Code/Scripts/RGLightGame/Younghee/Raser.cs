using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Raser : Skill
{
    [Header("Settings")]
    public float circleSrcTime = 1f;
    public float circleSrcMax = 1f;
    public float circleSpawnInterval;
    public int gridSize = 3;
    public float cageWidth = 30f;
    public float cageHeight = 30f;
    public int strikeCount = 3;

    [Header("Prefabs")]
    [SerializeField] private GameObject _circleIndicateDesPrefab;
    [SerializeField] private GameObject _circleIndicateSrcPrefab;
    [SerializeField] private GameObject _lightningPrefab;

    private Younghee _younghee;

    public override void Init(Younghee younghee)
    {
        _younghee = younghee;
    }

    public override void UseSkill()
    {
        Vector3 cageCenter = _younghee.RGLightManager.player.PlayerRay.CalcSpawnPoint();
        StartCoroutine(SpawnLightningInCage(cageCenter));
    }

    private IEnumerator SpawnLightningInCage(Vector3 cageCenter)
    {
        List<Vector3> cellCenters = CalculateCellCenters(cageCenter, cageWidth, cageHeight, gridSize);
        List<Vector3> selectedCells = SelectRandomCells(cellCenters, strikeCount);

        foreach (Vector3 target in selectedCells)
        {
            StartCoroutine(StrikeLightning(target));
        }

        yield return new WaitForSeconds(circleSrcTime + 0.1f);

        CompleteSkill();
    }

    private IEnumerator StrikeLightning(Vector3 target)
    {
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject circleDes = Instantiate(_circleIndicateDesPrefab, target, rotation);
        GameObject circle = Instantiate(_circleIndicateSrcPrefab, target, rotation);

        yield return StartCoroutine(ExpandCircle(circle));
        Instantiate(_lightningPrefab, target, Quaternion.identity);

        Destroy(circle);
        Destroy(circleDes);
    }

    private List<Vector3> CalculateCellCenters(Vector3 center, float width, float height, int gridSize)
    {
        List<Vector3> cellCenters = new List<Vector3>();
        float cellWidth = width / gridSize;
        float cellHeight = height / gridSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Vector3 cellCenter = new Vector3(
                    center.x - width / 2f + cellWidth * (col + 0.5f),
                    0.14f,
                    center.z - height / 2f + cellHeight * (row + 0.5f)
                );
                cellCenters.Add(cellCenter);
            }
        }

        return cellCenters;
    }

    private List<Vector3> SelectRandomCells(List<Vector3> cellCenters, int count)
    {
        List<Vector3> selectedCells = new List<Vector3>();

        while (selectedCells.Count < count)
        {
            Vector3 randomCell = cellCenters[Random.Range(0, cellCenters.Count)];
            if (!selectedCells.Contains(randomCell))
            {
                selectedCells.Add(randomCell);
            }
        }

        return selectedCells;
    }

    private IEnumerator ExpandCircle(GameObject circle)
    {
        float startTime = 0f;
        Vector3 startSize = Vector3.zero;
        Vector3 maxSize = new Vector3(circleSrcMax, circleSrcMax, circleSrcMax);

        while (startTime < circleSrcTime)
        {
            startTime += Time.deltaTime;

            float progress = Mathf.Clamp01(startTime / circleSrcTime);

            circle.transform.localScale = Vector3.Lerp(startSize, maxSize, progress);
            yield return null;
        }

        circle.transform.localScale = maxSize;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    public bool x;
    public float start;
    public float end;
    public float moveTimeInterval;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private void Awake()
    {
        if (x)
        {
            _startPos = new Vector3(start, transform.localPosition.y, transform.localPosition.z);
            _endPos = new Vector3(end, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            _startPos = new Vector3(transform.localPosition.x, start, transform.localPosition.z);
            _endPos = new Vector3(transform.localPosition.x, end, transform.localPosition.z);
        }
    }

    private IEnumerator Start()
    {
        while (true)
        {
            transform.localPosition = _startPos;

            float elapsedTime = 0f;

            while (elapsedTime < moveTimeInterval)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsedTime / moveTimeInterval);
                float @new = Mathf.Lerp(0f, end, progress);
                transform.localPosition = x == true ? new Vector3(@new, _startPos.y, _startPos.z) : new Vector3(_startPos.x, @new, _startPos.z);

                yield return null;
            }

            transform.localPosition = _endPos;
        }
    }
}

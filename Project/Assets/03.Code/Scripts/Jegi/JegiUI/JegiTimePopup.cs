using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JegiTimePopup : JegiPopup
{
    public TextMeshProUGUI timeText;

    public void SetTimeText(int time)
    {
        timeText.text = time.ToString();
    }

    public void StartTimer()
    {
        StartCoroutine(HandleTimer());
    }

    private IEnumerator HandleTimer()
    {
        JegiUIManager.Instance.PopupOpen<JegiTimePopup>().SetTimeText(3);
        yield return new WaitForSeconds(1f);
        JegiUIManager.Instance.PopupClose();
        JegiUIManager.Instance.PopupOpen<JegiTimePopup>().SetTimeText(2);
        yield return new WaitForSeconds(1f);
        JegiUIManager.Instance.PopupClose();
        JegiUIManager.Instance.PopupOpen<JegiTimePopup>().SetTimeText(1);
        yield return new WaitForSeconds(1f);
        JegiUIManager.Instance.PopupClose();

        JegiGameManager.Instance.pause = false;
    }
}

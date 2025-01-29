using UnityEngine;
using UnityEngine.UI;

public class SuccessPopup : MonoBehaviour
{
    [Header("참조가 비어있으면 Reset을 눌러주세요.")]
    public Button okButton;

    private void Reset()
    {
        okButton = transform.Find("Buttons/okButton").GetComponent<Button>();
    }
    
    private void Awake()
    {
        okButton.onClick.AddListener(() =>
        {
            ClosePopup();
        });
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
    }
    
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}

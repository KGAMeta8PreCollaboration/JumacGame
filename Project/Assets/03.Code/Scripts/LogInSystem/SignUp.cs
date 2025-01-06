using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    [SerializeField] private InputField _emailIF;
    [SerializeField] private InputField _passwordIF;
    [SerializeField] private Text _errorText;

    [SerializeField] private Button _signUpButton;

    [SerializeField] private Alarm _alarm;
    [SerializeField] private SignIn _signIn;

    private void Start()
    {
        _signUpButton.onClick.AddListener(Create);
    }

    public async void Create()
    {
        if (await FirebaseManager.Instance.Create(_emailIF.text, _passwordIF.text))
        {
            _alarm.gameObject.SetActive(true);
            _alarm.SetAlarm("알림", "계정 생성 완료", CreateAction);
        }
        else
        {
            _errorText.text = "계정을 생성할 수 없습니다.";
        }
    }

    private void CreateAction()
    {
        _signIn.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

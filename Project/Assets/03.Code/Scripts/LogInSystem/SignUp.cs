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

    private void Start()
    {
        _signUpButton.onClick.AddListener(Create);
    }

    public async void Create()
    {
        if (await FirebaseManager.Instance.Create(_emailIF.text, _passwordIF.text))
        {

        }
        else
        {
            _errorText.text = "계정을 생성할 수 없습니다.";
        }
    }
}

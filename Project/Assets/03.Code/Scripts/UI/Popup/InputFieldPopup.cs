using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputFieldPopup : Popup
{
    [SerializeField] protected Text _titleText;
    [SerializeField] protected InputField _inputField;

    public void SetPopup(string title = "", Action<string> callback = null)
    {
        _titleText.text = title;
        closeAction = () => callback(_inputField.text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameSelectPopup : InputFieldPopup
{
    [SerializeField] private Button _duplicateCheckButton;
    public bool isDuplicate;

    protected override void OnEnable()
    {
        base.OnEnable();
        print("1");
        _duplicateCheckButton.onClick.AddListener(DuplicateCheckButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        print("2");
        _duplicateCheckButton.onClick.RemoveListener(DuplicateCheckButtonClick);
    }

    private async void DuplicateCheckButtonClick()
    {
        print("3");
        if (await FirebaseManager.Instance.DuplicateNicknameCheck())
        {
            print("4");

        }
    }
}

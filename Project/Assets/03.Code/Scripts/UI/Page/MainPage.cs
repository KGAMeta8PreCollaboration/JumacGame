using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyUI
{
    public class MainPage : Page
    {
        private Button _optionButton;

        private void Awake()
        {
            _optionButton = transform.Find("OptionButton").GetComponent<Button>();
        }

        private void Start()
        {
            AddButtonListener();
        }

        private void OnDestroy()
        {
            RemoveButtonListener();
        }

        private void OptionButtonClick()
        {
            PopupManager.Instance.PopupOpen<OptionPopup>();
        }

        private void AddButtonListener()
        {
            _optionButton.onClick.AddListener(OptionButtonClick);
        }

        private void RemoveButtonListener()
        {
            _optionButton.onClick.RemoveListener(OptionButtonClick);
        }
    }
}
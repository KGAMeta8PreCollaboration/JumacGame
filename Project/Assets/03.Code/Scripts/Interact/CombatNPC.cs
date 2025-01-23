using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class CombatNPC : ButtonInteractable
{
    public string nextScene;

    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private Dialogue _combatDialogue;

    private DialogueLoader _dialogueLoader;
    private List<InteractionButton> _buttons = new List<InteractionButton>();

    private void Start()
    {
        _dialogueLoader = GetComponent<DialogueLoader>();
        buttonName = _dialogue.name;
    }
    protected override void InteractionButtonClick()
    {
        _dialogueLoader.LoadDialogue(_dialogue, CombatChoice);
    }

    private void CombatChoice()
    {
        InteractionButton combatIButton = Instantiate<InteractionButton>(_interactionButtonPrefab, GameObject.Find("InteractView").transform);
        _buttons.Add(combatIButton);
        combatIButton.SetTitle("싸운다");
        Button combatButton = combatIButton.GetComponent<Button>();
        combatButton.onClick.AddListener(CombatSelect);

        InteractionButton nonCombatIButton = Instantiate<InteractionButton>(_interactionButtonPrefab, GameObject.Find("InteractView").transform);
        _buttons.Add(nonCombatIButton);
        nonCombatIButton.SetTitle("대화 그만하기");
        Button nonCombatButton = nonCombatIButton.GetComponent<Button>();
        nonCombatButton.onClick.AddListener(NonCombatSelect);

        combatButton.onClick.AddListener(DestroyButtonAll);
        nonCombatButton.onClick.AddListener(DestroyButtonAll);
    }

    private void CombatSelect()
    {
        _dialogueLoader.LoadDialogue(_combatDialogue, () => SceneManager.LoadScene(nextScene));
    }

    private void NonCombatSelect()
    {

    }

    private void DestroyButtonAll()
    {
        foreach (InteractionButton button in _buttons)
        {
            Destroy(button.gameObject, 0.1f);
        }

        _buttons.Clear();
    }
}

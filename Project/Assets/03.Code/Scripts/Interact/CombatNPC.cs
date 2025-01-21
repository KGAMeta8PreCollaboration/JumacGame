using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    private List<Button> _buttons = new List<Button>();

    private void Start()
    {
        _dialogueLoader = GetComponent<DialogueLoader>();
    }
    protected override void InteractionButtonClick()
    {
        _dialogueLoader.LoadDialogue(_dialogue, CombatChoice);
    }

    private void CombatChoice()
    {
        Button combatButton = Instantiate<Button>(_interactionButtonPrefab, GameObject.Find("InteractView").transform);
        _buttons.Add(combatButton);
        combatButton.onClick.AddListener(CombatSelect);

        Button nonCombatButton = Instantiate<Button>(_interactionButtonPrefab, GameObject.Find("InteractView").transform);
        _buttons.Add(nonCombatButton);
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
        foreach (Button button in _buttons)
        {
            Destroy(button.gameObject, 0.1f);
        }

        _buttons.Clear();
    }
}

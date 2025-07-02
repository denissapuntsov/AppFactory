using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DialogueManager : MonoBehaviour
{
    public BlockOfLines firstShiftBlock, secondShiftBlock, planksWarningBlock;
    [SerializeField] private CanvasGroup dialogueFocusCanvasGroup;
    private BlockOfLines _currentBlock;
    private int _currentLineIndex;
    private DialoguePanel _dialoguePanel;
    private TextMeshProUGUI _dialogueText;

    private void OnEnable()
    {
        DialoguePanel.OnClick += ProcessCurrentBlock;
        GameManager.OnDialogue += ToggleCanvasGroup;
        
        _dialoguePanel = FindAnyObjectByType<DialoguePanel>();
        _dialogueText = _dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ConfigurePanel(int shiftIndex)
    {
        switch (shiftIndex)
        {
            case 0:
                SetBlock(firstShiftBlock);
                _dialoguePanel.Enter();
                break;
            case 1:
                SetBlock(secondShiftBlock);
                _dialoguePanel.Enter();
                break;
            case 2:
                SetBlock(planksWarningBlock);
                _dialoguePanel.Enter();
                break;
            default:
                _dialoguePanel.Skip();
                break;
        }
    }

    private void ToggleCanvasGroup()
    {
        dialogueFocusCanvasGroup.DOFade(1, 0.5f);
    }

    private void ProcessCurrentBlock()
    {
        if (_dialogueText.text == _currentBlock.lines[^1])
        {
            dialogueFocusCanvasGroup.DOFade(0, 0.5f);
            _dialoguePanel.Exit();
            return;
        }

        _dialoguePanel.audioSource.PlayOneShot(_dialoguePanel.panelSwitch);
        _currentLineIndex++;
        _dialogueText.text = _currentBlock.lines[_currentLineIndex];
    }

    private void ResetDialogueText()
    {
        _dialogueText.text = _currentBlock.lines[0];
        _currentLineIndex = 0;
    }

    public void SetBlock(BlockOfLines newBlock)
    {
        _currentBlock = newBlock;
        ResetDialogueText();
    }

    private void OnDisable()
    {
        DialoguePanel.OnClick -= ProcessCurrentBlock;
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<BlockOfLines> tutorialBlocks, regularBlocks;
    private BlockOfLines _currentBlock;
    private int _currentLineIndex;
    private DialoguePanel _dialoguePanel;
    private TextMeshProUGUI _dialogueText;

    private void OnEnable()
    {
        DialoguePanel.OnClick += ProcessCurrentBlock;
        
        _dialoguePanel = FindAnyObjectByType<DialoguePanel>();
        _dialogueText = _dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void ProcessCurrentBlock()
    {
        if (_dialogueText.text == _currentBlock.lines[^1])
        {
            _dialoguePanel.Exit();
            return;
        }

        _currentLineIndex++;
        _dialogueText.text = _currentBlock.lines[_currentLineIndex];
    }

    private void ResetDialogueText()
    {
        _dialogueText.text = _currentBlock.lines[0];
        _currentLineIndex = 0;
    }

    public void SetNextTutorialBlock()
    {
        BlockOfLines nextBlock = tutorialBlocks[0];
        tutorialBlocks.RemoveAt(0);
        _currentBlock = nextBlock;
        ResetDialogueText();
    }

    public void SetRegularBlock()
    {
        _currentBlock = regularBlocks[Random.Range(0, regularBlocks.Count)];
        ResetDialogueText();
    }

    private void OnDisable()
    {
        DialoguePanel.OnClick -= ProcessCurrentBlock;
    }
}

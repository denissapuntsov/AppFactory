using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button tutorialButton, freePlayButton, settingsButton;
    [SerializeField] private GameObject mainGroup, settingsGroup;
    [SerializeField] private MainMenuScroll mainMenuScroll;

    private TextMeshProUGUI _settingsButtonName;

    private void OnEnable()
    {
        _settingsButtonName = settingsButton.GetComponentInChildren<TextMeshProUGUI>();
        
        tutorialButton.onClick.AddListener(() =>
        {
            if (!mainMenuScroll.IsOpen) return;
            PersistentGameInfo.Instance.isTutorial = true;
            StartCoroutine(SwitchScene(1));
        });
        freePlayButton.onClick.AddListener(() =>
        {
            if (!mainMenuScroll.IsOpen) return;
            PersistentGameInfo.Instance.isTutorial = false;
            StartCoroutine(SwitchScene(1));
        });
        settingsButton.onClick.AddListener(() =>
        {
            if (!mainMenuScroll.IsOpen) return;
            StartCoroutine(ToggleGroup());
        });
    }
    private IEnumerator SwitchScene(int index)
    {
        mainMenuScroll.Close();
        yield return new WaitUntil(() => !mainMenuScroll.IsOpen);
        SceneManager.LoadScene(index);
    }

    private IEnumerator ToggleGroup()
    {
        mainMenuScroll.Close();
        yield return new WaitUntil(() => !mainMenuScroll.IsOpen);
        mainGroup.SetActive(!mainGroup.activeSelf);
        settingsGroup.SetActive(!settingsGroup.activeSelf);
        _settingsButtonName.text = mainGroup.activeSelf ? "SETTINGS" : "BACK";
        mainMenuScroll.Open();
    }
}

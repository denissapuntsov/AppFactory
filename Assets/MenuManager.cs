using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button tutorialButton, freePlayButton, settingsButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private MainMenuScroll mainMenuScroll;

    private void OnEnable()
    {
        tutorialButton.onClick.AddListener(() => { StartCoroutine(SwitchScene(2)); });
        freePlayButton.onClick.AddListener(() => { StartCoroutine(SwitchScene(1)); });
        settingsButton.onClick.AddListener(() => { Debug.Log("Opened settings"); });
    }
    private IEnumerator SwitchScene(int index)
    {
        mainMenuScroll.Close();
        yield return new WaitUntil(() => MainMenuScroll.IsOpen == false);
        SceneManager.LoadScene(index);
    }
}

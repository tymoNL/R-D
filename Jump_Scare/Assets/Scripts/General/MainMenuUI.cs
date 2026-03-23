using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var playButton = root.Q<Button>("playButton");
        var quitButton = root.Q<Button>("quitButton");

        playButton.clicked += () =>
        {
            SceneManager.LoadScene("GameScene");
        };

        quitButton.clicked += () =>
        {
            Application.Quit();
        };
    }
}
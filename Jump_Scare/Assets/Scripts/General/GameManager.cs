using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private GameObject player;

    public bool canEscape = false;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && canEscape) {
            SceneManager.LoadScene("VictoryScene");
        }
    }
}

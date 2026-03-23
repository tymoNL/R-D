using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private GameObject player;

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Hit: " + col.name);

        if (col.CompareTag("Player"))
        {
            Debug.Log("Player entered door");
            SceneManager.LoadScene("VictoryScene");
        }
    }
}

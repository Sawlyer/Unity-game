using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Key")]
    public GameObject keyPrefab;
    public Transform[] keySpawns;

    [Header("Deposit")]
    public Transform depositPoint;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;

    public bool hasKey { get; private set; }
    bool gameEnded;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
    }

    void Start()
    {
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);

        SpawnKey();
    }

    void SpawnKey()
    {
        if (keyPrefab == null || keySpawns == null || keySpawns.Length == 0) return;

        int idx = Random.Range(0, keySpawns.Length);
        Instantiate(keyPrefab, keySpawns[idx].position, keySpawns[idx].rotation);
        hasKey = false;
        gameEnded = false;
        Time.timeScale = 1f;
    }

    public void PickKey()
    {
        if (gameEnded) return;
        hasKey = true;
        // ici tu peux afficher un petit texte "Key acquired"
    }

    public void TryDeposit()
    {
        if (gameEnded) return;
        if (!hasKey) return;

        Win();
    }

    public void Lose()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;
        if (losePanel) losePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Win()
    {
        gameEnded = true;
        Time.timeScale = 0f;
        if (winPanel) winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

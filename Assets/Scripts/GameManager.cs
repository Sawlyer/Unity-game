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
    public GameObject keyIndicator;

    // Indique si le joueur possède la clé
    // get = accessible depuis d'autres scripts
    // private set = seule cette classe peut modifier la valeur
    public bool hasKey { get; private set; }
    bool gameEnded;

    void Awake()
    {
        // Si un GameManager existe déjà, on détruit celui-ci
        // Cela garantit qu'il n'y a qu'un seul GameManager dans la scène
        if (I != null) { Destroy(gameObject); return; }
        I = this;
    }

    void Start()
    {
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        if (keyIndicator) keyIndicator.SetActive(false);

        SpawnKey();
    }

    void SpawnKey()
    {
        // Vérifie que le prefab et les points de spawn existent
        if (keyPrefab == null || keySpawns == null || keySpawns.Length == 0) return;

        int idx = Random.Range(0, keySpawns.Length);
        Instantiate(keyPrefab, keySpawns[idx].position, keySpawns[idx].rotation);

        hasKey = false;
        gameEnded = false;
        Time.timeScale = 1f;

        if (keyIndicator) keyIndicator.SetActive(false);
    }

    public void PickKey()
    {
        if (gameEnded) return;

        hasKey = true;

        if (keyIndicator) keyIndicator.SetActive(true);
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

        // Libère la souris pour pouvoir cliquer sur les boutons UI
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

        // Cache et verrouille la souris pour le gameplay FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Recharge la scène actuelle (reset complet du jeu)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

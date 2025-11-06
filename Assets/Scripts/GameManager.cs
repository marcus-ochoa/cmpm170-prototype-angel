using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class GameManager : MonoBehaviour
{


    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private CanvasGroup startMenu;
    [SerializeField] private CanvasGroup winMenu;
    [SerializeField] private CanvasGroup loseMenu;
    [SerializeField] private CanvasGroup HUD;

    [SerializeField] private SplineAnimate victimSplineAnim;

    [SerializeField] private Player player;
    [SerializeField] private GameObject floor;

    [SerializeField] private float endZ = 75.0f;

    private bool gameOver = false;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        // DontDestroyOnLoad(this);

        CloseMenu(loseMenu);
        CloseMenu(winMenu);
        CloseMenu(HUD);
        OpenMenu(startMenu);

        victimSplineAnim.Pause();
        player.SetMoveEnabled(false);
    }


    private void CloseMenu(CanvasGroup cgroup)
    {
        cgroup.alpha = 0;
        cgroup.blocksRaycasts = false;
        cgroup.interactable = false;
    }

    private void OpenMenu(CanvasGroup cgroup)
    {
        cgroup.alpha = 1;
        cgroup.blocksRaycasts = true;
        cgroup.interactable = true;
    }
    
    public void StartGame()
    {
        victimSplineAnim.Play();
        player.SetMoveEnabled(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CloseMenu(startMenu);
        CloseMenu(loseMenu);
        CloseMenu(winMenu);
        OpenMenu(HUD);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && victimSplineAnim.gameObject.transform.position.z < endZ)
        {
            gameOver = true;
            EndGame(true);
        }
    }

    public void EndGame(bool isWin)
    {
        CloseMenu(HUD);
        victimSplineAnim.Pause();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        player.SetMoveEnabled(false);

        if (isWin)
        {
            Debug.Log("YOU WIN");
            OpenMenu(winMenu);
        }
        else
        {
            Debug.Log("YOU LOSE");
            OpenMenu(loseMenu);
        }
    }

    public void MoveCameraTo(Vector3 enemyPos)
    {
        GameObject victim = victimSplineAnim.gameObject;

        Vector3 direction = Vector3.Normalize(enemyPos - victim.transform.position);

        Vector3 rightDirection = Vector3.Cross(Vector3.up, direction);

        Vector3 finalTarget = victim.transform.position - (direction * 5) + (Vector3.up * 0.8f) + (rightDirection * 0.5f);

        floor.SetActive(false);

        player.MoveToAndLookAt(finalTarget, enemyPos);
    }
    
    public void ResetGame()
    {
        Debug.Log("Resetting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class GameManager : MonoBehaviour
{


    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private CanvasGroup winMenu;
    [SerializeField] private CanvasGroup loseMenu;
    [SerializeField] private CanvasGroup HUD;

    [SerializeField] private SplineAnimate victimSplineAnim;


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

        DontDestroyOnLoad(this);

        CloseMenu(loseMenu);
        CloseMenu(winMenu);
        OpenMenu(HUD);
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGame(bool isWin)
    {
        CloseMenu(HUD);
        victimSplineAnim.Pause();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
    
    public void ResetGame()
    {
        Debug.Log("Resetting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

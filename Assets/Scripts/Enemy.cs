using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float hitRange = 80.0f;
    [SerializeField] private float flashChance = 0.2f;

    private bool hasWon = false;
    private Light flash;
    private readonly WaitForSeconds waitForSeconds = new(1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponentInChildren<Light>();
        flash.enabled = false;
        StartCoroutine(FlashCycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWon && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, hitRange))
        {
            if (hit.transform.gameObject.CompareTag("Victim"))
            {
                Debug.Log("VICTIM SHOT");
                GameManager.Instance.EndGame(false);
                GameManager.Instance.MoveCameraTo(transform.position);
                hasWon = true;
            }
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Found()
    {
        gameObject.SetActive(false);
    }
    
    private IEnumerator FlashCycle()
    {
        while (true)
        {
            if (Random.value < flashChance) flash.enabled = true;
            yield return waitForSeconds;
            flash.enabled = false;
        }
    }
}

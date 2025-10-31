using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float hitRange = 80.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, hitRange))
        {
            if (hit.transform.gameObject.CompareTag("Victim"))
            {
                Debug.Log("VICTIM SHOT");
                GameManager.Instance.EndGame(false);
                Found();
            }
        }
    }
    
    public void Found()
    {
        gameObject.SetActive(false);
    }
}

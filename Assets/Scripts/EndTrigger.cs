using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED");
        if (other.CompareTag("Victim")) GameManager.Instance.EndGame(true);
    }
}

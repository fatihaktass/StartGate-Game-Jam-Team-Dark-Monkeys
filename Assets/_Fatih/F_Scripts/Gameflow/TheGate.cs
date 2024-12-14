using UnityEngine;

public class TheGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().FinishTheGame(true);
        }
    }
}

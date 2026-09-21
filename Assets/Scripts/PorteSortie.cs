using UnityEngine;

public class PorteSortie : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (autre.CompareTag("Player"))
        {
            Debug.Log("Le joueur a atteint la sortie !");
            Destroy(autre.gameObject);
        }
    }
}

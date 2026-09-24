using UnityEngine;

public class ZoneInterdite : MonoBehaviour
{
    [SerializeField] private Transform pointDepart;

    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (!autre.CompareTag("Player"))
        {
            return;
        }
        autre.transform.position = pointDepart.position;

        Debug.Log("Le joueur doit retourner");

    }
 }


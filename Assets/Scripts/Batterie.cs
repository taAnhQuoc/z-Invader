using UnityEngine;

public class Batterie : MonoBehaviour
{
    [SerializeField] private int valeur = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) {
            return;
        }

        if (GestionJeu.Instance == null)
        {
            Debug.Log("Aucun gestion n'est present dans la scene");
            return;
        }

        GestionJeu.Instance.AjouterBatterie(valeur);
        Destroy(gameObject);
    }
}

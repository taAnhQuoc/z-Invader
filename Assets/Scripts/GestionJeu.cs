using UnityEngine;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance { get; private set; }
    [SerializeField] private int objectif = 3;
    [SerializeField] private GameObject porte;

    private int batteriesCollectes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (porte == null)
        {

            Debug.LogError("La porte n'est pas configurer");

        }
    }

    public void AjouterBatterie(int valeur)
    {
        batteriesCollectes += valeur;

        Debug.Log($"Batteries: {batteriesCollectes}/{objectif}");

        if (batteriesCollectes >= objectif)
        {
            OuvrirPorte();
        }
    }

    public void OuvrirPorte()
    {
        if (porte == null)
        {
            return;
        }

        porte.SetActive(true);
        Debug.Log("La porte est ouverte");
    }



}

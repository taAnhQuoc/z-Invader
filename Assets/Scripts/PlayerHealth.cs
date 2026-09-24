using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vie")]
    [SerializeField] private int vies = 3;

    [Header("Effet de dégâts")]
    [SerializeField] private float dureeFlash = 0.1f;
    [SerializeField] private float dureeEntreFlash = 0.1f;
    [SerializeField] private int nombreDeFlash = 3;

    private SpriteRenderer rendu;
    private Color couleurOriginale;

    public int Vies => vies;

    private void Awake()
    {
        rendu = GetComponent<SpriteRenderer>();

        if (rendu != null)
        {
            couleurOriginale = rendu.color;
        }
    }

    public void PrendreDegats(int degats)
    {
        vies -= degats;

        Debug.Log("Le joueur a pris " + degats +
                  " dégât(s). Vies restantes : " + vies);

        // Lance l'effet de flash
        StartCoroutine(FlashDegats());

        if (vies <= 0)
        {
            Mourir();
        }
    }

    private IEnumerator FlashDegats()
    {
        for (int i = 0; i < nombreDeFlash; i++)
        {
            // Devient rouge
            rendu.color = Color.red;

            yield return new WaitForSeconds(dureeFlash);

            // Retour à la couleur normale
            rendu.color = couleurOriginale;

            yield return new WaitForSeconds(dureeEntreFlash);
        }
    }

    private void Mourir()
    {
        Debug.Log("Le joueur est mort !");

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

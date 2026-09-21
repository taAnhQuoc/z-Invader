using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class EnnemiMobile : MonoBehaviour
{
    public enum TypeDeplacement
    {
        Patrouille,
        Sinusoidal
    }

    [Header("Patrouille")]
    [SerializeField] private TypeDeplacement typeDeplacement = TypeDeplacement.Patrouille;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField, Min(0.1f)] private float vitessePatrouille = 1.8f;
    [SerializeField, Min(0.1f)] private float hauteurVague = 1.1f;
    [SerializeField, Min(0.1f)] private float frequenceVague = 1.4f;

    [Header("Poursuite")]
    [SerializeField] private Transform joueur;
    [SerializeField, Min(0.5f)] private float rayonDetection = 3.5f;
    [SerializeField, Min(0.1f)] private float vitessePoursuite = 2.8f;

    [Header("Impact")]
    [SerializeField] private Transform pointDepartJoueur;
    [SerializeField, Min(0.1f)] private float delaiEntreDegats = 1.2f;

    private Rigidbody2D corps;
    private SpriteRenderer rendu;
    private Transform ciblePatrouille;
    private float prochainDegat;
    private float progressionVague;
    

    private void Awake()
    {
        corps = GetComponent<Rigidbody2D>();
        rendu = GetComponent<SpriteRenderer>();
        corps.gravityScale = 0f;
        corps.freezeRotation = true;
        
    }

    private void Start()
    {
        ciblePatrouille = pointB != null ? pointB : pointA;
    }

    private void FixedUpdate()
    {
        if (GestionJeu.Instance != null)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        bool joueurDetecte = joueur != null &&
            Vector2.Distance(corps.position, joueur.position) <= rayonDetection;
        if (joueurDetecte)
            PoursuivreJoueur();
        else if (typeDeplacement == TypeDeplacement.Sinusoidal)
            DeplacementSinusoidal();
        else
            DeplacementPatrouille();
    }

    private void PoursuivreJoueur()
    {
        Vector2 direction = ((Vector2)joueur.position - corps.position).normalized;
        AppliquerVitesse(direction, vitessePoursuite);
    }

    private void DeplacementPatrouille()
    {
        if (ciblePatrouille == null) return;
        Vector2 direction = ((Vector2)ciblePatrouille.position - corps.position).normalized;
        AppliquerVitesse(direction, vitessePatrouille);
        if (Vector2.Distance(corps.position, ciblePatrouille.position) < 0.2f)
            ciblePatrouille = ciblePatrouille == pointA ? pointB : pointA;
    }

    private void DeplacementSinusoidal()
    {
        if (pointA == null || pointB == null) return;
        progressionVague += Time.fixedDeltaTime * frequenceVague;
        float allerRetour = Mathf.PingPong(progressionVague, 1f);
        Vector2 baseTrajet = Vector2.Lerp(pointA.position, pointB.position, allerRetour);
        Vector2 cibleVague = baseTrajet + Vector2.up *
            (Mathf.Sin(progressionVague * Mathf.PI * 2f) * hauteurVague);
        Vector2 direction = (cibleVague - corps.position).normalized;
        AppliquerVitesse(direction, vitessePatrouille * 1.15f);
    }

    private void AppliquerVitesse(Vector2 direction, float vitesse)
    {
        corps.linearVelocity = direction * vitesse;
        if (Mathf.Abs(direction.x) > 0.05f) rendu.flipX = direction.x < 0f;
    }

    private void OnTriggerStay2D(Collider2D autre)
    {
        if (!autre.CompareTag("Player") || Time.time < prochainDegat) return;
        prochainDegat = Time.time + delaiEntreDegats;

        if (pointDepartJoueur != null)
        {
            autre.transform.position = pointDepartJoueur.position;
            Rigidbody2D corpsJoueur = autre.GetComponent<Rigidbody2D>();
            if (corpsJoueur != null) corpsJoueur.linearVelocity = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rayonDetection);
        if (pointA != null && pointB != null) Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
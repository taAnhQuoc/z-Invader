using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnnemiMobile : MonoBehaviour
{
    [Header("Joueur")]
    [SerializeField] private Transform joueur;

    [Header("Movement")]
    [SerializeField] private float vitesse = 2f;
    [SerializeField] private float distanceMinimale = 0.5f;
    [SerializeField] private int degats = 1;
    [SerializeField] private float delaiEntreDegats = 1f;


    private Rigidbody2D corps;
    private SpriteRenderer rendu;
    private float prochainDegat;

    private void Awake()
    {
        corps = GetComponent<Rigidbody2D>();
        rendu = GetComponent<SpriteRenderer>();

        // Pas de gravité pour l'ennemi
        corps.gravityScale = 0f;

        // Empêche l'ennemi de tourner
        corps.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (joueur == null)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (joueur.position - transform.position);

        // Arrête l'ennemi lorsqu'il est assez proche
        if (direction.magnitude <= distanceMinimale)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        direction.Normalize();

        // Déplacement vers le joueur
        corps.linearVelocity = direction * vitesse;

        // Retourne le sprite dans la bonne direction
        if (direction.x != 0)
        {
            rendu.flipX = direction.x < 0;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time < prochainDegat)
            return;

        prochainDegat = Time.time + delaiEntreDegats;

        PlayerHealth playerHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.PrendreDegats(degats);
        }
    }

}

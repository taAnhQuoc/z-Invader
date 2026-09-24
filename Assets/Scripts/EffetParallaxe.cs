using UnityEngine;

public class EffetParallaxe : MonoBehaviour
{
    [Header("Caméra")]
    [SerializeField] private Transform cameraCible;

    [Header("Parallaxe")]
    [SerializeField, Range(0f, 1f)]
    private float suiviHorizontal = 0.05f;

    [SerializeField, Range(0f, 1f)]
    private float suiviVertical = 0.03f;

    [Header("Déplacement automatique")]
    [SerializeField]
    private Vector2 vitesseAutomatique = new Vector2(0.002f, 0f);

    [Header("Limites")]
    [SerializeField]
    private float limiteX = 2f;

    [SerializeField]
    private float limiteY = 1f;

    private Vector3 positionInitiale;
    private Vector3 positionCameraInitiale;
    private Vector2 decalageAutomatique;

    private void Start()
    {
        positionInitiale = transform.position;

        if (cameraCible == null && Camera.main != null)
            cameraCible = Camera.main.transform;

        if (cameraCible != null)
            positionCameraInitiale = cameraCible.position;
    }

    private void LateUpdate()
    {
        if (cameraCible == null)
            return;

        Vector3 mouvementCamera =
            cameraCible.position - positionCameraInitiale;

        decalageAutomatique +=
            vitesseAutomatique * Time.deltaTime;

        float x = positionInitiale.x
                + mouvementCamera.x * suiviHorizontal
                + decalageAutomatique.x;

        float y = positionInitiale.y
                + mouvementCamera.y * suiviVertical
                + decalageAutomatique.y;

        // Empêche la nébuleuse de trop s'éloigner
        x = Mathf.Clamp(
            x,
            positionInitiale.x - limiteX,
            positionInitiale.x + limiteX
        );

        y = Mathf.Clamp(
            y,
            positionInitiale.y - limiteY,
            positionInitiale.y + limiteY
        );

        transform.position = new Vector3(
            x,
            y,
            positionInitiale.z
        );
    }
}
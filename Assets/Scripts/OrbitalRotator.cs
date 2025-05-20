using UnityEngine;

public class OrbitalRotator : MonoBehaviour
{
    [Tooltip("El centro de la órbita (e.g. el contenedor del Sol)")]
    public Transform centerPoint;

    [Tooltip("Velocidad de revolución (°/seg)")]
    public float orbitSpeed = 20f;

    [Tooltip("¿Girar también sobre su propio eje?")]
    public bool selfRotate = true;

    [Tooltip("Velocidad de giro propio (°/seg)")]
    public float selfSpeed = 50f;

    void Update()
    {
        // Revolución alrededor del centro
        if (centerPoint != null)
            transform.RotateAround(centerPoint.position,
                                   Vector3.up,
                                   orbitSpeed * Time.deltaTime);

        // Giro propio
        if (selfRotate)
            transform.Rotate(Vector3.up,
                             selfSpeed * Time.deltaTime,
                             Space.Self);
    }
}
using LiquidSnake.Character;
using UnityEngine;

/// <summary>
/// Componente para gestionar el comportamiento de proyectiles que se autodestruyen al entran
/// en contacto con otro objeto, reduciendo la salud del objeto golpeado si procediera.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool debugHits;

    /// <summary>
    /// Objeto desde el que se instanció esta bala (para evitar hacerle daño)
    /// </summary>
    private GameObject _bulletOwner;

    // Use this for initialization
    /// <summary>
    /// Initialize the component to be destroy the GameObject in 2 seconds.
    /// </summary>
    void Start()
    {
        Destroy(gameObject, 2f);
    } // Start

    private void OnCollisionEnter(Collision collision)
    {
        if (debugHits) Debug.LogFormat("Bullet hit {0}", collision.gameObject.name);
        if (_bulletOwner == null)
        {
            Debug.LogWarningFormat("This bullet doesn't have an owner assigned {0}.", gameObject.name);
            if (collision.collider.TryGetComponent<Health>(out var health))
            {
                health.Damage(damage);
            }
        }
        else
        {

            if (collision.collider.TryGetComponent<Health>(out var health) &&
                !collision.gameObject.GetInstanceID().Equals(_bulletOwner.GetInstanceID()))
            {
                health.Damage(damage);
            }

        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Establece la referencia de owner del proyectil para evitar aplicar daño sobre él.
    /// </summary>
    public void SetOwner(GameObject owner)
    {
        _bulletOwner = owner;
    } // SetOwner
} // Bullet
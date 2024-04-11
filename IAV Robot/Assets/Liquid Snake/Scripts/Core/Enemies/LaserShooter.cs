using LiquidSnake.Utils;
using UnityEngine;


namespace LiquidSnake.Enemies
{
    /// <summary>
    /// Componente dedicado a gestionar la acción de disparo del enemigo.
    /// </summary>
    public class LaserShooter : MonoBehaviour, IResetteable
    {

        //--------------------------------------------------------------------------------
        //                        Propiedades del inspector
        //--------------------------------------------------------------------------------
        #region Propiedades del inspector
        /// <summary>
        /// Punto desde el que se disparará el láser del enemigo.
        /// </summary>
        [SerializeField]
        private Transform shootPoint;

        /// <summary>
        /// Prefab a instanciar como láser utilizado para el disparo.
        /// </summary>
        [SerializeField]
        private GameObject bullet;

        /// <summary>
        /// Velocidad con la que se lanzará el proyectil.
        /// </summary>
        [SerializeField]
        private float velocity = 30f;

        /// <summary>
        /// Tiempo en segundos que debe transcurrir para poder volver a disparar.
        /// </summary>
        [SerializeField]
        private float cooldown = 0.3f;
        #endregion

        //--------------------------------------------------------------------------------
        //                        Propiedades privadas
        //--------------------------------------------------------------------------------
        #region Propiedades privadas
        /// <summary>
        /// Tiempo a esperar hasta el próximo disparo.
        /// </summary>
        private float _timeUntilNextShoot = 0f;
        #endregion

        //--------------------------------------------------------------------------------
        //                        Ciclo de vida del componente
        //--------------------------------------------------------------------------------
        #region Ciclo de vida del componente
        public void Start()
        {
            // si no se ha especificado un punto de disparo explícito, buscamos por nombre en la jerarquía del objeto.
            if (shootPoint == null)
            {
                shootPoint = gameObject.transform.Find("shootPoint");
                if (shootPoint == null)
                {
                    Debug.LogWarning("Shoot point not specified. Laser Shooter will not work " +
                                     "for " + gameObject.name);
                }
            }
        } // Start

        private void Update()
        {
            _timeUntilNextShoot -= Time.deltaTime;
            _timeUntilNextShoot = Mathf.Max(0f, _timeUntilNextShoot);
        } // Update
        #endregion


        //--------------------------------------------------------------------------------
        //                        API pública del componente
        //--------------------------------------------------------------------------------

        #region API pública del componente
        /// <summary>
        /// Dispara el proyectil especificado desde el punto shootPoint.
        /// Si se indica el parámetro opcional objetivo, el disparo se hará en dirección a dicho objeto.
        /// En caso contrario, el disparo se realiza en la dirección del vector forward del enemigo.
        /// </summary>
        /// <returns>true si el disparo pudo realizarse con éxito</returns>
        public bool Shoot(Transform target = null)
        {
            if (shootPoint == null || _timeUntilNextShoot > 0f)
            {
                return false;
            }

            // Instantiate the bullet prefab.
            GameObject newBullet = Instantiate(
                                        bullet, shootPoint.position,
                                        shootPoint.rotation * bullet.transform.rotation
                                    );
            newBullet.GetComponent<Bullet>().SetOwner(gameObject);
            // Give it a velocity
            if (newBullet.GetComponent<Rigidbody>() == null)
                // Safeguard test, altough the rigid body should be provided by the
                // prefab to set its weight.
                newBullet.AddComponent<Rigidbody>();

            // si el objetivo tiene un collider (lo cual normalmente será el caso), la posición objetivo será la del centro del collider
            // para tener más precisión y garantizar que golpea lo que debe golpear para generar el hit (asumiendo que no tiene una forma extraña).
            Collider targetCollider = target.GetComponent<Collider>();
            Vector3 targetPos = targetCollider == null ? target.transform.position : targetCollider.bounds.center;
            newBullet.GetComponent<Rigidbody>().velocity = velocity * (target == null ? shootPoint.forward : (targetPos - shootPoint.position).normalized);
            // reseteo del cooldown
            _timeUntilNextShoot = cooldown;
            return true;
        } // Shoot

        #endregion

        //--------------------------------------------------------------------------------
        //                        Implementación de IResetteable
        //--------------------------------------------------------------------------------

        #region Implementación de IResetteable
        public void Reset()
        {
            _timeUntilNextShoot = 0f;
        } // Reset
        #endregion
    }

} // namespace LiquidSnake.Enemies
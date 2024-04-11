using LiquidSnake.Character;
using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace LiquidSnake.Enemies
{
    public class Enemy : MonoBehaviour, IResetteable
    {
        [SerializeField]
        private Transform initialWaypoint;

        //----------------------------------------------------------------------------
        //                      Variables privadas del componente
        //----------------------------------------------------------------------------
        #region Variables privadas del componente

        /// <summary>
        /// Ejecutor de comportamiento de árboles de comportamiento (Behavior Bricks)
        /// </summary>
        private BehaviorExecutor _executor;
        /// <summary>
        /// Nav Mesh Agent para gestionar la navegación del enemigo por el entorno.
        /// </summary>
        private NavMeshAgent _navMeshAgent;
        /// <summary>
        /// Componente de salud del enemigo.
        /// </summary>
        private Health _health;
        /// <summary>
        /// Gestor de animaciones del enemigo.
        /// </summary>
        private Animator _animator;
        /// <summary>
        /// True si el GameObject contiene una referencia a un animador.
        /// </summary>
        private bool _hasAnimator;

        // animation IDs
        private int _animIDSpeed;
        #endregion

        /// <summary>
        /// Gestión de los puntos de salud del enemigo.
        /// </summary>
        public Health Health { get { return _health; } }

        //----------------------------------------------------------------------------
        //                      Ciclo de vida del componente
        //----------------------------------------------------------------------------
        #region Ciclo de vida del componente

        private void Update()
        {
            if (_hasAnimator)
            {
                if (_navMeshAgent.velocity.sqrMagnitude > 0)
                {
                    // update animator if using character
                    _animator.SetFloat(_animIDSpeed, 1f);
                }
                else
                {
                    _animator.SetFloat(_animIDSpeed, 0f);
                }
            }
        } // Update

        private void Awake()
        {
            _executor = GetComponent<BehaviorExecutor>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _health = GetComponent<Health>();
            if (_health == null)
            {
                Debug.LogWarningFormat("No se pudo encontrar un gestor de salud en el game object {0}.", gameObject.name);
            }

            _hasAnimator = TryGetComponent(out _animator);
            AssignAnimationIDs();
        } // Awake

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("speed");
        } // AssignAnimationIDs

        #endregion

        //----------------------------------------------------------------------------
        //              Métodos públicos e implementación de IResetteable
        //----------------------------------------------------------------------------
        #region Métodos públicos e implementación de IResetteable

        void IResetteable.Reset()
        {
            gameObject.SetActive(true);
            _executor.SetBehaviorParam("currentWaypoint", initialWaypoint);
            _executor.SetBehaviorParam("playerFound", false);
            _executor.SetBehaviorParam("target", null);
            _navMeshAgent.Warp(initialWaypoint.position);
            _navMeshAgent.ResetPath();
        } // Reset
        #endregion
    } // Enemy
} // namespace LiquidSnake.Enemies

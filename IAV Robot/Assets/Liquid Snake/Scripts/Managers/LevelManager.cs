using LiquidSnake.Character;
using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.Management
{
    public class LevelManager : MonoBehaviour
    {
        // -----------------------------------------------------------------------
        //              Eventos para notificar de acciones sobre el nivel
        // -----------------------------------------------------------------------
        [SerializeField]
        public UnityEvent OnLevelReset;

        [SerializeField]
        public UnityEvent OnLevelCompleted;

        [SerializeField]
        public UnityEvent OnLevelFailed;

        // -----------------------------------------------------------------------
        //                          Propiedades Privadas
        // -----------------------------------------------------------------------
        #region Propiedades Privadas
        /// <summary>
        /// Array que contiene todos los componentes que puedan ser reseteados en el nivel.
        /// Este se puebla de forma automática en el awake de este componente, de manera que 
        /// basta con implementar la interfaz IResetteable en cualquier componente hijo para
        /// que se llame a la función Reset correspondiente al comienzo de cada episodio.
        /// </summary>
        private IResetteable[] resetteables;
        #endregion

        // -----------------------------------------------------------------------
        //                    Ciclo de vida de MonoBehaviour
        // -----------------------------------------------------------------------
        #region Ciclo de vida de MonoBehaviour
        private void Awake()
        {
            SetResetteablesFromChildren();
        } // Awake

        private void Start()
        {
            ResetLevel();
        }
        #endregion

        // -----------------------------------------------------------------------
        //                          Métodos públicos
        // -----------------------------------------------------------------------
        #region Métodos públicos
        /// <summary>
        /// Resets all reseteable objects in scene.
        /// </summary>
        public void ResetLevel()
        {
            foreach (IResetteable resetteable in resetteables)
            {
                resetteable.Reset();
            }
            OnLevelReset?.Invoke();
        } // HandleResetLevel


        public void SetResetteablesFromChildren()
        {
            // Store references to all components in children that comply to the IResetteable interface
            // to reset them at the beginning of each level iteration.
            resetteables = GetComponentsInChildren<IResetteable>();
        } // SetResetteablesFromChildren


        public void EndLevel(bool success)
        {
            if (success) { OnLevelCompleted?.Invoke(); }
            else { OnLevelFailed?.Invoke(); }
        } // EndLevel
        #endregion

    } // LevelManager

} // LiquidSnake.Management
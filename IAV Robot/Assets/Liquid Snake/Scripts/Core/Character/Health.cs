using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.Character
{
    /// <summary>
    /// Componente básico para la gestión de la salud del personaje y el 
    /// lanzamiento de eventos asociados a modificaciones sobre esta variable.
    /// </summary>
    public class Health : BoundedObservableComponent<float>, IResetteable
    {

        //----------------------------------------------------------------------------
        //               Atributos desde el Inspector
        //----------------------------------------------------------------------------
        /// <summary>
        /// Puntos de salud actuales del personaje.
        /// </summary>
        [SerializeField]
        private float currentHealth = 10;

        /// <summary>
        /// Valor máximo que puede llegar a alcanzar el número de puntos 
        /// de salud del personaje.
        /// </summary>
        [SerializeField]
        private float maxHealth = 10;

        //----------------------------------------------------------------------------
        //               Implementación de BoundedObservableComponent
        //----------------------------------------------------------------------------
        #region Comunicación por Eventos

        /// <summary>
        /// Evento que notifica de la muerte del personaje cuando sus puntos de vida llegan a 0.
        /// </summary>
        [Tooltip("Evento que notifica de la muerte del personaje por vida == 0.")]
        public UnityEvent OnDeath;

        override public float CurrentValue()
        {
            return currentHealth;
        }

        override public float MinValue()
        {
            return 0f;
        }

        override public float MaxValue()
        {
            return maxHealth;
        }
        #endregion

        //----------------------------------------------------------------------------
        //              Métodos públicos e implementación de IResetteable
        //----------------------------------------------------------------------------
        #region Métodos públicos e implementación de IResetteable

        /// <summary>
        /// Aplica un daño dado sobre la salud del personaje.
        /// </summary>
        /// <param name="damage">Valor numérico del daño a aplicar sobre el personaje</param>
        public void Damage(float damage)
        {
            // guardamos la salud anterior para notificar de ella en el cambio.
            float prevHealth = currentHealth;

            // aplicamos un decremento sobre los puntos de salud del personaje
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            // notificamos del cambio en currentHealth
            OnChange?.Invoke(prevHealth, currentHealth);

            if (currentHealth <= 0f)
            {
                OnDeath?.Invoke();
            }
        } // Damage

        /// <summary>
        /// Método de conveniencia para restaurar los puntos de salud actuales del
        /// personaje a sus puntos de salud máximos. Esto tiene sentido especialmente 
        /// en el contexto de reinicios de nivel, o cuando se obtiene un objeto que cura
        /// completamente al personaje.
        /// </summary>
        public void Reset()
        {
            // guardamos la salud anterior para notificar de ella en el cambio.
            float prevHealth = currentHealth;

            currentHealth = maxHealth;

            // notificamos del cambio en currentHealth
            OnChange?.Invoke(prevHealth, currentHealth);
        }
        #endregion
    } // Health
} // namespace LiquidSnake.Character

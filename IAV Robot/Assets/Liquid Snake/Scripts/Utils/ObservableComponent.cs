using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.Utils
{
    public abstract class ObservableComponent<T> : MonoBehaviour
    {
        /// <summary>
        /// M�todo a llamar desde la clase implementadora para reaccionar ante cambios
        /// en el valor del observable. Es responsabilidad de la clase implementadora
        /// lanzar este evento cada vez que se produce una modificaci�n.
        /// El primer par�metro de la llamada se corresponde con el valor previo del observable,
        /// mientras que el segundo valor representa el nuevo valor que acaba de tomar.
        /// </summary>
        [SerializeField]
        [Tooltip("Evento lanzado para notificar a los observadores de una modificaci�n en el valor del observable.")]
        public UnityEvent<T, T> OnChange;

        /// <summary>
        /// Devuelve el valor actual del observable.
        /// </summary>
        public abstract T CurrentValue();
    } // ObservableComponent

    public abstract class BoundedObservableComponent<T> : ObservableComponent<T>
    {
        /// <summary>
        /// Devuelve el valor m�nimo de este observable.
        /// </summary>
        public abstract T MinValue();

        /// <summary>
        /// Devuelve el valor m�ximo de este observable.
        /// </summary>
        public abstract T MaxValue();
    } // BoundedObservableComponent

} // namespace LiquidSnake.Utils

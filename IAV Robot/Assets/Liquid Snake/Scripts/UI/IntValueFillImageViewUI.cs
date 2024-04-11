using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LiquidSnake.UI
{
    /// <summary>
    /// Componente destinado a manejar la visualización de un observable de tipo int que refleje
    /// su estado en formato Imagen con fill (vida, munición, tiempo, etc).
    /// </summary>
    public class IntValueFillImageViewUI : MonoBehaviour
    {
        // -----------------------------------------------------------------------
        //                Propiedades a configurar desde el inspector
        // -----------------------------------------------------------------------
        #region Propiedades a configurar desde el inspector
        [Tooltip("Imagen cuyo estado de fill representará este componente.")]
        [SerializeField]
        private Image fillImage;

        [Tooltip("Imagen \"dark\" que se actualiza únicamente tras parar de recibir estímulos durante un tiempo dado.")]
        [SerializeField]
        private Image fillDarkImage;

        [Tooltip("Tiempo en segundos a esperar sin estímulos antes de actualizar la imagen dark.")]
        [SerializeField]
        private float timeUntilRebound = 1f;

        [Tooltip("Tiempo dedicado a modificar el valor de la imagen dark hasta alcanzar a la imagen base.")]
        [SerializeField]
        private float reboundTime = 1f;
        #endregion

        // -----------------------------------------------------------------------
        //                        Propiedades privadas
        // -----------------------------------------------------------------------
        #region Propiedades privadas
        /// <summary>
        /// Observable a representar por medio de este componente.
        /// </summary>
        [SerializeField]
        protected BoundedObservableComponent<float> _observable;

        /// <summary>
        /// Tiempo que ha pasado desde que se percibió el último cambio.
        /// </summary>
        protected float _timeWithoutStimuli = 0f;
        #endregion

        // -----------------------------------------------------------------------
        //                        Métodos privados
        // -----------------------------------------------------------------------
        #region Métodos privados
        /// <summary>
        /// Actualiza el valor de salud actual de la imagen base y resetea el contador 
        /// de tiempo hasta poder actualizar la imagen oscura de acumulación de cambios.
        /// </summary>
        protected void UpdateValue(float prevValue, float newValue)
        {
            bool barsInSync = Mathf.Approximately(fillImage.fillAmount, fillDarkImage.fillAmount);

            fillImage.fillAmount = (float)newValue / _observable.MaxValue();

            if(_timeWithoutStimuli > timeUntilRebound && !barsInSync)
            {
                // nos ha llegado un estímulo mientras estábamos actualizando la barra oscura.
                // forzamos a que la barra oscura tenga el fill amount de la base, pero sólo si las barras 
                // estaban desincronizadas antes del estímulo (en caso contrario, en realidad la barra oscura
                // ya se había terminado de actualizar).
                fillDarkImage.fillAmount = fillImage.fillAmount;
            }
            _timeWithoutStimuli = 0f;
        } // UpdateValue
        #endregion

        // -----------------------------------------------------------------------
        //               Métodos de Ciclo de vida del MonoBehaviour
        // -----------------------------------------------------------------------
        #region Métodos de Ciclo de vida del MonoBehaviour
        private void OnEnable()
        {
            if (_observable == null) return;

            _observable.OnChange.AddListener(UpdateValue);
            fillImage.fillAmount = (float)_observable.CurrentValue() / _observable.MaxValue();
        } // OnEnable

        private void OnDisable()
        {
            if (_observable == null) return;

            _observable.OnChange.RemoveListener(UpdateValue);
        } // OnDisable

        private void Update()
        {
            if (fillDarkImage == null) return;

            _timeWithoutStimuli += Time.deltaTime;
            // establecemos que este contador de tiempo no puede llegar a más del tiempo hasta empezar
            // el cambio en la barra oscura más el tiempo de modificación de dicha barra. Esto nos dará 
            // una idea de cuánto tiempo ha pasado desde que excedimos el tiempo hasta el rebound.
            _timeWithoutStimuli = Mathf.Min(_timeWithoutStimuli, timeUntilRebound + reboundTime);

            if(_timeWithoutStimuli > timeUntilRebound)
            {
                // cuánto hemos avanzado desde que excedimos el tiempo until rebound
                // este valor será 0 cuando acabemos de alcanzar timeUntilRebound y
                // 1 cuando se alcance el máximo que comentábamos anteriormente.
                float t = (_timeWithoutStimuli - timeUntilRebound) / reboundTime;
                fillDarkImage.fillAmount = Mathf.Lerp(fillDarkImage.fillAmount, fillImage.fillAmount, t);
            }
        }
        #endregion

    } // FloatValueFillImageViewUI

} // namespace LiquidSnake.UI

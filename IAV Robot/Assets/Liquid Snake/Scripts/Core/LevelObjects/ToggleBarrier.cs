using LiquidSnake.Utils;
using System.Collections;
using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class ToggleBarrier : MonoBehaviour, IResetteable
    {
        [SerializeField]
        [Tooltip("Tiempo en segundos que tarda el láser en activarse o desactivarse.")]
        private float toggleTime = 2f;

        [SerializeField]
        [Tooltip("Determina si el láser está activado nada más empezar el nivel.")]
        private bool originallyEnabled;


        /// <summary>
        /// Corrutina destinada a habilitar o deshabilitar el láser en el tiempo establecido.
        /// Almacenada en esta variable para poder cancelarla en una llamada a Reset.
        /// </summary>
        private Coroutine _toggleLaserCoroutine = null;
        private MeshRenderer _laserRenderer;
        private Collider _laserCollider;
        private bool _shouldBeEnabled;


        private void Start()
        {
            Reset();
        }

        private void Awake()
        {
            _laserRenderer = GetComponent<MeshRenderer>();
            _laserCollider = GetComponent<Collider>();
        }

        public void ToggleLaser()
        {
            _toggleLaserCoroutine = StartCoroutine(ToggleLaserCoroutine());
        }

        IEnumerator ToggleLaserCoroutine()
        {
            // habilitamos siempre el mesh renderer para mostrar la animación
            _laserRenderer.enabled = true;

            // estado final al que queremos llegar
            _shouldBeEnabled = !_shouldBeEnabled;

            // material original del mesh cuyo alpha queremos modificar
            Color clr = _laserRenderer.material.color;

            // puntos A y B en la interpolación de la animación del alpha
            float a = _shouldBeEnabled ? 0f : 1f; // ¿De dónde venimos?
            float b = _shouldBeEnabled ? 1f : 0f; // ¿A dónde vamos?

            for (float t = 0f; t < toggleTime; t += Time.deltaTime)
            {
                float p = t / toggleTime;
                _laserRenderer.material.SetColor("_Color", new Color(clr.r, clr.g, clr.b, (1 - p) * a + p * b));
                yield return null;
            }

            _laserRenderer.material.SetColor("_Color", new Color(clr.r, clr.g, clr.b, b));
            _laserRenderer.enabled = _shouldBeEnabled;
            _laserCollider.enabled = _shouldBeEnabled;
        } // ToggleLaserCoroutine

        public void Reset()
        {
            if (_toggleLaserCoroutine != null) StopCoroutine(_toggleLaserCoroutine);

            _shouldBeEnabled = originallyEnabled;
            _laserRenderer.enabled = originallyEnabled;
            _laserCollider.enabled = originallyEnabled;

            Color clr = _laserRenderer.material.color;
            _laserRenderer.material.SetColor("_Color", new Color(clr.r, clr.g, clr.b, originallyEnabled ? 1f : 0f));
        } // Reset

    } // ToggleBarrier

} // namespace LiquidSnake.LevelObjects

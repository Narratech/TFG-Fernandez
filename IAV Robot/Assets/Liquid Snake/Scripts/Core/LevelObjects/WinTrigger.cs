using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.Character
{

    /// <summary>
    /// Interface to implement by any objects that trigger an interaction with a win region.
    /// </summary>
    public interface IWinRegionHandler
    {
        void WinRegionEntered();
    }

    public class WinTrigger : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Tags que serán comprobadas cuando entre un objeto en este trigger de cara a lanzar eventos.")]
        private List<string> tagsToCheck = new List<string>();

        [SerializeField]
        [Tooltip("Evento invocado cada vez que se detecta un trigger con una tag de la lista de etiquetas permitidas con el objeto como parámetro")]
        private UnityEvent<GameObject> onTagDetected;

        private void OnTriggerEnter(Collider other)
        {
            foreach (var tag in tagsToCheck)
            {
                if (other.CompareTag(tag))
                {
                    onTagDetected?.Invoke(other.gameObject);
                }
            }
        }
    }
}
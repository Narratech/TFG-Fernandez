using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.LevelObjects
{
    public class StarAreaTrigger : MonoBehaviour, IResetteable
    {
        [SerializeField]
        private UnityEvent onAreaEntered;

        public void Reset()
        {
            enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onAreaEntered?.Invoke();
                enabled = false;
            }
        }
    } // StarAreaTrigger

} // namespace LiquidSnake.LevelObjects
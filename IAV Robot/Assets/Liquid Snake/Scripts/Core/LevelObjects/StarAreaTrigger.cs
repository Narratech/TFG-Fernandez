using LiquidSnake.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace LiquidSnake.LevelObjects
{
    public class StarAreaTrigger : MonoBehaviour, IResetteable
    {
        [SerializeField]
        private bool triggerAlways = false;

        [SerializeField]
        private UnityEvent onAreaEntered;

        private bool _triggered = false;

        public bool Triggered()
        {
            return _triggered;
        }

        public void Reset()
        {
            _triggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !_triggered)
            {
                onAreaEntered?.Invoke();
                if (!triggerAlways) _triggered = true;
            }
        }
    } // StarAreaTrigger

} // namespace LiquidSnake.LevelObjects
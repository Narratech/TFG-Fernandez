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
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onAreaEntered?.Invoke();
                gameObject.SetActive(false);
            }
        }
    } // StarAreaTrigger

} // namespace LiquidSnake.LevelObjects

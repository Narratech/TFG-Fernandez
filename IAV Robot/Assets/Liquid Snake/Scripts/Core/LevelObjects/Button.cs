using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class Button : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private Transform position;

        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }

        public bool Triggered()
        {
            StarAreaTrigger trigger = GetComponent<StarAreaTrigger>();
            return trigger != null ? trigger.Triggered() : false;
        }

        public Transform GetPosition()
        {
            return position;
        }
    }
} // namespace LiquidSnake.LevelObjects
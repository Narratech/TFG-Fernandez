using LiquidSnake.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class Button : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }
    }
} // namespace LiquidSnake.LevelObjects
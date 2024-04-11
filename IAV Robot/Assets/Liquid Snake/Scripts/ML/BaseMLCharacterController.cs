
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.Sentis;
using UnityEngine.Events;

namespace LiquidSnake.Agents
{
    public class BaseMLCharacterController : Agent
    {
        public UnityEvent onEpisodeBegin;

        public ModelAsset asset;

        private void Start()
        {
            if (asset)
            {
                SetModel("MLPlayerController", asset);
                var behaviorParams = GetComponent<BehaviorParameters>();
                behaviorParams.BehaviorType = BehaviorType.InferenceOnly;
            }
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();
            onEpisodeBegin?.Invoke();
        }
    } // BaseMLCharacterController

} // namespace TLTLUnity.Agents
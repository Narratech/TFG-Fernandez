using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to play an animation in the GameObject
    /// </summary>
    [Action("Animation/ToggleAnimator")]
    [Help("Enables or disables the animator found in the game object")]
    public class ToggleAnimator : GOAction
    {
        ///<value>Wheter to enable or disable the animator.</value>
        [InParam("enableAnimator")]
        [Help("Wheter to enable or disable the animator")]
        public bool enableAnimator;

        private Animator _animator;

        /// <summary>Initialization Method of PlayAnimation.</summary>
        /// <remarks>Associate and Inacialize the animation and the elapsed time.</remarks>
        public override void OnStart()
        {
            _animator = gameObject.GetComponent<Animator>();
            if(_animator == null )
            {
                Debug.LogError("No animator found in game object with name " + gameObject.name);
            }
        }
        /// <summary>Method of Update of ToggleAnimator.</summary>
        public override TaskStatus OnUpdate()
        {
            if (_animator == null)
                return TaskStatus.FAILED;
            _animator.enabled = enableAnimator;
            return TaskStatus.COMPLETED;
        }
    } // ToggleAnimator
} // namespace BBUnity.Actions

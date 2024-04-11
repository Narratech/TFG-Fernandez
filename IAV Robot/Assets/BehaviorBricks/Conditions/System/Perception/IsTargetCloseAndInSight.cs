using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance and angle.
    [Condition("Perception/IsTargetCloseAndInSight")]
    [Help("Checks whether a target is close and in sight depending on a given distance and an angle")]
    public class IsTargetCloseAndInSight : GOCondition
    {
        ///<value>Input Target Parameter to to check the distance and angle.</value>
        [InParam("target")]
        [Help("Target to check the distance and angle")]
        public GameObject target;

        ///<value>Input view angle parameter to consider that the target is in sight.value>
        [InParam("angle")]
        [Help("The view angle to consider that the target is in sight")]
        public float angle;

        ///<value>Input maximum distance Parameter to consider that the target is close.</value>
        [InParam("closeDistance")]
        [Help("The maximum distance to consider that the target is close")]
        public float closeDistance;

        [InParam("eyes")]
        [Help("Point that marks the origin of the raycast")]
        public Transform eyes;

        [InParam("layerMask")]
        [Help("What layers we should take into account when raycasting")]
        public LayerMask layerMask;

        /// <summary>
        /// Collider del objetivo para cachear resultado.
        /// </summary>
        private Collider targetCollider;


        /// <summary>
        /// Checks whether a target is close and in sight depending on a given distance and an angle, 
        /// First calculates the magnitude between the gameobject and the target and then compares with the given distance, then
        /// casting a raycast to the target and then compare the angle of forward vector with de raycast direction.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and the target is lower than the given distance.
        ///          false if the angle of forward vector with the  raycast direction is lower than the given angle.</returns>
		public override bool Check()
        {
            if (targetCollider == null) { targetCollider = target.GetComponent<Collider>(); }
            Vector3 targetPos = targetCollider.bounds.center;

            Vector3 dir = targetPos - eyes.position;
            if (dir.sqrMagnitude > closeDistance * closeDistance)
                return false;
            if (Physics.Raycast(eyes.position, dir, out var eyesRaycastHit, layerMask))
            {
                // ¿Hemos golpeado al objetivo?
                bool targetSeen = eyesRaycastHit.collider.gameObject == target;
                if (!targetSeen) return false;

                // el ángulo es adecuado si es menor que el ángulo del cono de visión entre dos (el cono está centrado en los ojos)
                bool angleCorrect = Vector3.Angle(dir, eyes.forward) < angle * 0.5f;

                return angleCorrect;
            }
            // no hemos golpeado nada
            return false;
        } // Check
    } // IsTargetCloseAndInSight

} // namespace BBUnity.Conditions
using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace LiquidSnake.Agents
{
    public class RobotKyleActuatorComponent : ActuatorComponent
    {
        [SerializeField]
        private StarterAssetsInputs inputs;

        [SerializeField]
        private BaseActuator.ActuatorMode actuatorMode;


        [SerializeField]
        private bool moveEnabled;
        [SerializeField]
        private bool lookEnabled;
        [SerializeField]
        private bool jumpEnabled;
        [SerializeField]
        private bool sprintEnabled;
        [SerializeField]
        private bool crouchEnabled;

        ActionSpec m_ActionSpec;

        /// <summary>
        /// Creates a BasicActuator.
        /// </summary>
        /// <returns></returns>
        public override IActuator[] CreateActuators()
        {
            var actuators = new List<IActuator>();
            if (moveEnabled) actuators.Add(new MoveActuator(inputs, actuatorMode));
            if (jumpEnabled) actuators.Add(new JumpActuator(inputs, actuatorMode));
            if (sprintEnabled) actuators.Add(new SprintActuator(inputs, actuatorMode));
            if (crouchEnabled) actuators.Add(new CrouchActuator(inputs, actuatorMode));

            m_ActionSpec = ActionSpec.Combine(actuators.Select(act => act.ActionSpec).ToArray());
            return actuators.ToArray();
        }

        public override ActionSpec ActionSpec
        {
            get { return m_ActionSpec; }
        }
    } // RobotKyleActuatorComponent
} // namespace TLTLUnity.Agents
using StarterAssets;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace LiquidSnake.Agents
{
    public abstract class BaseActuator : IActuator
    {
        public enum ActuatorMode
        {
            HumanGame, // utilizado cuando un humano juega sin grabar demostraciones (Heuristic OFF, AI Action OFF)
            AIGame, // utilizado cuando una IA juega sin grabar demostraciones, o mientras entrena (Heuristic OFF, AI Action ON)
            HumanDemonstrations, // utilizado cuando un humano juega grabando demostraciones (Heuristic ON, AI Action OFF)
            AIDemonstrations // utilizado cuando un humano juega sin grabar demostraciones (Heuristic ON, AI Action ON)
        }

        protected StarterAssetsInputs inputs;
        protected ActionSpec m_ActionSpec;

        protected ActuatorMode m_ActuatorMode;

        public BaseActuator(StarterAssetsInputs inputs, ActuatorMode actuatorMode)
        {
            this.inputs = inputs;
            m_ActuatorMode = actuatorMode;
        }

        public abstract string Name { get; }

        public abstract ActionSpec ActionSpec { get; }

        public void Heuristic(in ActionBuffers actionBuffersOut)
        {
            // La función heurística se llama por ML Agents únicamente cuando
            // el modo está en Heurístico Ó cuando está en Default y no hay un modelo
            // asociado a nuestro agente. Ahora bien, cuando juega un humano, toda la
            // funcionalidad va a estar gestionada en otro lado (de hecho resulta extraño
            // pedir que se modifique el esquema de control para que pase por aquí en lugar
            // de por donde sea que se gestione en el juego en cuestión), y aquí en realidad lo
            // que nos va a interesar va a ser "grabar" los inputs del jugador, asumiendo que
            // estamos grabando demostraciones, para poder hacer aprendizaje por imitación.

            // ML Agents siempre llamará a este método antes que a OnActionReceived para 
            // poder settear los buffers de acción que le llegarán a OnActionReceived, 
            // asumiendo de nuevo que estemos usando una política Heurística. Esto significa
            // que cuando juegue el humano, aquí debemos "interceptar" su intencionalidad
            // y guardarla en los buffers de acción (si queremos grabar su comportamiento), 
            // PERO no vamos a querer que estas acciones se lleguen a ejecutar por medio
            // de OnActionReceived, así que en ese caso nos tenemos que asegurar de que
            // al llegar a OnActionReceived tras pasar por este método se ignore su contenido.
            if (m_ActuatorMode == ActuatorMode.HumanDemonstrations) RecordHeuristicActions(actionBuffersOut);
        }

        public abstract void RecordHeuristicActions(in ActionBuffers actionBuffers);

        public void OnActionReceived(ActionBuffers actionBuffers)
        {
            // ¿Cuándo se va a llamar a este método? 
            // Caso 1: cuando hay un modelo controlado por IA en el agente y el modo
            // no está explícitamente establecido como Heuristic. En ese caso, NO se llama
            // al método Heuristic, sino que se le pide al modelo neuronal que rellene los
            // buffers de acción con lo que le parezca oportuno y se le pasa esa información 
            // a este método, donde se espera que se aplique una acción sobre el agente en 
            // base al contenido de los buffers. Esto por lo general se traduce en convertir
            // el contenido de los buffers a algo semejante a un input del jugador (o a una 
            // acción de alto nivel, pero de momento nos limitamos a inputs en un sentido más 
            // de controlador).

            // Caso 2: cuando hay un modelo heurístico, controlado por el humano. Esta es la parte
            // "sucia", porque como asumimos que en realidad ahí el control se gestiona por el sistema
            // del juego (en el caso 1 también, pero ahí no hay un humano dando input directamente), si 
            // aplicamos las acciones que registremos en RecordHeuristicActions durante las demos
            // en la práctica estaremos duplicando estas acciones en cada paso de la ejecución
            // (una vez como consecuencia del input humano, y otra como consecuencia del HandleActionReceived).
            // Por esto, si estamos en un modo Humano, vamos a ignorar directamente este método.
            if (m_ActuatorMode == ActuatorMode.HumanGame || m_ActuatorMode == ActuatorMode.HumanDemonstrations) return;

            HandleActionReceived(actionBuffers);
        }

        public abstract void HandleActionReceived(ActionBuffers actionBuffers);

        public virtual void ResetData() { }

        public virtual void WriteDiscreteActionMask(IDiscreteActionMask actionMask) { }
    } // BaseActuator

    public class MoveActuator : BaseActuator
    {

        public MoveActuator(StarterAssetsInputs inputs, ActuatorMode actuatorMode) : base(inputs, actuatorMode)
        {
            m_ActionSpec = ActionSpec.MakeDiscrete(3, 3);
        }

        public override string Name => "Move";
        public override ActionSpec ActionSpec => m_ActionSpec;

        public override void HandleActionReceived(ActionBuffers actionBuffers)
        {
            // 0 = left, 1 = no move, 2 = right
            var movementX = -1f + actionBuffers.DiscreteActions[0];
            // 0 = left, 1 = no move, 2 = forward
            var movementY = -1f + actionBuffers.DiscreteActions[1];

            inputs.MoveInput(new Vector2(movementX, movementY));
        }

        public override void RecordHeuristicActions(in ActionBuffers actionBuffersOut)
        {
            var discreteActions = actionBuffersOut.DiscreteActions;
            discreteActions[0] = inputs.move.x < -0.1f ? 0 : (inputs.move.x > 0.1f ? 2 : 1);
            discreteActions[1] = inputs.move.y < -0.1f ? 0 : (inputs.move.y > 0.1f ? 2 : 1);
        }

    } // MoveActuator

    // -----------------------------------------------------------------------------

    public class JumpActuator : BaseActuator
    {

        public JumpActuator(StarterAssetsInputs inputs, ActuatorMode actuatorMode) : base(inputs, actuatorMode)
        {
            m_ActionSpec = ActionSpec.MakeDiscrete(2); // saltar o no saltar
        }
        public override string Name => "Jump";
        public override ActionSpec ActionSpec => m_ActionSpec;

        public override void HandleActionReceived(ActionBuffers actionBuffers)
        {
            var jump = actionBuffers.DiscreteActions[0];

            inputs.JumpInput(jump == 1);
        }

        public override void RecordHeuristicActions(in ActionBuffers actionBuffersOut)
        {
            var discreteActions = actionBuffersOut.DiscreteActions;
            discreteActions[0] = inputs.jump ? 1 : 0;
        }
    } // JumpActuator

    // -----------------------------------------------------------------------------

    public class SprintActuator : BaseActuator
    {
        public SprintActuator(StarterAssetsInputs inputs, ActuatorMode actuatorMode) : base(inputs, actuatorMode)
        {
            m_ActionSpec = ActionSpec.MakeDiscrete(2); // sprint o no sprint
        }

        public override string Name => "Sprint";
        public override ActionSpec ActionSpec => m_ActionSpec;

        public override void HandleActionReceived(ActionBuffers actionBuffers)
        {
            var sprint = actionBuffers.DiscreteActions[0];

            inputs.SprintInput(sprint == 1);
        }

        public override void RecordHeuristicActions(in ActionBuffers actionBuffersOut)
        {
            var discreteActions = actionBuffersOut.DiscreteActions;
            discreteActions[0] = inputs.sprint ? 1 : 0;
        }
    } // SprintActuator

    // -----------------------------------------------------------------------------

    public class CrouchActuator : BaseActuator
    {
        public CrouchActuator(StarterAssetsInputs inputs, ActuatorMode actuatorMode) : base(inputs, actuatorMode)
        {
            m_ActionSpec = ActionSpec.MakeDiscrete(2); // crouch o no crouch
        }

        public override string Name => "Crouch";
        public override ActionSpec ActionSpec => m_ActionSpec;

        public override void HandleActionReceived(ActionBuffers actionBuffers)
        {
            var crouch = actionBuffers.DiscreteActions[0];

            inputs.CrouchInput(crouch == 1);
        }

        public override void RecordHeuristicActions(in ActionBuffers actionBuffersOut)
        {
            var discreteActions = actionBuffersOut.DiscreteActions;
            discreteActions[0] = inputs.crouch ? 1 : 0;
        }
    } // CrouchActuator

} // namespace TLTLUnity.Agents

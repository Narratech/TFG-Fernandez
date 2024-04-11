using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// Acción básica para dar valor a una variable GameObject de la pizarra
    /// </summary>
    [Action("Basic/SetGameObject")]
    [Help("Sets a value to a boolean variable")]
    public class SetGameObject : BasePrimitiveAction
    {
        

        ///<value>OutPut GameObject Parameter.</value>
        [OutParam("var")]
        [Help("output variable")]
        public GameObject var;


        ///<value>Input GameObject Parameter.</value>
        [InParam("value")]
        [Help("Value")]
        public GameObject value;

        /// <summary>Initialization Method of SetGameObject.</summary>
        /// <remarks>Initializes the GameObject value.</remarks>
        public override void OnStart()
        {
            var = value;
        }
        /// <summary>Method of Update of SetGameObject.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}

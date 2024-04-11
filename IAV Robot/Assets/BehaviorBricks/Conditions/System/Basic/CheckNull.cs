using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if Booleans have the same value.
    /// </summary>
    [Condition("Basic/CheckNull")]
    [Help("Checks whether a game object is null")]
    public class CheckNull : ConditionBase
    {
        [InParam("objectToCheck")]
        [Help("Object to evaluate")]
        public GameObject objectToCheck;

        [InParam("expectedToBeNull")]
        [Help("Whether we expect this game object to be null or not")]
        public bool expectedToBeNull;

        /// <summary>
        /// Checks whether two booleans have the same value.
        /// </summary>
        /// <returns>the value of compare first boolean with the second boolean.</returns>
		public override bool Check()
		{
			return expectedToBeNull ? objectToCheck == null : objectToCheck != null;
		}
    }
}
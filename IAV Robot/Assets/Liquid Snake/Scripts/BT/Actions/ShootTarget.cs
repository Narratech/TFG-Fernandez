using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

namespace LiquidSnake.Enemies
{
    [Action("LiquidSnake/Enemies/ShootTarget")]
    [Help("Triggerea la acción de disparo del enemigo, si es posible, en dirección al objetivo especificado.")]
    public class ShootTarget : GOAction
    {

        ///<value>Objetivo al que disparar el láser.</value>
        [InParam("target")]
        public GameObject target;


        /// <summary>
        /// Disparador de láser del enemigo que realiza la acción.
        /// </summary>
        private LaserShooter shooter;


        public override void OnStart()
        {
            shooter = gameObject.GetComponent<LaserShooter>();
            if (shooter == null)
            {

                Debug.LogWarning($"No se encontró un LaserShooter en el enemigo con nombre {gameObject.name}.");

            }
            base.OnStart();
        } // OnStart


        public override TaskStatus OnUpdate()
        {
            if (shooter == null)
            {
                return TaskStatus.FAILED;
            }

            return shooter.Shoot(target != null ? target.transform : null) ? TaskStatus.COMPLETED : TaskStatus.FAILED;
        } // OnUpdate

    } // class DoneShootOnce

} // namespace
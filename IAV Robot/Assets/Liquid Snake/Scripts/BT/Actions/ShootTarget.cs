using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

namespace LiquidSnake.Enemies
{
    [Action("LiquidSnake/Enemies/ShootTarget")]
    [Help("Triggerea la acci�n de disparo del enemigo, si es posible, en direcci�n al objetivo especificado.")]
    public class ShootTarget : GOAction
    {

        ///<value>Objetivo al que disparar el l�ser.</value>
        [InParam("target")]
        public GameObject target;


        /// <summary>
        /// Disparador de l�ser del enemigo que realiza la acci�n.
        /// </summary>
        private LaserShooter shooter;


        public override void OnStart()
        {
            shooter = gameObject.GetComponent<LaserShooter>();
            if (shooter == null)
            {

                Debug.LogWarning($"No se encontr� un LaserShooter en el enemigo con nombre {gameObject.name}.");

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
using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;


[Action("LiquidSnake/RotateRandomly")]
[Help("Smoothly rotates the character in a random direction and degrees")]
public class RotateRandomly : GOAction
{
    [InParam("rotationSpeed")]
    [Help("Rotation speed")]
    public float rotationSpeed;

    private float _acceptableRange = 0.1f;
    private Quaternion _goalRotation;

    public override void OnStart()
    {
        base.OnStart();

        int direction = Random.Range(-1, 1) >= 0 ? 1 : -1;
        // nos aseguramos de que siempre haya una rotación lo suficientemente amplia
        _goalRotation = Quaternion.Euler(0, Random.Range(90f, 180f) * direction, 0);
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        // ¿Cuánto nos queda para alcanzar la rotación final en grados?
        float currentAngularDistance = Quaternion.Angle(gameObject.transform.rotation, _goalRotation);

        // Como slerp toma un parámetro entre 0 y 1 para interpolar rotaciones,
        // utilizamos la distancia angular restante y hacemos que avance tanto como nos permitiría
        // nuestra velocidad de rotación (deltaTime * rotationSpeed nos da los grados que podemos
        // avanzar en este update, y eso hay que normalizarlo respecto al total de distancia).
        float t = Mathf.Clamp01(Time.deltaTime * rotationSpeed / currentAngularDistance);

        gameObject.transform.rotation =
            Quaternion.Slerp(gameObject.transform.rotation,
            _goalRotation, t);

        var dot = Quaternion.Dot(_goalRotation, gameObject.transform.rotation);
        var abs = Mathf.Abs(dot);
        if (1 - abs > _acceptableRange)
        {
            return TaskStatus.RUNNING;
        }
        return TaskStatus.COMPLETED;
    }
}

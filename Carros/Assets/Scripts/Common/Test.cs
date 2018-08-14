using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Rigidbody body;
    public Transform target;

    public float height = 3.0f;
    public float time = 1.0f;
    public float angle = 45.0f;
    public float speed = 10.0f;

    public void StartParabolic1()
    {
        Vector3 initialPosition = body.position;
        Vector3 targetPosition = target.position;
        Vector3 jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, height);
        Debug.Log("Jump velocity" + jumpVelocity.ToString());

        body.velocity = jumpVelocity;
    }
    public void StartParabolic2()
    {
        StartCoroutine(DoParabolic());
    }

    public IEnumerator DoParabolic()
    {
        Vector3 initialPosition = body.transform.position;
        Vector3 targetPosition = target.position;
        Vector3 launchVector = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, height);
        float deltaY = 0;
        body.velocity = launchVector * speed;
        yield return new WaitForSeconds(0.25f);

        while(deltaY < height)
        {
            Debug.Log("Going Up");
            deltaY = body.position.y - initialPosition.y;
            float currentSpeed = body.velocity.magnitude;
            body.velocity = launchVector * currentSpeed;
            yield return new WaitForSeconds(0.25f);
        }
        while(deltaY > height && (body.position - targetPosition).magnitude > 0.1)
        {
            Debug.Log("Going Down");
            float currentSpeed = body.velocity.magnitude;
            body.velocity = (targetPosition - body.position).normalized * (currentSpeed + Mathf.Abs(Physics.gravity.y));
            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }
}

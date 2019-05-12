using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    Rigidbody rBody;
    public Transform Target;
    public float speed = 10;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }


    public override void AgentReset()
    {
        // 바닥으로 떨어지는 경우, 초기화
        if (this.transform.position.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0, 0.5f, 0);
        }

        Target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        // Target의 Vector 정보 3가지
        AddVectorObs(Target.position);
        // Agent의 Vector 정보 3가지
        AddVectorObs(this.transform.position);


        // x, z 방향의 Velocity Vector 2가지
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        // 보상
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);

        if (distanceToTarget < 1.42f) 
        {
            SetReward(1.0f);
            Done();
        }

        if (this.transform.position.y < 0)
        {
            Done();   
        }
    }


}
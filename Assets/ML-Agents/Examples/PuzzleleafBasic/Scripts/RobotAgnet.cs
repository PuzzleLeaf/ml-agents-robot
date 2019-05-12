using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RobotAgnet : Agent
{
    Animator anim;
    float h, v;
    public Transform Target;
    public float speed = 10;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void AgentReset()
    {
        // 바닥으로 떨어지는 경우, 초기화
        if (this.transform.position.y < 0)
        {
            this.transform.position = new Vector3(0, 0, 0);
            this.transform.rotation = Quaternion.identity;
        }

        Target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        // Target의 Vector 정보 3가지
        AddVectorObs(Target.position);
        // Agent의 Vector 정보 3가지
        AddVectorObs(this.transform.position);


        // x,z 방향의 Velocity Vector 2가지
        AddVectorObs(this.anim.velocity.x);
        AddVectorObs(this.anim.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];

        anim.SetFloat("Direction", controlSignal.x);
        anim.SetFloat("Speed", controlSignal.z);

        //rBody.AddForce(controlSignal * speed);

        // 보상
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);

        if (controlSignal.z < 0)
        {
            Done();
            this.transform.position = new Vector3(0, 0, 0);
            this.transform.rotation = Quaternion.identity;
        }

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

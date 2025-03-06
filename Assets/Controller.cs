using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[] motors;
    HingeJoint[] joints;
    public GameObject gameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joints = new HingeJoint[motors.Length];
        for (int i = 0; i < motors.Length; i++)
        {
            var hinge = motors[i].GetComponent<HingeJoint>();

            // Make the hinge motor rotate with 90 degrees per second and a strong force.
            var motor = hinge.motor;
            motor.force = 500;
            motor.targetVelocity = 0;
            motor.freeSpin = false;
            hinge.motor = motor;
            hinge.useMotor = true;
            joints[i] = hinge;
        }
    }

    // Update is called once per frame
    float θ = -Mathf.PI / 6;
    float φ = 0;
    public GameObject dir;
    //float[] θ=new float[3];
    bool auto = false;
    (int x, int z) nowPoint = (0, 0);
    List<Node> nodes = new();
    int count = 0;//nowPoint用
    void Update()
    {
        float Vx = 0, Vy = 0;
        float ω = 0;
        // φ = Vector3.SignedAngle(new Vector3(0,0,100).normalized, (new Vector3(dir.transform.position.x, this.transform.position.y, dir.transform.position.z) - this.transform.position).normalized, new Vector3(0, -1, 0));
        // Debug.DrawLine(new Vector3(0,0,100)+this.transform.position,this.transform.position);
        // Debug.DrawLine(new Vector3(dir.transform.position.x,this.transform.position.y,dir.transform.position.z),this.transform.position);
        // //Debug.Log(φ);
        // var a = MathF.Atan2(nowPoint.z - this.transform.position.z, nowPoint.x - this.transform.position.x) - φ / 180f * MathF.PI;
        // //Debug.Log(MathF.Atan2(nowPoint.z-this.transform.position.z,nowPoint.x-this.transform.position.x)*180/MathF.PI);
        var a=Math.PI/180*Vector3.SignedAngle(new Vector3(dir.transform.position.x,this.transform.position.y,dir.transform.position.z)-this.transform.position,new Vector3(nowPoint.x - this.transform.position.x,0,nowPoint.z - this.transform.position.z),Vector3.up);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            auto = !auto;
            var nowNode = AStar.SolveAStar(gameObject.GetComponent<障害物>().map);
            nodes.Clear();
            while (nowNode.kind != NodeKind.start)
            {
                nowNode.position=(nowNode.position.j-250,nowNode.position.i-250);
                nodes.Add(nowNode);
                nowNode = nowNode.parent;
            }
            nodes.Reverse();
            nowPoint=nodes[0].position;
        }
        //Debug.Log(nodes.Count);
        for (int i = nodes.Count == 0 ? 0 : 1; i < nodes.Count; i++)
        {
            Debug.DrawLine(new Vector3(nodes[i].position.j, 1, nodes[i].position.i), new Vector3(nodes[i - 1].position.j, 1, nodes[i - 1].position.i));
        }
        if (auto)
        {
            var _distance = distance((nowPoint.x, nowPoint.z), (this.transform.position.x, this.transform.position.z));
            if (_distance < 1f && count != nodes.Count)
            {
                count++;
                nowPoint = nodes[count].position;
                _distance = distance((nowPoint.x, nowPoint.z), (this.transform.position.x, this.transform.position.z));
            }
            Debug.Log(nowPoint);
            Vy = 1;
            ω = 2f * Vy * MathF.Sin((float)a) / _distance;
            //Debug.Log(ω);
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vy = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vy = -1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vx = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vx = 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                ω = -1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                ω = 1;
            }
        }

        //θ+=(2*ω*Time.deltaTime);

        for (int i = 0; i < motors.Length; i++)
        {
            var motor = joints[i].motor;
            motor.targetVelocity = Calc(150 * Vx, 150 * Vy, 150 * ω, 4 * Mathf.PI / 3 + 2 * Mathf.PI / 3 * i, 1.25f);
            joints[i].motor = motor;
        }
    }

    float Calc(float Vx, float Vy, float ω, float angle, float R) => Mathf.Cos(angle) * Vx + Mathf.Sin(angle) * Vy + R * ω;
    float distance((float x, float y) point1, (float x, float y) point2) => MathF.Sqrt(MathF.Pow(point1.x - point2.x, 2) + MathF.Pow(point1.y - point2.y, 2));
}

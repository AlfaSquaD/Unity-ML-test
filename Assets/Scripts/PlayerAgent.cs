using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{

    [SerializeField] private Transform ballTransform;
    [SerializeField] private Collider2D ownGoal;
    [SerializeField] private Collider2D enemyGoal;
    [SerializeField] private Transform enemyTransform;

    private void Start()
    {
        transform.position = new Vector3(-5, -6,0);
    }
    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(-5, -6, 0);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ballTransform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(enemyTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0] * 2f;
        float moveY = actions.ContinuousActions[1] * 2f;
        float moveSpeed = 1f;
        transform.position += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            continousActions[0] = mousePos.x;
            continousActions[1] = mousePos.y;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "puck")
        {
            AddReward(2f);
            EndEpisode();
        }
        else if(collision.gameObject.tag == "border")
        {
            AddReward(-1f);
        }

    }
}

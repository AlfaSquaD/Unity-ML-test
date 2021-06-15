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
    private PlayerMovement pm;

    private void Start()
    {
        pm = gameObject.GetComponent<PlayerMovement>();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ballTransform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(enemyTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.ContinuousActions);
        pm.movePlayerAgent(new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]));
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
}

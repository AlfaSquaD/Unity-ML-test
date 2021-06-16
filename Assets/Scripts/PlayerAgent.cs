using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    [SerializeField] private int direction;
    [SerializeField] private GameObject puck;
    [SerializeField] private Collider2D ownGoal;
    [SerializeField] private Collider2D enemyGoal;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private float agentSpeed;
    private PlayerMovement playerMovement;
    private PuckScript puckScript;
    private Vector2 startPos;

    public override void Initialize()
    {
        startPos = gameObject.transform.localPosition;
        puckScript = puck.GetComponent<PuckScript>();
        puckScript.setRandomPos();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPos;
        puckScript.setRandomPos();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(puck.transform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(enemyTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp01(actions.ContinuousActions[0]);
        float moveY = Mathf.Clamp01(actions.ContinuousActions[1]) * direction;
        Vector2 pos = transform.position;
        Vector2 nPos = pos + (new Vector2(moveX, moveY) * Time.deltaTime * (agentSpeed * Mathf.Clamp01(actions.ContinuousActions[2])));
        playerMovement.rb.MovePosition(nPos);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");
    }


    public void halfAreaPunisment()
    {
        AddReward(0f);
    }

    public void goalReward()
    {
        Debug.Log("Goalllll!!");
        SetReward(1f);
        EndEpisode();
    }
}

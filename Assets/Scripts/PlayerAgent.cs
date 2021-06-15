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
    private Vector2 startPos;
    private Vector2 puckStart;

    public override void Initialize()
    {
        startPos = gameObject.transform.localPosition;
        puckStart = puck.transform.localPosition;
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPos;
        puck.transform.localPosition = puckStart;
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
        float moveY = Mathf.Clamp01(actions.ContinuousActions[1]);
        Vector2 pos = transform.localPosition;
        Vector2 nPos = pos + (new Vector2(moveX, moveY) * Time.deltaTime * (agentSpeed * Mathf.Clamp01(actions.ContinuousActions[2])));
        if (nPos.x > playerMovement.playArea.bounds.max.x || nPos.x < playerMovement.playArea.bounds.min.x || nPos.y > playerMovement.playArea.bounds.max.y || nPos.y < playerMovement.playArea.bounds.min.y)
        {
            nPos.x = Mathf.Clamp(nPos.x, playerMovement.playArea.bounds.min.x, playerMovement.playArea.bounds.max.x);
            nPos.y = Mathf.Clamp(nPos.y, playerMovement.playArea.bounds.min.y, playerMovement.playArea.bounds.max.y);
        }
        nPos = transform.TransformPoint(new Vector3(nPos.x, nPos.y));
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
        AddReward(-0.1f);
    }

    public void goalReward()
    {
        Debug.Log("Goalllll!!");
        SetReward(1f);
        EndEpisode();
    }
}

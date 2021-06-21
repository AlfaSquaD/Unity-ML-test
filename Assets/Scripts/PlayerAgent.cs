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
    [SerializeField] private GameObject enemy;
    [SerializeField] private float agentSpeed;
    private float timeCounter = 10;
    private PlayerAgent enemyAgent;
    private PlayerMovement playerMovement;
    private PuckScript puckScript;
    private Vector2 startPos;
    bool player = false;

    public override void Initialize()
    {
    
        startPos = gameObject.transform.localPosition;
        puckScript = puck.GetComponent<PuckScript>();;
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        enemyAgent = enemy.GetComponent<PlayerAgent>();
    }

    public override void OnEpisodeBegin()
    {
        player = false;
        puck.transform.localPosition = startPos;
        transform.localPosition = startPos;
        puckScript.setRandomPos();
        timeCounter = 10;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(puck.transform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(enemy.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // TODO: Ikiside dogruymus.
        // Add punishment for absurd Values
        //float moveX = (0.5f -Mathf.Clamp01(actions.ContinuousActions[0])) * 2;
        //float moveY = (0.5f - Mathf.Clamp01(actions.ContinuousActions[1])) * 2;
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -3f, 3f);
        float moveY = Mathf.Clamp(actions.ContinuousActions[1], -3f, 3f);
        Vector2 pos = transform.position;
        Vector2 nPos = pos + (new Vector2(moveX, moveY) * Time.deltaTime * (agentSpeed * Mathf.Clamp01(actions.ContinuousActions[2])));
        playerMovement.rb.MovePosition(nPos);
        AddReward(-0.0002f);
        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;
        }
        else
        {
            resetEnvironment();
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("puck"))
        {
            if (!player) 
            {
                AddReward(0.2f);
                player = true;
            }
        }
    }

    public void halfAreaPunisment()
    {
        AddReward(-0.0001f);
    }

    public void goalReward()
    {
        Debug.Log("Goalllll!!");
        SetReward(1f);
        enemyAgent.SetReward(-1f);
        EndEpisode();
        enemyAgent.EndEpisode();
    }

    // TODO: Reset without
    public void resetEnvironment()    
    {
        player = false;
        puck.transform.localPosition = startPos;
        transform.localPosition = startPos;
        puckScript.setRandomPos();
        timeCounter = 10;
    }
}

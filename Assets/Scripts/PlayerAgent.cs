using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{

    [SerializeField] private Transform ballTransform;
    [SerializeField] private Collider2D ownGoal;
    [SerializeField] private Collider2D enemyGoal;
    [SerializeField] private Transform enemyTransform;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ballTransform.localPosition);
        sensor.AddObservation(gameObject.transform.localPosition);
        sensor.AddObservation(enemyTransform.localPosition);
    }
}

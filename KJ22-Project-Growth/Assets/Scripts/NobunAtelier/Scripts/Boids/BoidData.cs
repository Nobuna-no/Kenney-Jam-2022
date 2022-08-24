using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "[BoidData]")]
public class BoidData : ScriptableObject
{
    public int MaxBoidsCount = 50;
    // Boids' detection distance with other objects.
    public float BoidSpeed = 10f;
    [Min(0.1f)]
    [Header("- Boids' detection distance with other objects.")]
    public float DetectionDistanceSquared = 5.0f * 5.0f;
    // Boids' detection distance with other objects.
    public float SeparationDistanceSquared = 1.0f * 1.0f;
    
    [Header("Predator")]
    [Header("- Boids' safety distance from the predator.")]
    [Min(0.1f)]
    public float SafetyDistanceSquared = 5.0f * 5.0f;
    [Header("- Boid's predator avoidance percentage per frame.")]
    public float FleeingPredatorPercentage = 50.0f;

    // Boid's grouping movement percentage per frame.
    public float GroupingPercentage = 5.0f;
    // Boid's separation fGameObject.
    public float SeparationFGameObject = 1.0f;
    // Boid's cohesion movement percentage per frame.
    public float CohesionPercentage = 50.0f;
    // Force applied to the boid when it encounters a border. 
    public float BorderBouncingForce = 30.0f;
    
    
    public float GoodPlaceAttractionFactor = 10.0f;
    public float MaxVelocity = 20.0f;
}

using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A class design to manage boids behaviour.
/// /// </summary>
public class BoidsManager : MonoBehaviour
{
    #region PROPERTIES
    private static BoidsManager m_instance = null;
    // Singleton instantiated on demand.
    public static BoidsManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<BoidsManager>();// new BoidsManager(m_BoidData.MaxBoidsCount);
            }
            return m_instance;
        }
    }

    [SerializeField]
    private BoidData m_BoidData;
    public BoidData Data { get { return m_BoidData; } }
    public List<GameObject> Boids; // Generic simulated flocking GameObjects.
    public List<GameObject> Predators; // GameObject to avoid as boid.
    #endregion


    #region CONSTRUCTOR
    // private constructor. Only available for singleton instantiation.
    private BoidsManager(int boidsCount)
    {
        Boids = new List<GameObject>(boidsCount);
        Predators = new List<GameObject>();
    }
    #endregion


    #region MAIN BOIDS RULES METHODS
    // Rule 1: Boids try to fly towards the centre of mass of neighbouring boids.
    private Vector2 BoidsGrouping(GameObject currentBoid)
    {
        GameObject b;
        Vector2 vel = Vector2.zero;
        int boidsCount = 0;

        for (int i = 0, c = Boids.Count; i < c; ++i)
        {
            b = Boids[i];
            if (b != currentBoid
                && Vector2.SqrMagnitude(b.transform.position - currentBoid.transform.position) < m_BoidData.DetectionDistanceSquared)
            {
                vel += new Vector2(b.transform.position.x, b.transform.position.y);
                ++boidsCount;
            }
        }

        if (boidsCount == 0)
        {
            return vel;
        }

        // Compute direction vector to nearby boids center. Moving 1% in that direction.
        vel = ((vel / boidsCount - new Vector2(currentBoid.transform.position.x, currentBoid.transform.position.y)) * 0.01f);

        return vel * m_BoidData.GroupingPercentage;
    }

    // Rule two: Boids try to keep a small distance away from other objects (including other boids)
    private Vector2 BoidsSeparation(GameObject currentBoid)
    {
        GameObject b;
        Vector2 seprationForce = Vector2.zero;

        for (int i = 0, c = Boids.Count; i < c; ++i)
        {
            b = Boids[i];
            if (b != currentBoid)
            {
                float distanceSquared = Vector2.SqrMagnitude(b.transform.position - currentBoid.transform.position);
                if (distanceSquared < m_BoidData.DetectionDistanceSquared && distanceSquared < m_BoidData.SeparationDistanceSquared)
                {
                    seprationForce -= new Vector2((b.transform.position - currentBoid.transform.position).x, (b.transform.position - currentBoid.transform.position).y);
                }
            }
        }

        return seprationForce * m_BoidData.SeparationFGameObject;
    }

    // Rule three: Boids try to match velocity with near boids.
    private Vector2 BoidsCohesion(GameObject currentBoid)
    {
        GameObject b;
        Vector2 perceivedVelocity = Vector2.zero;
        int boidsCount = 0;

        for (int i = 0, c = Boids.Count; i < c; ++i)
        {
            b = Boids[i];
            if (b != currentBoid
                && Vector2.SqrMagnitude(b.transform.position - currentBoid.transform.position) < m_BoidData.DetectionDistanceSquared)
            {
                perceivedVelocity += b.GetComponent<Rigidbody2D>().velocity;
                ++boidsCount;
            }
        }

        if (boidsCount == 0)
        {
            return perceivedVelocity;
        }

        perceivedVelocity /= Boids.Count - 1;

        return (perceivedVelocity - currentBoid.GetComponent<Rigidbody2D>().velocity) * 0.01f * m_BoidData.CohesionPercentage;
    }


    /// <summary>
    /// Combined version of the 3 main boids rules: Output the grouping, sepration and cohesion accelerations.
    /// </summary>
    /// <param name="currentBoid">Targeted boid.</param>
    /// <param name="out_groupingAcc"></param>
    /// <param name="out_separationAcc"></param>
    /// <param name="out_cohesionAcc"></param>
    public void BoidsGSC(GameObject currentBoid, out Vector2 out_groupingAcc, out Vector2 out_separationAcc, out Vector2 out_cohesionAcc)
    {
        GameObject b;
        out_groupingAcc = out_separationAcc = out_cohesionAcc = Vector2.zero;
        int boidsCount = 0;

        for (int i = 0, c = Boids.Count; i < c; ++i)
        {
            b = Boids[i];
            float distanceSquared = Vector2.SqrMagnitude(b.transform.position - currentBoid.transform.position);

			bool distanceOk = distanceSquared < m_BoidData.DetectionDistanceSquared;
			if (b != currentBoid && distanceOk)
            {
                out_groupingAcc += new Vector2(b.transform.position.x, b.transform.position.y);
                out_cohesionAcc += b.GetComponent<Rigidbody2D>().velocity;
                ++boidsCount;

                if (distanceSquared < m_BoidData.SeparationDistanceSquared)
                {
                    out_separationAcc -= new Vector2((b.transform.position - currentBoid.transform.position).x, (b.transform.position - currentBoid.transform.position).y);
                }
            }
        }

        if (boidsCount == 0)
        {
            return;
        }

        // Compute direction vector to nearby boids center. Moving 1 + X% in that direction.
        out_groupingAcc = (out_groupingAcc / boidsCount - new Vector2(currentBoid.transform.position.x, currentBoid.transform.position.y)) * 0.01f * m_BoidData.GroupingPercentage;

        // Compute direction vector to average boids velocity. Moving 1 + X% in that direction.
        out_cohesionAcc = (out_cohesionAcc / boidsCount - currentBoid.GetComponent<Rigidbody2D>().velocity) * 0.01f * m_BoidData.CohesionPercentage;

        out_separationAcc *= m_BoidData.SeparationFGameObject;
    }
    #endregion


    #region ADDITIONAL RULES METHODS
    /// <summary>
    /// Return a velocity to move away from a predator.
    /// </summary>
    /// <param name="currentBoid">Targeted boid.</param>
    /// <returns>Velocity vector to add to boid's velocity.</returns>
    public Vector2 AvoidPredator(GameObject currentBoid)
    {
        if (Predators.Count == 0)
        {
            return Vector2.zero;
        }

        float lastSquaredDistance = float.MaxValue;
        Vector2 predatorPosition = Vector2.zero;
        for (int i = 0, c = Predators.Count; i < c; ++i)
        {
            GameObject p = Predators[i];
            float squaredDistance = Vector3.SqrMagnitude(p.transform.position - currentBoid.transform.position);

            if (squaredDistance < m_BoidData.SafetyDistanceSquared && squaredDistance < lastSquaredDistance)
            {
                lastSquaredDistance = squaredDistance;
                predatorPosition = new Vector2(p.transform.position.x, p.transform.position.y);
            }
        }

        if (predatorPosition == Vector2.zero)
        {
            return predatorPosition;
        }

        // 1% moving to the place. FGameObject -1 to move avay from it.
        return (predatorPosition - new Vector2(currentBoid.transform.position.x, currentBoid.transform.position.y)) * -0.01f * m_BoidData.FleeingPredatorPercentage;
    }

    /// <summary>
    /// Encourages boid to stay within rough boundaries. 
    /// That way they can fly out of them, but then slowly turn back, avoiding any harsh motions. 
    /// </summary>
    /// <param name="currentBoid">Targeted boid.</param>
    /// <returns>Velocity vector to add to boid's velocity.</returns>
    //public static Vector2 BorderBouncing(GameObject currentBoid)
    //{
    //    const float borderOffset = 50.0f;
    //    float xMin = borderOffset, xMax = Application.Current.MainWindow.ActualWidth - borderOffset,
    //            yMin = borderOffset, yMax = Application.Current.MainWindow.ActualHeight - borderOffset * 2.0;

    //    Vector v = MathUtils.ZeroVector;

    //    if (currentBoid.Position.X < xMin)
    //    {
    //        v.X = m_BoidData.BorderBouncingForce;
    //    }
    //    else if (currentBoid.Position.X > xMax)
    //    {
    //        v.X = -m_BoidData.BorderBouncingForce;
    //    }
    //    if (currentBoid.Position.Y < yMin)
    //    {
    //        v.Y = m_BoidData.BorderBouncingForce;
    //    }
    //    else if (currentBoid.Position.Y > yMax)
    //    {
    //        v.Y = -m_BoidData.BorderBouncingForce;
    //    }

    //    return Vector2.zero;
    //}
    #endregion


    #region UTILS
    /// <summary>
    /// Player take possesion of boid around the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius">Distance around the position.</param>
    public void PossessBoidAround(Vector2 position, float radius)
    {
        /*  for (int i = 0, c = Boids.Count; i < c; ++i)
          {
              if ((Boids[i].transform.position - position).LengthSquared < MathUtils.Square(radius))
              {
                  Boids[i].PlayerPossession();
                  return;
              }
          }*/
    }

    /// <summary>
    /// Player loses possession of a boid around a given position. The boid becomes controlled by an autonomous controller.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius">Distance around the position.</param>
    public void UnpossessPredatorAround(Vector2 position, float radius)
    {
        /*for (int i = 0, c = Predators.Count; i < c; ++i)
        {
            if ((Predators[i].Position - position).LengthSquared < MathUtils.Square(radius))
            {
                Predators[i].AutonomousPossession();
                return;
            }
        }*/
    }

    /// <summary>
    /// Player loses possession of all predators. They become controlled by autonomous controllers.
    /// </summary>
    public void UnpossessAllPredators()
    {
        /*for (int i = Predators.Count - 1; i >= 0; --i)
        {
            Predators[i].AutonomousPossession();
        }*/
    }
    #endregion
}

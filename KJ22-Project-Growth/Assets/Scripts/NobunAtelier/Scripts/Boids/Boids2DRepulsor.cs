using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NobunAtelier
{
    public abstract class Boids2DRepulsor : MonoBehaviour
    {
        public abstract Vector2 GetRepulsionVector(Vector2 boidPosition);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NobunAtelier
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Boids2DBoxRepulsor : Boids2DRepulsor
    {
        private static UnityAction s_ToggleDebugAction;

        [SerializeField]
        private Vector2 m_repulsionVector = Vector2.zero;
        [Header("Debug")]
        [SerializeField]
        private bool m_normalizedOnValidate = true;
        [SerializeField]
        private bool m_drawGizmos = false;

        public override Vector2 GetRepulsionVector(Vector2 boidPosition)
        {
            return m_repulsionVector;
        }

        private void OnValidate()
        {
            if(m_normalizedOnValidate)
            {
                m_repulsionVector.Normalize();
            }
        }

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        [SerializeField]
        List<float> valuesOfCalc = new List<float>();

        private void OnDrawGizmos()
        {
            if(!m_drawGizmos)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            var collider = GetComponent<BoxCollider2D>();
            Vector2 scaledSize = collider.size * transform.localScale;

            Gizmos.DrawWireCube(transform.position, scaledSize);

            GizmosUtility.DrawArrow(transform.position, m_repulsionVector);
            //GizmosUtility.DrawArrow(transform.position + new Vector3(scaledSize.x, scaledSize.y) * 0.5f, m_repulsionVector);
            //GizmosUtility.DrawArrow(transform.position + new Vector3(-scaledSize.x, scaledSize.y) * 0.5f, m_repulsionVector);
            //GizmosUtility.DrawArrow(transform.position + new Vector3(scaledSize.x, -scaledSize.y) * 0.5f, m_repulsionVector);
            //GizmosUtility.DrawArrow(transform.position + new Vector3(-scaledSize.x, -scaledSize.y) * 0.5f, m_repulsionVector);
        }
    }

    public static class GizmosUtility
    {
        public static void DrawArrow(Vector3 position, Vector3 direction)
        {
            Gizmos.DrawRay(position, direction);

            Vector3 headPosition = position + direction;
            Vector2 dir = new Vector2(direction.y + -direction.x, -direction.x).normalized * 0.3f;

            Gizmos.DrawRay(headPosition, dir);

            dir.y *= -1;
            Gizmos.DrawRay(headPosition, dir);
        }
    }
}
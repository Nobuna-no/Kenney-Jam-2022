using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
	[Header("Character Controller")]
	[InfoBox("Provides basic method to control character.")]
	[HorizontalLine(1, EColor.Orange)]
	
	[SerializeField]
	private Vector2 m_maxMoveSpeed = new Vector2(5f, 10f);
	[SerializeField, Min(0.1f)]
	private float m_moveSpeed = 1;
	[SerializeField, Min(0.1f)]
	private float m_moveVerticalSpeed = 1;

	[SerializeField, Min(1)]
	private float m_jumpForce = 10;

	[BoxGroup("Debug")]
	[SerializeField]
	private bool m_useDebugInputs = false;
	[BoxGroup("Debug"), InputAxis, ShowIf("ShowDebug")]
	[SerializeField]
	private string m_moveAxis;
	[BoxGroup("Debug"), InputAxis, ShowIf("ShowDebug")]
	[SerializeField]
	private string m_moveVerticalAxis;
	[BoxGroup("Debug"), ShowIf("ShowDebug")]
	[SerializeField]
	private KeyCode m_jumpKey;

	[BoxGroup("Debug")]
	[SerializeField]
	[ReadOnly]
	private Vector2 m_lastGroundNormal = Vector2.up;
	private bool m_stickToTheCeiling = false;

	[SerializeField]
	private float m_stickinessVel = 0.75f;

	private Rigidbody2D m_body;
	private GrowthComponent m_growthComponent;
	private Vector2 m_inputMovement = Vector2.zero;
	
	private RaycastHit2D[] hits = new RaycastHit2D[5];
	private bool m_canJump = true;

    private void Awake()
    {
		m_body = GetComponent<Rigidbody2D>();
		m_growthComponent = GetComponent<GrowthComponent>();
	}

    private void Update()
    {
		if (m_useDebugInputs)
        {
			m_inputMovement.x = Input.GetAxisRaw(m_moveAxis);
			m_inputMovement.y = Input.GetAxisRaw(m_moveVerticalAxis);
		}
    }


	private void FixedUpdate()
    {
		if (m_inputMovement != Vector2.zero)
        {
			MoveHorizontal(m_inputMovement.x);
			MoveVertical(m_inputMovement.y);
			m_inputMovement = Vector2.zero;
        }

		m_body.velocity = new Vector2(Mathf.Clamp(m_body.velocity.x, -m_maxMoveSpeed.x, m_maxMoveSpeed.x),
		    Mathf.Clamp(m_stickToTheCeiling ? m_stickinessVel : m_body.velocity.y, -m_maxMoveSpeed.y, m_maxMoveSpeed.y));
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.contacts[0];
		Debug.DrawRay(contact.point, contact.normal, Color.red, 1f);
		
		// If edges or vertical wall.
		if (Mathf.Abs(contact.normal.x) >= 0.2f)
		{
			m_lastGroundNormal = new Vector2(contact.normal.x, 1).normalized;
			m_canJump = true;
		}
		// If ceiling or vertical wall.
		else if (contact.normal.y < -0.75f)
        {
			m_body.velocity = Vector2.right * m_body.velocity;
			m_body.gravityScale = 0;
			m_lastGroundNormal = Vector2.down;
			m_stickToTheCeiling = true;
			m_canJump = false;
		}
		// Ground floor.
		else
        {
			m_lastGroundNormal = Vector2.up;
			m_canJump = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (m_stickToTheCeiling)
        {
			m_stickToTheCeiling = false;
			m_body.gravityScale = 1;
		}
		m_lastGroundNormal = Vector2.up;
	}

    public void Move(Vector2 normalizedValue)
    {
		MoveHorizontal(normalizedValue.x);
		MoveVertical(normalizedValue.y);
	}

	public void MoveHorizontal(float normalizedValue)
	{
		m_body.velocity += Vector2.right * (normalizedValue * m_moveSpeed);
	}

	public void MoveVertical(float normalizedValue)
	{
		if (normalizedValue == 0)
        {
			return;
        }

		if (normalizedValue > 0 && !m_stickToTheCeiling)
        {
			Jump();
        }
		else
        {
			if (m_stickToTheCeiling)
			{
				m_body.gravityScale = 1;
				m_lastGroundNormal = Vector2.zero;
				m_stickToTheCeiling = false;
			}

			m_body.velocity += Vector2.up * (normalizedValue * m_moveVerticalSpeed);
        }
	}

	public void Jump()
	{
		if (m_canJump)
        {
			m_canJump = false;
			// Reset Y vel and add jump force.
			m_body.velocity = Vector2.right * m_body.velocity + m_lastGroundNormal * m_jumpForce;
		}
	}

	private bool ShowDebug()
    {
		return m_useDebugInputs;

	}
}

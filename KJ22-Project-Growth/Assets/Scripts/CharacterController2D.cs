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


	[BoxGroup("Physics")]
	[SerializeField, Required]
	private Transform m_groundDetectorTransform;
	[BoxGroup("Physics")]
	[SerializeField]
	private float m_rayLenght = 0.1f;

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
	private Vector2 CurrentVel;

	private Rigidbody2D m_body;
	private Vector2 m_inputMovement = Vector2.zero;
	
	private RaycastHit2D[] hits = new RaycastHit2D[5];
	private bool m_canJump = false;

    private void Start()
    {
		m_body = GetComponent<Rigidbody2D>();
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
		    Mathf.Clamp(m_body.velocity.y, -m_maxMoveSpeed.y, m_maxMoveSpeed.y));

		if(m_body.velocity.y < 0 && !m_canJump)
        {
			float size = GetComponent<Growth>().GetSizeRatio();
			Vector2 origin = transform.position + (Vector3.down * (size));
			int hitCount = Physics2D.RaycastNonAlloc(origin, Vector2.down, hits, m_rayLenght);
			if (hitCount > 0)
			{
				m_canJump = true;
			}

			Debug.DrawRay(origin, Vector2.down * m_rayLenght, Color.red);
        }
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

		if (normalizedValue > 0)
        {
			Jump();
        }
		else
        {
			m_body.velocity += Vector2.up * (normalizedValue * m_moveVerticalSpeed);
        }
	}

	public void Jump()
	{
		if (m_canJump)
        {
			m_canJump = false;
			m_body.velocity += Vector2.up * m_jumpForce;
        }
	}

	private bool ShowDebug()
    {
		return m_useDebugInputs;

	}
}

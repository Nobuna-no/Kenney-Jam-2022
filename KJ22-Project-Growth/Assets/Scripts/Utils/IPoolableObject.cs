using System;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Extension/Pool Object")]
public abstract class IPoolableObject : MonoBehaviour
{
	[SerializeField]
	private PoolObjectID m_objectID;
	public PoolObjectID ID => m_objectID;

	// Called when the object is reset by the pool.
	public event Action onReset = null;
	// Called each time the object is activated by the pool.
	public event Action onActivation = null;
	// Called each time the object is deactivated by the pool.
	public event Action onDeactivation = null;

	public int PrefabIndex
	{
		get; set;
	}

	public bool IsActive
	{
		get
		{
			return gameObject.activeSelf;
		}
		set
		{
			if (IsActive == value)
				return;

			if (value)
            {
				onActivation?.Invoke();
            }
			else
            {
				onDeactivation?.Invoke();
            }

			gameObject.SetActive(value);
		}
	}

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
		set
		{
			transform.position = value;
		}
	}

	public void ResetObject()
	{
		onReset?.Invoke();
		gameObject.SetActive(false);
	}

	protected virtual void OnReset()
    { }

	protected virtual void OnActivation()
	{ }

	protected virtual void OnDeactivation()
	{ }

	// Not reliable
	private void Awake()
	{
		onReset += OnReset;
		onActivation += OnActivation;
		onDeactivation += OnDeactivation;
	}

	// Not reliable
	// This is call when the object is instantiate.
	// We don't want to start doing it now as we don't know if the object is ready to be use...
	private void Start()
    { }

	// Not reliable
	// This is call when the object is instantiate.
	// We don't want to start doing it now as we don't know if the object is ready to be use...
	private void OnEnable()
	{
		// onEnable?.Invoke();
	}

	// Not reliable
	private void OnDisable()
	{
		// onDisable?.Invoke();
	}
}

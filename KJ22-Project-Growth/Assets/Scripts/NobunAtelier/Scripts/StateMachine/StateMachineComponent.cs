using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NobunAtelier
{
    public class StateMachineComponent<T> : StateComponent<T>
        where T : StateDefinition
    {
        public T CurrentStateDefinition => m_activeStateDefinition;

        [Header("--- State Machine Debug ---")]
        [SerializeField]
        protected bool m_displayDebug = false;

        private Dictionary<T, StateComponent<T>> m_statesMap = new Dictionary<T, StateComponent<T>>();
        private T m_activeStateDefinition = null;
        private T m_activeDebugState = null;
        private Vector2 m_debugStateScrollPosition = Vector2.zero;

        protected override void Start()
        {
            base.Start();
        }

        public void ToggleDebug()
        {
            m_displayDebug = !m_displayDebug;
        }

        public void RegisterStateComponent(StateComponent<T> state)
        {
            m_statesMap.Add(state.GetStateDefinition(), state);
        }

        public override void SetState(T newState)
        {
            if(newState == m_activeStateDefinition)
            {
                return;
            }

            if (!m_statesMap.ContainsKey(newState))
            {
                base.SetState(newState);
                return;
            }

            m_statesMap[m_activeStateDefinition].Exit();
            m_activeStateDefinition = newState;
            m_statesMap[m_activeStateDefinition].Enter();
        }

        public override void Enter()
        {
            if (m_stateDefinition != null)
            {
                m_activeStateDefinition = m_stateDefinition;
                while (m_activeStateDefinition.RequiredPriorState != null)
                {
                    Debug.LogWarning($"Required condition <b>{m_activeStateDefinition.RequiredPriorState.name}</b> for state <b>{m_activeStateDefinition.name}</b>. " +
                        $"Rolling back state to <b>{m_activeStateDefinition.RequiredPriorState.name}</b>.");
                    m_activeStateDefinition = m_activeStateDefinition.RequiredPriorState as T;
                }

                if (!m_statesMap.ContainsKey(m_activeStateDefinition))
                {
                    Debug.LogError($"State machine doesn't have a valid StateComponent for state <b>{m_activeStateDefinition.name}</b>");
                }
                m_statesMap[m_activeStateDefinition].Enter();
            }
        }

        public override void Exit()
        {
            if (m_activeStateDefinition != null)
            {
                m_statesMap[m_activeStateDefinition].Exit();
            }
        }

        public override void Tick(float deltaTime)
        {
            if (m_activeStateDefinition == null)
            {
                return;
            }

            m_statesMap[m_activeStateDefinition].Tick(deltaTime);
        }

        protected virtual void OnGUI()
        {
            if (!Application.isPlaying || !m_displayDebug)
            {
                return;
            }

            GUILayout.BeginVertical(GUI.skin.window);
            {
                IMGUIUtility.DrawTitle(this.ToString());
                IMGUIUtility.DrawLabelValue("Current state", m_activeStateDefinition ? m_activeStateDefinition.name : "None");

                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        foreach (var a in m_statesMap)
                        {
                            if (GUILayout.Toggle(m_activeDebugState == a.Key, a.Key.name))
                            {
                                m_activeDebugState = a.Key;
                            }
                            else if(m_activeDebugState == a.Key)
                            {
                                m_activeDebugState = null;
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    if (m_activeDebugState != null)
                    {
                        m_debugStateScrollPosition = GUILayout.BeginScrollView(m_debugStateScrollPosition, GUI.skin.box);
                        m_statesMap[m_activeDebugState].StateDebugGUI();
                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
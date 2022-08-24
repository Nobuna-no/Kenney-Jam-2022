using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NobunAtelier
{
    public class GameActionConsole : MonoBehaviour
    {
        [System.Serializable]
        private class DebugData
        {
            public string MenuName;
            public string ButtonName;
            public UnityEvent Action;
        }

        [SerializeField]
        private string m_consoleName = "Game Console";
        [SerializeField]
        private KeyCode m_toggleKey = KeyCode.F2;
        [SerializeField]
        private DebugData[] m_data;
        private bool m_toggleDisplay = false;

        // Improve the system by adding a way to parse Menu name + depth system.
        // i.e. "Debug/AI/Boids", "Debug/AI/StateMachine"
        // Isn't a bit overkill? I mean, for a small game I don't think that much menu would be relevant.
        private Dictionary<string, List<DebugData>> m_hierarchy = new Dictionary<string, List<DebugData>>();
        private string m_selectedMenu = null;

        private void Start()
        {
            for (int i = 0; i < m_data.Length; ++i)
            {
                if (m_data[i].MenuName.Length == 0)
                {
                    m_hierarchy.Add(m_data[i].ButtonName, new List<DebugData>());
                    m_hierarchy[m_data[i].ButtonName].Add(m_data[i]);
                    continue;
                }

                if (!m_hierarchy.ContainsKey(m_data[i].MenuName))
                {
                    m_hierarchy.Add(m_data[i].MenuName, new List<DebugData>());
                }
                m_hierarchy[m_data[i].MenuName].Add(m_data[i]);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(m_toggleKey))
            {
                m_toggleDisplay = !m_toggleDisplay;
            }
        }

        Vector2 position = Vector2.zero;
        [SerializeField]
        Vector2 size = new Vector2(150, 0);
        [SerializeField]
        Rect currentRect;
        float originalHeight = 0;
        bool firstResize = true;

        bool needScrollbar = false;
        private void OnGUI()
        {
            if(!m_toggleDisplay)
            {
                return;
            }

            needScrollbar = size.y >= Screen.safeArea.height;
            if (needScrollbar)
            {
                size.x = Screen.safeArea.width * 0.3f;
                size.y = Screen.safeArea.height;
            }

            Rect screenRect = new Rect(position, size);
            currentRect = GUILayout.Window(0, screenRect, OnWindow, m_consoleName);
            if(currentRect.size.y > 0 && firstResize)
            {
                firstResize = false;
                originalHeight = currentRect.size.y;
            }
            
            size = new Vector2(currentRect.size.x, currentRect.size.y);

            if(position == currentRect.position && m_selectedMenu == null)
            {
                
                size.y = originalHeight;
            }
            else
            {
                position = new Vector2(
                    Mathf.Clamp(currentRect.position.x, Screen.safeArea.x, Screen.safeArea.width - size.x),
                    Mathf.Clamp(currentRect.position.y, Screen.safeArea.y, Screen.safeArea.height - size.y));
            }
        }
        Vector2 scrollPosition = Vector2.zero;
        void OnWindow(int id)
        {
            if(needScrollbar)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            }

            GUILayout.BeginVertical();

            foreach (var d in m_hierarchy)
            {
                if(d.Key == d.Value[0].ButtonName)
                {
                    if (GUILayout.Button(d.Key))
                    {
                        d.Value[0].Action?.Invoke();
                    }
                }
                else if (GUILayout.Toggle(m_selectedMenu == d.Key, $"<b>{d.Key}</b>"))
                {
                    // if toggle and that selected is already d.Key
                    m_selectedMenu = d.Key;
                    
                    GUILayout.BeginHorizontal(/*GUI.skin.box*/);
                    {
                        GUILayout.VerticalSlider(-1, 0, 0, GUILayout.MinHeight(1));
                        GUILayout.BeginVertical();
                        {
                            foreach (var a in d.Value)
                            {
                                if (GUILayout.Button(a.ButtonName))
                                {
                                    a.Action?.Invoke();
                                }
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
                else if (m_selectedMenu == d.Key)
                {
                    m_selectedMenu = null;
                }
            }

            GUILayout.EndVertical();

            if (needScrollbar)
            {
                GUILayout.EndScrollView();
            }

            GUI.DragWindow();
        }
    }
}
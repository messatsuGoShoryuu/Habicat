using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jam
{
   

    public class DebugUI : MonoBehaviour
    {
        [SerializeField]
        Text m_text;

        private void Awake()
        {
            EventSystem.AddListener<Event_UIText>(OnTextUpdated);
        }

        private void OnTextUpdated(Event_UIText e)
        {
            if (e.add)
                m_text.text += ("\n" + e.text);
            else
                m_text.text = e.text;
        }
    }
}

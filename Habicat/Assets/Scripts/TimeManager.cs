using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public class TimerEntry
    {
        public TimerEntry(float tickRate)
        {
            this.tickRate = tickRate;
            this.lastTickTimestamp = Time.time;
        }

        public void Tick()
        {
            if (onTick != null)
                onTick();
        }

        public float tickRate;
        public float lastTickTimestamp;
        public event System.Action onTick;
    }

    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        private Dictionary<string, TimerEntry> m_timers = new Dictionary<string, TimerEntry>();

        private void Awake()
        {
            Instance = this;
        }


        public static void Create(string id, float tickRate)
        {
            if (Instance.m_timers.ContainsKey(id))
            {
                Instance.m_timers.Add(id, new TimerEntry(tickRate));
            }
        }

        public static void Register(string id, System.Action callback, float tickRate)
        {
            if (!Instance.m_timers.ContainsKey(id))
            {
                Instance.m_timers.Add(id, new TimerEntry(tickRate));
            }

            Instance.m_timers[id].onTick += callback;
        }

        public static void Unregister(string id, System.Action callback)
        {
            if (Instance.m_timers.ContainsKey(id))
            {
                Instance.m_timers[id].onTick -= callback;
            }
        }

        void Update()
        {
            foreach (KeyValuePair<string, TimerEntry> kv in m_timers)
            {
                TimerEntry entry = kv.Value;
                float lastTime = entry.lastTickTimestamp;
                float now = Time.time;
                if (now - lastTime > entry.tickRate)
                {
                    entry.Tick();
                    entry.lastTickTimestamp = Time.time;
                }
            }
        }
    }
}
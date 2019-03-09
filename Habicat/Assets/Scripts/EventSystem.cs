using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public class BaseEventData
    {
        public BaseEventData()
        { }
    }

    public class GenericEvent<T> where T : BaseEventData
    {
        public delegate void Callback(T data);
        public static event Callback OnDispatch;

        public static void Dispatch(T data)
        {
            if (OnDispatch != null) OnDispatch(data);
        }
    }

    public class EventSystem
    {
        public static void AddListener<T>(GenericEvent<T>.Callback callback) where T : BaseEventData
        {
            GenericEvent<T>.OnDispatch += callback;
        }

        public static void RemoveListener<T>(GenericEvent<T>.Callback callback) where T : BaseEventData
        {
            GenericEvent<T>.OnDispatch -= callback;
        }

        public static void Dispatch<T>(T data) where T : BaseEventData
        {
            GenericEvent<T>.Dispatch(data);
        }
    }
}
/*
 * Date Created: Sunday, December 26, 2021 7:17 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    using ET = EventTriggerType;
    using Event = EventTrigger.TriggerEvent;

    [RequireComponent(typeof(EventTrigger))]
    [ExecuteInEditMode]
    public class EventTriggerExtender : MonoBehaviour
    {
        public EventTrigger trigger => GetComponent<EventTrigger>();
        public Event onPointerHold = new Event();

        public virtual Event pointerDown =>
            trigger?.EnsureEntry(ET.PointerDown).callback;
        public virtual Event pointerUp =>
            trigger?.EnsureEntry(ET.PointerUp).callback;

        protected BaseEventData _onPointerHoldData = null;

        public virtual void Awake()
        {
            if (trigger)
            {
                pointerDown.AddListener(_OnPointerHoldBegin);
                pointerUp.AddListener(_OnPointerHoldEnd);
            }
        }

        protected virtual void _OnPointerHoldBegin(BaseEventData data)
        {
            _onPointerHoldData = data;
        }

        protected virtual void _OnPointerHoldEnd(BaseEventData data)
        {
            _onPointerHoldData = null;
        }

        public virtual void Update()
        {
            if (_onPointerHoldData != null)
            {
                var data = _onPointerHoldData;
                onPointerHold?.Invoke(data);
            }
        }
    }
}
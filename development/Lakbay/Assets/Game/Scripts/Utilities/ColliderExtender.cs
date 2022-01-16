/*
 * Date Created: Saturday, January 1, 2022 10:14 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class ColliderExtender : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnter = new UnityEvent<Collider>();
        public UnityEvent<Collider> onTriggerStay = new UnityEvent<Collider>();
        public UnityEvent<Collider> onTriggerExit = new UnityEvent<Collider>();
        public UnityEvent<Collision> onCollisionEnter = new UnityEvent<Collision>();
        public UnityEvent<Collision> onCollisionStay = new UnityEvent<Collision>();
        public UnityEvent<Collision> onCollisionExit = new UnityEvent<Collision>();

        public virtual void OnTriggerEnter(Collider collider)
        {
            onTriggerEnter?.Invoke(collider);
        }

        public virtual void OnTriggerStay(Collider collider)
        {
            onTriggerStay?.Invoke(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            onTriggerExit?.Invoke(collider);
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter?.Invoke(collision);
        }

        public virtual void OnCollisionStay(Collision collision)
        {
            onCollisionStay?.Invoke(collision);
        }

        public virtual void OnCollisionExit(Collision collision)
        {
            onCollisionExit?.Invoke(collision);
        }
    }
}
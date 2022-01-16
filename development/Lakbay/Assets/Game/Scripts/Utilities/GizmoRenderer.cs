/*
 * Date Created: Thursday, December 30, 2021 1:45 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;

using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(Renderer))]
    public class GizmoRenderer : MonoBehaviour, IGizmoObject
    {
        public virtual new Renderer renderer => GetComponent<Renderer>();
        public List<int> materialIndices = new List<int>() { 0 };

        public void SetValue(float value)
        {
            foreach (var index in materialIndices)
            {
                var mat = renderer.materials[index];
                var color = mat.color;
                color.a = value;
                mat.color = color;
            }
        }
    }
}
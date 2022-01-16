/*
 * Date Created: Wednesday, January 5, 2022 5:10 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CinemachineSmoothPath), typeof(LineRenderer))]
    public class LinePath : MonoBehaviour
    {
        public virtual CinemachineSmoothPath path =>
            GetComponent<CinemachineSmoothPath>();
        public virtual LineRenderer line => GetComponent<LineRenderer>();
        public bool active = true;
        public bool realtimeDuringSceneMode = false;

        public virtual void Update()
        {
            if (path && line && active && (
                Application.isPlaying ||
                (realtimeDuringSceneMode && !Application.isPlaying)
            ))
            {
                UpdateLine();
            }
        }

        public virtual void OnValidate()
        {
            if (path && line && active && !realtimeDuringSceneMode)
            {
                UpdateLine();
            }
        }

        public virtual void UpdateLine()
        {
            line.positionCount = 0;
            var positions = new List<Vector3>();
            foreach (var wp in path.m_Waypoints)
            {
                positions.Add(wp.position);
            }
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());
        }
    }
}
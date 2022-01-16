/*
 * Date Created: Tuesday, January 4, 2022 4:21 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication
{
    using Core;

    [Serializable]
    public struct Point
    {
        public PointTrigger trigger;
        public List<Transform> obstacles;

        public void Hide() => SetVisibility(false);

        public void Show() => SetVisibility(true);

        public void SetVisibility(bool value)
        {
            trigger?.gameObject.SetActive(value);
            foreach (var obs in obstacles)
            {
                obs?.gameObject.SetActive(value);
            }
        }
    }

    public class SASteppedVehiclePlayer : SAVehicleParkingPlayer
    {
        [Space]
        public TargetTrigger target;
        public List<Point> points = new List<Point>();
        protected int _current = 0;
        public virtual int current => _current;
        public virtual Point currentPoint => points[current];
        public virtual Point nextPoint => current != points.Count - 1
            ? points[current + 1] : default;

        public override void OnTriggerStay(Collider collider)
        {
            base.OnTriggerStay(collider);
            if (!doneParking && !hitObstacle)
            {
                var point = collider.GetTrigger<PointTrigger>();
                if (point)
                {
                    float posOffset = Mathf.Abs(targetPositionOffset);
                    float rotOffset = Mathf.Abs(targetRotationOffset);
                    bool bothWays = targetRotationIsBothWays;
                    bool parked = ParkedProperly(
                        transform, point.transform,
                        posOffset, rotOffset, bothWays
                    );
                    if (point == currentPoint.trigger)
                    {
                        if (parked)
                        {
                            currentPoint.Hide();
                            arrowGuide?.gameObject.SetActive(true);
                            if (nextPoint.trigger)
                            {
                                nextPoint.Show();
                                SetGuide(nextPoint);
                                _current++;
                            }
                            else
                            {
                                SetGuide(target.transform);
                            }
                        }
                    }
                }
            }
        }

        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            var point = collider.GetTrigger<PointTrigger>();
        }

        public override void Awake()
        {
            base.Awake();
            foreach (var point in points) point.Hide();
        }

        public override void Build()
        {
            base.Build();
            if (points.Count != 0)
            {
                currentPoint.Show();
                SetGuide(currentPoint);
            }
            else
            {
                SetGuide(target.transform);
            }
        }

        public virtual void SetGuide(Point point)
        {
            if (point.trigger) SetGuide(point.trigger.transform);
        }
    }
}
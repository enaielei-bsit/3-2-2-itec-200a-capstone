/*
 * Date Created: Sunday, January 2, 2022 5:16 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication
{
    using Core;
    using UnityEngine.Animations;

    public class SAVehicleParkingPlayer : SAVehiclePlayer
    {
        protected bool _hitObstacle = false;
        public virtual bool hitObstacle => _hitObstacle;
        protected bool _doneParking = false;
        public virtual bool doneParking => _doneParking;

        public bool targetRotationIsBothWays = true;
        [Min(0.0f)]
        public float targetPositionOffset = 0.5f;
        [Min(0.0f)]
        public float targetRotationOffset = 5.0f;
        public LookAtConstraint arrowGuide;

        public virtual void Proceed()
        {
            LoadScene();
        }

        public override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
            var collider = collision.collider;
            var obstacle = collider.GetTrigger<ObstacleTrigger>();
            if (obstacle)
            {
                if (!hitObstacle && !doneParking)
                {
                    _hitObstacle = true;
                    if (vehicle) Reset();
                    // gameOverUI?.gameObject.SetActive(true);
                    OnObstacleHit();
                }
            }
        }

        public virtual void OnObstacleHit()
        {

        }

        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
        }

        public override void OnTriggerStay(Collider collider)
        {
            base.OnTriggerStay(collider);
            var target = collider.GetTrigger<TargetTrigger>();
            if (target && !hitObstacle)
            {
                var parked = ParkedProperly(
                    transform, target.transform,
                    targetPositionOffset, targetRotationOffset,
                    targetRotationIsBothWays
                );

                printLog($"Parked: {parked}");
                if (parked && !doneParking && Mathf.Floor(vehicle.Speed) == 0.0f)
                {
                    _doneParking = true;
                    Reset();
                    inGameUI?.gameObject.SetActive(false);
                    Reset();
                    OnPark();
                }
            }
        }

        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
        }

        public override void Update()
        {
            base.Update();
        }

        public static bool ParkedProperly(
            Transform vehicle,
            Transform location,
            float positionOffset = 0.0f,
            float rotationOffset = 0.0f,
            bool bothWays = true
        )
        {
            return PositionedProperly(
                vehicle.position, location.position, positionOffset)
                && RotatedProperly(
                    vehicle.rotation, location.rotation, rotationOffset, bothWays
                );
        }

        public static bool PositionedProperly(
            Vector3 vehicle,
            Vector3 location,
            float offset = 0.0f
        )
        {
            offset = Mathf.Abs(offset);
            float distance =
                Mathf.Abs(Vector3.Distance(vehicle, location));
            return distance <= offset;
        }

        public static bool RotatedProperly(
            Quaternion vehicle,
            Quaternion location,
            float offset = 0.0f,
            bool bothWays = true
        )
        {
            offset = Mathf.Abs(offset);
            float angle =
                Mathf.Abs(Quaternion.Angle(vehicle, location));
            bool ok = angle <= offset;
            if (!ok && bothWays)
            {
                // Check the 180deg version of the location
                // if the vehicle is parked that way.
                var rlocation = location * Quaternion.Euler(0.0f, 180.0f, 0.0f);
                angle = Mathf.Abs(Quaternion.Angle(vehicle, rlocation));
                ok = angle <= offset;
            }

            return ok;
        }

        public virtual void SetGuide(Transform transform)
        {
            if (arrowGuide)
            {
                if (arrowGuide.sourceCount != 0) arrowGuide.RemoveSource(0);
                arrowGuide.AddSource(new ConstraintSource()
                {
                    sourceTransform = transform,
                    weight = 1,
                });
            }
        }

        public virtual void OnPark()
        {

        }
    }
}
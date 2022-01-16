/*
 * Date Created: Tuesday, November 23, 2021 7:33 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using Ph.CoDe_A.Lakbay.Core;
using System.Linq;
using UnityEngine;

namespace Ph.CoDe_A.Lakbay.QuestionRunner
{
    public class QRSpawn : Core.Spawn
    {
        public override bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location)
        {
            if (base.OnSpawnCheck(spawner, locations, location))
            {
                var occupied =
                    locations.Count((l) => l.GetComponentInChildren<QRSpawner>());
                if (occupied >= locations.Length) return false;
                return true;
            }

            return false;
        }
    }
}
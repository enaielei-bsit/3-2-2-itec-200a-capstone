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
    public class QRSpawner : Core.Spawner
    {
        public Repeater repeater;
        public int freeLocationPerRow = 1;

        public override bool CanSpawn(
            Transform[] locations, Transform location,
            Spawn[] spawns, Spawn spawn)
        {
            if (base.CanSpawn(locations, location, spawns, spawn))
            {
                return locations.Count((l) => l.GetComponentInChildren<QRSpawn>())
                    < locations.Length - freeLocationPerRow;
            }

            return false;
        }

        public override void OnSpawnInstantiate(Spawn spawn)
        {
            base.OnSpawnInstantiate(spawn);
        }
    }
}
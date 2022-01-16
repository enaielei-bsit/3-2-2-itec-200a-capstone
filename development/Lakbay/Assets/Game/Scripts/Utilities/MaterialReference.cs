/*
 * Date Created: Friday, October 15, 2021 6:32 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;

using UnityEngine;

namespace Utilities
{
    [Serializable]
    public class MaterialReference
    {
        public int index;
        public Renderer renderer;

        public Material material
        {
            get => materials[index];
            set
            {
                var materials = this.materials;
                materials[index] = value;
                this.materials = materials;
            }
        }
        public Material[] materials
        {
            get => renderer.materials;
            set => renderer.materials = value;
        }

        public Material this[int index]
        {
            get => materials[index];
            set => materials[index] = value;
        }
    }
}
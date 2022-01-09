/*
 * Date Created: Saturday, January 8, 2022 5:02 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {

    [CreateAssetMenu(
        fileName="Path",
        menuName="Game/Core/Path"
    )]
    public class Path : Asset {
        public enum Type {Readable, Watchable}

        public virtual string parsedName => 
            name.Split('_').Last();
        public Type type = Type.Readable;
        public Path parent;
        public List<Path> paths = new List<Path>();
        public List<TextAsset> files =
            new List<TextAsset>();

        public static void ResolveParent(Path path) {
            foreach(var spath in path.paths) {
                spath.parent = path;
                spath.ResolveParent();
            }
        }

        
        [ContextMenu("Resolve Parent")]
        public void ResolveParent() => ResolveParent(this);
    }
}
/*
 * Date Created: Friday, January 7, 2022 1:56 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections.Generic;

using UnityEngine;
using Utilities;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class AboutUI : MonoBehaviour
    {
        public Content acknowledgementContent;
        public LocalizeTextAssetEvent acknowledgementEvent;
        public Content gameContent;
        public LocalizeTextAssetEvent gameEvent;

        public virtual void Build(Content content, TextAsset asset,
            bool value, Func<string, string> onContentLoad = null)
        {
            if (value)
            {
                if (!asset) return;
                var ct = asset.text;
                if (onContentLoad != null) ct = onContentLoad.Invoke(ct);
                content?.Build(ct.DeserializeAsYaml<List<Entry>>());
            }
        }

        public virtual void BuildAcknowledgement(bool value) =>
            Build(acknowledgementContent,
                acknowledgementEvent.AssetReference.LoadAsset(), value);

        public virtual void BuildGame(bool value) =>
            Build(gameContent,
                gameEvent.AssetReference.LoadAsset(), value, IncludeVersion);

        public virtual string IncludeVersion(string text)
        {
            string version = "v" + Application.version;
            try
            {
                return string.Format(text, version);
            }
            catch
            {
                return text;
            }
        }

        public virtual void BuildGame(TextAsset asset)
        {
            Build(gameContent, asset, true, IncludeVersion);
        }
    }
}
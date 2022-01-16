/*
 * Date Created: Saturday, January 8, 2022 11:19 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    using Utilities;

    [Serializable]
    public class Watchable
    {
        public string url = "";
        public string thumbnail = "";
        public string time = "";
        public string label = "";
        public string author = "";
        public string description = "";
    }

    public class WatchableViewer : Controller
    {
        public TextMeshProUGUI url;
        public Image thumbnail;
        public TextMeshProUGUI time;
        public TextMeshProUGUI label;
        public TextMeshProUGUI author;
        public TextMeshProUGUI description;
        public ImageViewer imageViewer;

        [Space]
        public Watchable watchable = new Watchable();

        public virtual void Build(Watchable watchable)
        {
            this.watchable = watchable;
            url?.SetText(watchable.url);
            var sprite = Session.database.Get<Sprite>(watchable.thumbnail);
            if (sprite && thumbnail) thumbnail.sprite = sprite;
            time?.SetText(watchable.time);
            label?.SetText(watchable.label);
            author?.SetText(watchable.author);
            description?.SetText(watchable.description);

            if (imageViewer)
            {
                var btn = thumbnail.gameObject.EnsureComponent<Button>();
                btn.onClick.AddListener(() => imageViewer.Show(sprite));
            }
        }

        public virtual void Build() => Build(watchable);

        public virtual void PlayOnline()
        {
            if (!string.IsNullOrEmpty(watchable.url))
            {
                Application.OpenURL(watchable.url);
            }
        }
    }
}
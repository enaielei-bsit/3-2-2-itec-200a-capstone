/*
 * Date Created: Sunday, December 19, 2021 5:19 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Blowbagets
{
    using Core;
    using TMPro;
    using Utilities;

    public class BlowbagetsUI : Controller
    {
        public TextMeshProUGUI title;
        public Image image;
        public ImageViewer viewer;
        public Content content;

        public BlowbagetsInfo info;

        public virtual void Build(string title, Sprite image,
            IEnumerable<Entry> content, string description = "", string source = "")
        {
            gameObject.SetActive(true);
            this.title?.SetText(title);
            this.content?.Build(content);
            if (this.image)
            {
                this.image.sprite = image;
                if (viewer)
                {
                    var button = this.image.gameObject.EnsureComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(
                        () => viewer.Show(image, description, source));
                }
            }
        }

        public virtual void Build(string title, Sprite image,
            IEnumerable<string> content, string description = "", string source = "")
        {
            Build(title, image, content.Select((c) => (Entry)c),
                description, source);
        }

        public virtual void Build(string title, Sprite image,
            string description = "", string source = "",
            params string[] content)
        {
            Build(title, image, content.AsEnumerable(), description, source);
        }

        public virtual void Build(BlowbagetsInfo info)
        {
            this.info = info;
            // Load image from the database.
            var image = Session.database.Get<Sprite>(info.image);
            Build(info.title, image, info.content, info.description, info.source);
        }

        public virtual void Build() => Build(info);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}
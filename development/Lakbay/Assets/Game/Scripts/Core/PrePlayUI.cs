/*
 * Date Created: Tuesday, December 7, 2021 7:05 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class PrePlayUI : Controller
    {
        public RectTransform nonProLabel;
        public RectTransform proLabel;

        public override void Awake()
        {
            base.Awake();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide() => gameObject.SetActive(false);

        public override void Update()
        {
            base.Update();
            if (Session.mode == GameMode.NonPro)
            {
                nonProLabel?.gameObject.SetActive(true);
                proLabel?.gameObject.SetActive(false);
            }
            else if (Session.mode == GameMode.Pro)
            {
                nonProLabel?.gameObject.SetActive(false);
                proLabel?.gameObject.SetActive(true);
            }
        }
    }
}
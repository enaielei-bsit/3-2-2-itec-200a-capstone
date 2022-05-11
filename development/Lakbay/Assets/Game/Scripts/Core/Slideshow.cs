/*
 * Date Created: Wednesday, May 11, 2022 9:24 PM
 * Author: 
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class Slideshow : Controller {
        protected int _index = 0;

        public List<GameObject> slides = new List<GameObject>();
        public UnityEvent onFinish = new UnityEvent();

        public int index {
            get => slides.Count > 0 ? _index : -1;
            set {
                _index = Mathf.Clamp(value, 0, slides.Count - 1);
                foreach(int i in Enumerable.Range(0, slides.Count)) {
                    slides[i].SetActive(i == index);
                }
                if(value >= slides.Count) onFinish?.Invoke();
            }
        }

        public override void Start() {
            base.Start();
            index = 0;
        }
        
        public virtual void Show() => Show(0);

        public virtual void Show(int index) {
            gameObject.SetActive(true);
            this.index = index;
        }

        public virtual void Hide() => gameObject.SetActive(false);

        public virtual void Next() => index++;

        public virtual void Previous() => index--;
    }
}
/*
 * Date Created: Sunday, October 10, 2021 5:26 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;
using static Utilities.Helper;

namespace Utilities {
    public static class Extension {
        // float
        public static bool Within(this float value, float min, float max) {
            return value >= min && value <= max;
        }

        public static bool Between(this float value, float min, float max) {
            return value > min && value < max;
        }

        // int
        public static bool Within(this int value, int min, int max) {
            return ((float) value).Within(min, max);
        }

        public static bool Between(this int value, int min, int max) {
            return ((float) value).Between(min, max);
        }


        // string
        public static string TrimEnd(this string str, string substring) {
            if(str.EndsWith(substring)) return str.Substring(
                0, str.LastIndexOf(substring)
            );
            return str;
        }

        public static string CopyToClipboard(this string str) {
            GUIUtility.systemCopyBuffer = str;
            return str;
        }

        // T
        public static bool Either<T>(
            this T obj, out T value, params T[] values) {
            value = default;
            foreach(var value_ in values) {
                if(obj.Equals(value_)) {
                    value = value_;
                    return true;
                }
            }

            return false;
        }

        public static bool Either<T>(
            this T obj, params T[] values) {
            return obj.Either(out var value, values);
        }

        // IEnumerable
        public static T PickRandomly<T>(this IEnumerable<T> enumerable) {
            var array = enumerable.ToArray();
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) {
            var enumerable_ = enumerable.ToList();
            var newEnumerable = new List<T>();

            while(enumerable_.Count > 0) {
                var popped = enumerable_.PopRandomly();
                newEnumerable.Add(popped);
            }

            return newEnumerable;
        }

        public static string[] ToCodes(this IEnumerable<Locale> locales) {
            return locales.Select((l) => l.Identifier.Code).ToArray();
        }

        public static bool All(this IEnumerable<bool> enumerable) {
            return enumerable.All((i) => i);
        }

        public static bool Any(this IEnumerable<bool> enumerable) {
            return enumerable.Any((i) => i);
        }

        public static string Join<T>(
            this IEnumerable<T> enumerable, string separator
        ) {
            return string.Join(separator, enumerable);
        }

        // IList
        public static T Pop<T>(this IList<T> list, int index=0) {
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static T PopRandomly<T>(this IList<T> list) {
            var index = Enumerable.Range(0, list.Count).PickRandomly();
            return list.Pop(index);
        }

        // GameObject
        public static bool IsEmpty(this GameObject gameObject) {
            return gameObject.transform.childCount == 0;
        }

        public static GameObject[] Children(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return new GameObject[] {};
            return (from i in Enumerable.Range(
                0, gameObject.transform.childCount)
                select gameObject.transform.GetChild(i).gameObject).ToArray();
        }

        public static GameObject FirstChild(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return null;
            return gameObject.transform.GetChild(0).gameObject;
        }

        public static GameObject LastChild(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return null;
            return gameObject.transform.GetChild(
                gameObject.transform.childCount - 1).gameObject;
        }

        public static void DestroyChildren(
            this GameObject gameObject, float time=0.0f) {
            var children = gameObject.Children();
            foreach(var child in children) {
                GameObject.Destroy(child, time);
            }
        }

        public static void DestroyChildrenImmediately(
            this GameObject gameObject, bool allowDestroyingAssets=false) {
            var children = gameObject.Children();
            foreach(var child in children) {
                GameObject.DestroyImmediate(child, allowDestroyingAssets);
            }
        }

        public static Component EnsureComponent(
            this GameObject gameObject, Type type) {
            var component = gameObject.GetComponent(type);
            if(!component) component = gameObject.AddComponent(type);
            return component;
        }

        public static T EnsureComponent<T>(this GameObject gameObject)
            where T : Component {
            return (T) gameObject.EnsureComponent(typeof(T));
        }

        // MonoBehaviour
        public static Coroutine Run(
            this MonoBehaviour mono,
            ConditionalRunCondition condition=default,
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            ConditionalRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                condition, onStart, onProgress, onFinish, fixedUpdate
            ));
        }

        public static Coroutine Run(
            this MonoBehaviour mono,
            float duration,
            TimedRunOnStart onStart=default,
            TimedRunOnProgress onProgress=default,
            TimedRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                duration, onStart, onProgress, onFinish, fixedUpdate
            ));
        }

        public static Coroutine Run(
            this MonoBehaviour mono,
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                onStart,
                onProgress,
                fixedUpdate
            ));
        }
    }
}
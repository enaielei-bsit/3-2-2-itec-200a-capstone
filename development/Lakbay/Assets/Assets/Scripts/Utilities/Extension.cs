/*
 * Date Created: Thursday, October 7, 2021 3:20 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Utilities {
    public static class Extension {
        // T
        public static bool Either<T>(this T obj, params T[] values) {
            foreach(var value in values) {
                if(obj.Equals(value)) return true;
            }

            return false;
        }

        // float
        public static bool Within(this float value, float min, float max) {
            return value >= min && value <= max;
        }

        public static bool Within(this int value, int min, int max) {
            return ((float) value).Within(min, max);
        }

        public static bool Between(this float value, float min, float max) {
            return value > min && value < max;
        }

        public static bool Between(this int value, int min, int max) {
            return ((float) value).Between(min, max);
        }

        // string
        public static string CopyToClipboard(this string str) {
            GUIUtility.systemCopyBuffer = str;
            return str;
        }

        public static string TrimEnd(this string str, string substring) {
            if(str.EndsWith(substring)) return str.Substring(
                0, str.LastIndexOf(substring));
            return str;
        }

        // IEnumerable
        public static bool All(this IEnumerable<bool> enumerable) {
            return enumerable.All((i) => i);
        }

        public static bool Any(this IEnumerable<bool> enumerable) {
            return enumerable.Any((i) => i);
        }

        public static bool HasIndex<T>(this IEnumerable<T> enumerable, int index) {
            return Enumerable.Range(0, enumerable.Count()).Contains(index); 
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) {
            var list = enumerable.ToList();
            var newList = new List<T>();

            while(list.Count > 0) {
                var popped = list.PopRandomly();
                newList.Add(popped);
            }

            return newList;
        }

        public static string[] ToCodes(this IEnumerable<Locale> locales) {
            return locales.Select((l) => l.Identifier.Code).ToArray();
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string separator) {
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

        public static T PickRandomly<T>(this IEnumerable<T> enumerable) {
            var array = enumerable.ToArray();
            if(array.Length == 0) return default;
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        // GameObject
        public static bool EnsureComponent(
            this GameObject gameObject, System.Type type, out Component component) {
            component = gameObject.GetComponent(type);
            if(!component) {
                component = gameObject.AddComponent(type);
                return true;
            }

            return false;
        }

        public static bool EnsureComponent(
            this GameObject gameObject, System.Type type) {
            return gameObject.EnsureComponent(type, out var comp);
        }

        public static bool EnsureComponent<T>(
            this GameObject gameObject, out T component
        ) where T : Component {
            bool ensured = gameObject.EnsureComponent(typeof(T), out var comp);
            component = (T) comp;
            return ensured;
        }

        public static bool EnsureComponent<T>(
            this GameObject gameObject
        ) where T : Component {
            return gameObject.EnsureComponent<T>(out var comp);
        }
    }
}
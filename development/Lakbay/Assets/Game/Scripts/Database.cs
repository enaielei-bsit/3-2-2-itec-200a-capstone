/*
 * Date Created: Wednesday, November 24, 2021 6:33 AM
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
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay {
    using QuestionRunner;

    public static class Database {
        public static readonly List<Question> questions
            = new List<Question>();
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    //TODO figure out whether to use scriptable objects, or to have some DialogueData asset holding serializable fields
    //TODO decide whether to have a base class for common functionality (ie variable tables)
    [CreateAssetMenu(menuName = "Dialogue/Actor")]
    public class Actor : ScriptableObject
    {
        public string Name;
    }
}

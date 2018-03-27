﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public abstract class DialogueEvent : ScriptableObject
    {

        public abstract void Execute(DialogueManager manager);
    }
}
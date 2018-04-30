﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        const float DELETE_BUTTON_WIDTH = 18f;
        const float ROW_MARGIN = 2f;
        const float COL_MARGIN = 2f;
        const float ROW_HEIGHT = 17f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = ROW_HEIGHT;
            if (property.isExpanded)
            {
                SerializedProperty keys = property.FindPropertyRelative("keys");
                SerializedProperty values = property.FindPropertyRelative("values");
                int size = Mathf.Min(keys.arraySize, values.arraySize);
                for (int i = 0; i < size; ++i)
                {
                    SerializedProperty key = keys.GetArrayElementAtIndex(i);
                    SerializedProperty value = values.GetArrayElementAtIndex(i);
                    height += Mathf.Max(ROW_HEIGHT, EditorGUI.GetPropertyHeight(key), EditorGUI.GetPropertyHeight(value));
                }
                height += 2f * ROW_HEIGHT + 3f * ROW_MARGIN;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect foldoutRect = position;
            foldoutRect.height = ROW_HEIGHT;

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                float fieldWidth = (position.width - (DELETE_BUTTON_WIDTH + 2f * COL_MARGIN)) * 0.5f;
                float currentY = position.y + ROW_HEIGHT;
                SerializedProperty keys = property.FindPropertyRelative("keys");
                SerializedProperty values = property.FindPropertyRelative("values");
                int size = Mathf.Min(keys.arraySize, values.arraySize);

                bool deleteRow = false;
                int deletedIndex = -1;

                Rect keyLabelRect = new Rect(position.x, currentY, fieldWidth, ROW_HEIGHT);
                Rect valueLabelRect = new Rect(position.x + fieldWidth + COL_MARGIN, currentY, fieldWidth, ROW_HEIGHT);

                GUI.Label(keyLabelRect, "Key");
                GUI.Label(valueLabelRect, "Value");

                currentY += ROW_HEIGHT;

                for (int i = 0; i < size; ++i)
                {
                    SerializedProperty key = keys.GetArrayElementAtIndex(i);
                    SerializedProperty value = values.GetArrayElementAtIndex(i);
                    float rowHeight = Mathf.Max(ROW_HEIGHT, EditorGUI.GetPropertyHeight(key), EditorGUI.GetPropertyHeight(value));

                    Rect keyRect = new Rect(position.x, currentY, fieldWidth, rowHeight);
                    Rect fieldRect = new Rect(position.x + fieldWidth + COL_MARGIN, currentY, fieldWidth, rowHeight);
                    Rect buttonRect = new Rect(position.x + 2f * (fieldWidth + COL_MARGIN), currentY, DELETE_BUTTON_WIDTH, ROW_HEIGHT);

                    currentY += rowHeight + ROW_MARGIN;

                    EditorGUI.PropertyField(keyRect, key, GUIContent.none);
                    EditorGUI.PropertyField(fieldRect, value, GUIContent.none);

                    if (GUI.Button(buttonRect,"X"))
                    {
                        deleteRow = true;
                        deletedIndex = i;
                    }
                }

                //TODO spread these out a bit
                Rect AddButtonRect = new Rect(position.x, currentY, position.width * 0.5f, ROW_HEIGHT);
                Rect ClearButtonRect = new Rect(position.x + position.width * 0.5f, currentY, position.width * 0.5f, ROW_HEIGHT);

                if (GUI.Button(AddButtonRect, "Add Pair"))
                {
                    AddItem(property);
                }
                if (GUI.Button(ClearButtonRect, "Clear Dictionary"))
                {
                    ClearDictionary(property);
                }
                if (deleteRow)
                {
                    DeletePair(property, deletedIndex);
                }
            }
        }

        protected void AddItem(SerializedProperty property)
        {
            SerializedProperty keys = property.FindPropertyRelative("keys");
            SerializedProperty values = property.FindPropertyRelative("values");
            int size = Mathf.Min(keys.arraySize, values.arraySize);
            keys.InsertArrayElementAtIndex(size);
            values.InsertArrayElementAtIndex(size);
        }

        protected void DeletePair(SerializedProperty property, int index)
        {
            property.FindPropertyRelative("keys").DeleteArrayElementAtIndex(index);
            property.FindPropertyRelative("values").DeleteArrayElementAtIndex(index);
        }

        protected void ClearDictionary(SerializedProperty property)
        {
            property.FindPropertyRelative("keys").ClearArray();
            property.FindPropertyRelative("values").ClearArray();
        }
    }
}
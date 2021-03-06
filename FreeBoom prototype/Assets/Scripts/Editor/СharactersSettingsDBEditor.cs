﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharactersSettingsDatabase))]
public class СharactersSettingsDBEditor : Editor
{
    private CharactersSettingsDatabase database;

    private void Awake()
    {
        database = (CharactersSettingsDatabase)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            database.AddElement();
        }
        if (GUILayout.Button("<"))
        {
            database.GetPrev();
        }
        if (GUILayout.Button(">"))
        {
            database.GetNext();
        }
        if (GUILayout.Button("Remove"))
        {
            database.RemoveCurrentElement();
        }
        if (GUILayout.Button("RemoveAll"))
        {
            database.ClearDatabase();
        }

        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}

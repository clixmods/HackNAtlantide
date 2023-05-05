using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

//[CustomEditor(typeof(AnimationClip))]
public class AnimationEventEditor : Editor
{
    private Editor _editor;

    private void Awake()
    {
//        SerializedObject so = new SerializedObject(target);
        CreateCachedEditor(target, typeof(AnimationClip), ref _editor);
            
    }

    public override void OnInspectorGUI()
     {
         base.OnInspectorGUI();
         
    //     // Appel la méthode de l'éditeur par défaut pour afficher les champs par défaut
    //   
    //    SerializedObject so = new SerializedObject(target);
    //    CreateCachedEditor(target, null, ref _editor);
    //    _editor.OnInspectorGUI();
    //     // Crée un bouton personnalisé dans l'éditeur
    //     if (GUILayout.Button("Custom Button"))
    //     {
    //         Debug.Log("Button clicked!");
    //     }
     }

    // protected override void OnHeaderGUI()
    // {
    //     base.OnHeaderGUI();
    //        // Crée un bouton personnalisé dans l'éditeur
    //     if (GUILayout.Button("Custom Button"))
    //     {
    //         Debug.Log("Button clicked!");
    //     }
    // }
    
}
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneratorPCG))]

public class GeneratorPCG_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GeneratorPCG generatorPcg = (GeneratorPCG)target;
        
        EditorGUILayout.Space(20);
        
        if (GUILayout.Button("Generate"))
        {
            Debug.Log("Generate baby !!!");
            generatorPcg.generationCoroutine = true;
            generatorPcg.StartGenerate();
        }

        if (GUILayout.Button("Clear all"))
        {
            generatorPcg.StopAllCoroutines();
            generatorPcg.ClearMap();
        }
    }
}

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Jobs;

public class TTCombineMesh : EditorWindow
{
    [SerializeField] public List<GameObject> meshes;

    [MenuItem("TT/Combine Meshes")]
    public static void ShowWindow()
    {
        GetWindow<TTCombineMesh>("Meshes");
    }

    void OnGUI()
    {
        GUILayout.Label("Meshes", EditorStyles.boldLabel);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("meshes");
        EditorGUILayout.PropertyField(stringsProperty, true);

        if (GUILayout.Button("Combine"))
        {
            MeshFilter[] filters = new MeshFilter[stringsProperty.arraySize];
            Mesh finalMesh = new Mesh();
            CombineInstance[] combiners = new CombineInstance[filters.Length];
            GameObject go;

            for (int i = 0; i < stringsProperty.arraySize; i++)
            {
                go = GameObject.Find(stringsProperty.GetArrayElementAtIndex(i).objectReferenceValue.name);
                filters[i] = go.GetComponent<MeshFilter>();

                if (filters[i] == null)
                {
                    Debug.LogError($"Listedeki {i} sýrasýndaki objede MeshFilter yok!");
                    return;
                }
            }

            for (int i = 0; i < filters.Length; i++)
            {
                combiners[i].subMeshIndex = 0;
                combiners[i].mesh = filters[i].sharedMesh;
                combiners[i].transform = filters[i].transform.localToWorldMatrix;
            }

            finalMesh.CombineMeshes(combiners);

            GameObject newObj = new GameObject("Item");
            newObj.AddComponent<MeshFilter>();
            newObj.AddComponent<MeshRenderer>();
            newObj.GetComponent<MeshFilter>().sharedMesh = finalMesh;
            go = GameObject.Find(stringsProperty.GetArrayElementAtIndex(0).objectReferenceValue.name);
            newObj.GetComponent<MeshRenderer>().material = go.GetComponent<Renderer>().material;
        }

        so.ApplyModifiedProperties();
    }
}

#endif

using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(ObjectController))]
public class ObjectControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectController script = (ObjectController)target;

        script.typeOfObject = (TypeOfObject)EditorGUILayout.EnumPopup("Tipo de Objeto", script.typeOfObject);

        script.objectPassTheCam = EditorGUILayout.Toggle("Object Pass Cam", script.objectPassTheCam);
        
        switch (script.typeOfObject)
        {
            case TypeOfObject.Surface:
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical("box");

                script.pontuacaoAtual = EditorGUILayout.IntField("Pontuacao Atual", script.pontuacaoAtual);
                script.pontuacaoVisivel = EditorGUILayout.IntField("Pontuacao Visivel", script.pontuacaoVisivel);
                script.textPontuation = (TextMeshProUGUI)EditorGUILayout.ObjectField("Pontuacao Texto", script.textPontuation, typeof(TextMeshProUGUI), true);
                script.pontosPorPessoa = EditorGUILayout.IntField("Pontos Por Pessoa", script.pontosPorPessoa);
                
                script.delayEntrePessoas = EditorGUILayout.FloatField("Delay Entre Pessoas", script.delayEntrePessoas);

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");

                script.estacheio = EditorGUILayout.Toggle("Esta Cheio", script.estacheio);
                script.sprite = (Sprite)EditorGUILayout.ObjectField("sprite", script.sprite, typeof(Sprite), true);
                script.parentPainelForImagePeople = (GameObject)EditorGUILayout.ObjectField("parent Painel For Image People", script.parentPainelForImagePeople, typeof(GameObject), true);

                EditorGUILayout.EndVertical();

                break;

            case TypeOfObject.People:
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical("box");

                script.cometoLeft = EditorGUILayout.Toggle("come To Left", script.cometoLeft);
                script.deslocamento = EditorGUILayout.Vector3Field("Speed", script.deslocamento);

                EditorGUILayout.EndVertical();
                break;

            case TypeOfObject.Shot:
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical("box");

                script.cometoLeft = EditorGUILayout.Toggle("come To Left", script.cometoLeft);
                script.deslocamento = EditorGUILayout.Vector3Field("Speed", script.deslocamento);

                EditorGUILayout.EndVertical();
                break;

            case TypeOfObject.Shark:
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical("box");
                script.cometoLeft = EditorGUILayout.Toggle("come To Left", script.cometoLeft);
                script.isTheFirstSpawn = EditorGUILayout.Toggle("is The First Spawn", script.isTheFirstSpawn);
                script.rotY = EditorGUILayout.FloatField("Rotation Y", script.rotY);
                script.deslocamento = EditorGUILayout.Vector3Field("Speed", script.deslocamento);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");

                script.timeCountDown = EditorGUILayout.FloatField("Time Count Down", script.timeCountDown);
                script.shotSharkPrefab = (GameObject)EditorGUILayout.ObjectField("Shot Shark Prefab", script.shotSharkPrefab, typeof(GameObject), true);
                EditorGUILayout.EndVertical();  
                
                EditorGUILayout.BeginVertical("box");
                script.indexInGameManager = EditorGUILayout.IntField("index In Game Manager", script.indexInGameManager);
                script.manager = (GameManager)EditorGUILayout.ObjectField("Manager", script.manager, typeof(GameManager), true);             

                EditorGUILayout.EndVertical();  

                break;

            case TypeOfObject.Fish:
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical("box");

                script.rotY = EditorGUILayout.FloatField("Rotation Y", script.rotY);
                script.cometoLeft = EditorGUILayout.Toggle("come To Left", script.cometoLeft);
                script.deslocamento = EditorGUILayout.Vector3Field("Speed", script.deslocamento);

                EditorGUILayout.EndVertical();
                break;
        }
        /*if (script.typeOfObject == TypeOfObject.Surface)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            //script.raio = EditorGUILayout.FloatField("Raio", script.raio);
            //script.centro = EditorGUILayout.Vector2Field("Centro", script.centro);
            EditorGUILayout.EndVertical();
        }


        if (script.typeOfObject == TypeOfObject.People)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("helpbox");
            //script.centro2 = (Transform)EditorGUILayout.ObjectField("Centro", script.centro2, typeof(Transform), true);
            EditorGUILayout.EndVertical();  
        }

        if (script.typeOfObject == TypeOfObject.Fish)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            //script.raio = EditorGUILayout.FloatField("Raio", script.raio);
            //script.centro = EditorGUILayout.Vector2Field("Centro", script.centro);
            EditorGUILayout.EndVertical();
        }


        if (script.whatObject == TypeOfObject.Shot)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("helpbox");
            //script.centro2 = (Transform)EditorGUILayout.ObjectField("Centro", script.centro2, typeof(Transform), true);
            EditorGUILayout.EndVertical();  
        }


        if (script.whatObject == TypeOfObject.Shark)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.EndVertical();  
        }*/

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script); // Marca o objeto como "alterado"
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionPlayer))]
public class CollisionPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollisionPlayer script = (CollisionPlayer)target;

        script.whatObject = (TypeOfObject)EditorGUILayout.EnumPopup("Tipo de Objeto", script.whatObject);

        //script.velocidade_angular = EditorGUILayout.FloatField("Velocidade Angular", script.velocidade_angular);
        //script.teta_inicial = EditorGUILayout.FloatField("Teta Inicial", script.teta_inicial);
        //script.delay = EditorGUILayout.FloatField("Delay", script.delay);

        if (script.whatObject == TypeOfObject.Surface)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            script.startGame = EditorGUILayout.Toggle("Start Game", script.startGame);
            EditorGUILayout.EndVertical();
        }


        if (script.whatObject == TypeOfObject.People)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("helpbox");
            
            //script.posObjX = EditorGUILayout.IntPopup("Pos Obj X", script.posObjX);
            EditorGUILayout.EndVertical();  
        }

        if (script.whatObject == TypeOfObject.Fish)
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

            EditorGUILayout.EndVertical();  
        }


        if (script.whatObject == TypeOfObject.Shark)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.EndVertical();  
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script); // Marca o objeto como "alterado"
        }
    }
}

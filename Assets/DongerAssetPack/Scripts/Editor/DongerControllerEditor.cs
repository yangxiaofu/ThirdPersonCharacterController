using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DongerAssetPack.MovementEngine{
	[CustomEditor(typeof(DongerController))]
	public class DongerControllerEditor : Editor {

		DongerController _controller;

		SerializedProperty m_Crouch;
		SerializedProperty m_Jump;

		void OnEnable(){
			_controller = (DongerController)target;

			m_Crouch = serializedObject.FindProperty("m_Crouch");
			m_Jump = serializedObject.FindProperty("m_Jump");
		}

		public override void OnInspectorGUI()
		{
			float column1Width = 200f;
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(new GUIContent("Crouch"), GUILayout.Width(column1Width));
			GUILayout.Label(m_Crouch.boolValue.ToString());

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(new GUIContent("Jump"), GUILayout.Width(column1Width));
			GUILayout.Label(m_Jump.boolValue.ToString());

			EditorGUILayout.EndHorizontal();

			//DrawDefaultInspector();
		}
	}

}

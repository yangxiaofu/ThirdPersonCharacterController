using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DongerAssetPack.MovementEngine{
	[CustomEditor(typeof(DongerController))]
	public class DongerControllerEditor : Editor {

		DongerController _controller;
		SerializedProperty m_Crouch;

		void OnEnable()
		{
			_controller = (DongerController)target;
			m_Crouch = serializedObject.FindProperty("m_Crouch");
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox(_controller.HelpBox(), MessageType.Info);
			DrawDefaultInspector();
		}
	}

}

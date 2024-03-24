using UnityEditor;
using UnityEngine;

namespace Editor {

	[CustomEditor(typeof(Game))]
	public class WorldEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			Game mapGen = (Game)target;
			if (GUILayout.Button("Generate Level"))
			{
				mapGen.CreateLevel();
			}
		}
	}

}
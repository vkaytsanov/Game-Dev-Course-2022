using UnityEngine;
using UnityEditor;
using Utils.Attributes;

namespace Editor
{
	[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
	public class NamedArrayDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			try
			{
				int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
				EditorGUI.PropertyField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]), true);

			}
			catch
			{
				EditorGUI.PropertyField(rect, property, label);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	}

}
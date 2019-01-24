//c# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(EffectFragmentWrapper))]
public class EffectFragmentWrapperEditor : PropertyDrawer 
{
	const float ELEMENT_HEIGHT = 18.0f;
	const float INDENT = 3.0f;

	//From http://sketchyventures.com/2015/08/07/unity-tip-getting-the-actual-object-from-a-custom-property-drawer/
	T PropToObj<T>(SerializedProperty property) where T : class
    {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        if (obj == null) { return null; }
 
        T actualObject = null;
        if (obj.GetType().IsArray)
        {
            var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
			if(index < ((T[])obj).Length)
            	actualObject = ((T[])obj)[index];
			else
				actualObject = null;
        }
        else
        {
            actualObject = obj as T;
        }
        return actualObject;
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);
		EditorGUI.BeginChangeCheck();
		EditorGUI.ObjectField(new Rect(position.x, position.y, position.width, ELEMENT_HEIGHT - 2), property.FindPropertyRelative("fragment"));		
		EffectFragmentWrapper wrapper = PropToObj<EffectFragmentWrapper>(property);

		if(wrapper != null)
		{		
			EffectFragment fragment = wrapper.fragment;
			if(fragment != null)
			{
				EffectFragment.Type[] argumentTypes = fragment.GetArgumentTypes();
				int count = 0;
				/* 
				while(property.arraySize < argumentTypes.Length && count < 1000)
				{
					property.InsertArrayElementAtIndex(0);
					property.serializedObject.ApplyModifiedProperties();
					++count;
				}
				*/
				string[] argumentNames = fragment.GetArgumentNames();

				if(wrapper.arguments == null || wrapper.arguments.Length != argumentTypes.Length)
				{
					wrapper.arguments = new object[argumentTypes.Length];
				}

				// Don't make child fields be indented
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = indent + 1;

				// Draw fields - passs GUIContent.none to each so they are drawn without labels
				for(int i = 0; i < argumentTypes.Length; ++i)
				{
					GUIContent innerLabel = new GUIContent();
					innerLabel.text = argumentNames[i];
					if(argumentTypes[i] == EffectFragment.Type.Integer)
					{
						if(wrapper.arguments[i] == null)
							wrapper.arguments[i] = new int();
						wrapper.arguments[i] = EditorGUI.IntField(new Rect(position.x + INDENT, position.y + ELEMENT_HEIGHT * i + ELEMENT_HEIGHT, position.width - INDENT, ELEMENT_HEIGHT - 2), innerLabel, (int)wrapper.arguments[i]);
					}
					else if(argumentTypes[i] == EffectFragment.Type.Float)
					{
						if(wrapper.arguments[i] == null)
							wrapper.arguments[i] = new float();
						wrapper.arguments[i] = EditorGUI.FloatField(new Rect(position.x + INDENT, position.y + ELEMENT_HEIGHT * i + ELEMENT_HEIGHT, position.width - INDENT, ELEMENT_HEIGHT - 2), innerLabel, (float)wrapper.arguments[i]);
					}
					else if(argumentTypes[i] == EffectFragment.Type.String)
					{
						if(wrapper.arguments[i] == null)
							wrapper.arguments[i] = "";
						wrapper.arguments[i] = EditorGUI.TextField(new Rect(position.x + INDENT, position.y + ELEMENT_HEIGHT * i + ELEMENT_HEIGHT, position.width - INDENT, ELEMENT_HEIGHT - 2), innerLabel, (string)wrapper.arguments[i]);
					}
					else if(argumentTypes[i] == EffectFragment.Type.Boolean)
					{
						if(wrapper.arguments[i] == null)
							wrapper.arguments[i] = new bool();
						wrapper.arguments[i] = EditorGUI.Toggle(new Rect(position.x + INDENT, position.y + ELEMENT_HEIGHT * i + ELEMENT_HEIGHT, position.width - INDENT, ELEMENT_HEIGHT - 2), innerLabel, (bool)wrapper.arguments[i]);
					}
				}

				// Set indent back to what it was
				EditorGUI.indentLevel = indent;
			}
		}
		EditorGUI.EndProperty();
		EditorUtility.SetDirty(property.serializedObject.targetObject);
    }

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
     {
		 EffectFragmentWrapper wrapper = PropToObj<EffectFragmentWrapper>(property);
         if (wrapper != null && wrapper.fragment != null)
             return EditorGUI.GetPropertyHeight(property) + ELEMENT_HEIGHT * (wrapper.fragment).GetArgumentTypes().Length;
         return EditorGUI.GetPropertyHeight(property);
     }
}

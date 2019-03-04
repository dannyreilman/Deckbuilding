//c# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;

[CustomPropertyDrawer(typeof(EffectFragmentWrapper))]
public class EffectFragmentWrapperEditor : PropertyDrawer 
{
	const float ELEMENT_HEIGHT = 18.0f;
	const float INDENT = 3.0f;

	//Modified from https://answers.unity.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html
	public object GetObjectFromProp(SerializedProperty prop)
	{
		var path = prop.propertyPath.Replace(".Array.data[", "[");
		object obj = prop.serializedObject.targetObject;
		var elements = path.Split('.');
		foreach(var element in elements)
		{
			if(element.Contains("["))
			{
				var elementName = element.Substring(0, element.IndexOf("["));
				var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
				obj = GetValue(obj, elementName, index);
			}
			else
			{
				obj = GetValue(obj, element);
			}
		}
		return obj;
	}
	public object GetValue(object source, string name)
	{
		if(source == null)
			return null;
		var type = source.GetType();
		var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if(f == null)
		{
			var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if(p == null)
				return null;
			return p.GetValue(source, null);
		}
		return f.GetValue(source);
	}
	
	public object GetValue(object source, string name, int index)
	{	
		var enumerable = GetValue(source, name) as IEnumerable;
		var enm = enumerable.GetEnumerator();
		bool canMoveNext = true;
		while(index-- >= 0 && canMoveNext)
			canMoveNext = enm.MoveNext();
		if(!canMoveNext)
			return null;
		return enm.Current;
	}

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		EditorGUI.BeginProperty(position,label,property);
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		GUIContent fragmentLabel = new GUIContent();
		fragmentLabel.text = "Fragment";
		SerializedProperty fragmentProp = property.FindPropertyRelative("fragment");
		EditorGUI.PropertyField(position, fragmentProp, fragmentLabel, true);
		
		EffectFragment fragment = (EffectFragment)property.FindPropertyRelative("fragment").objectReferenceValue;
		if(fragment != null)
		{
			EffectFragment.Type[] argumentTypes = fragment.GetArgumentTypes();
			string[] argumentNames = fragment.GetArgumentNames();

			EffectFragmentWrapper wrapper = GetObjectFromProp(property) as EffectFragmentWrapper;
			if(wrapper != null)
			{
				if(wrapper.arguments == null || wrapper.arguments.Length != argumentTypes.Length)
					wrapper.arguments = new System.Object[argumentTypes.Length];
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
			}
		}
		EditorUtility.SetDirty(property.serializedObject.targetObject);
		EditorGUI.EndProperty();
		return;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		EffectFragmentWrapper wrapper = GetObjectFromProp(property) as EffectFragmentWrapper;
		if (wrapper != null && wrapper.fragment != null)
			return EditorGUI.GetPropertyHeight(property) + ELEMENT_HEIGHT * (wrapper.fragment).GetArgumentTypes().Length;
		return EditorGUI.GetPropertyHeight(property);
	}
}

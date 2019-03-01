using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName="TypeColorSelector", fileName="New TypeColor")]
public class TypeColorSelector : ScriptableObject
{
	private static TypeColorSelector instance_internal = null;

	public static TypeColorSelector instance
	{
		get
		{
			if(instance_internal == null)
			{
				instance_internal = Resources.Load<TypeColorSelector>("TypeColorSelectorInstance");
			}

			return instance_internal;
		}
	}


	[System.Serializable]
	public struct ColorMapEntry
	{
		public string type;
		public Color color;
	}

	private Dictionary<string, Color> colorMap = null;
	public ColorMapEntry[] entries;

	void LoadMap()
	{
		colorMap = new Dictionary<string, Color>();

		foreach(ColorMapEntry entry in entries)
		{
			colorMap[entry.type] = entry.color;
		}
	}

	public Color defaultColor;

	public Color DetermineTypeColor(string types)
	{
		if(colorMap == null)
			LoadMap();
	
		if(types != "" && colorMap.ContainsKey(types))
		{
			return colorMap[types];
		}

		foreach(string part in types.Split(' '))
		{
			if(part != "" && colorMap.ContainsKey(part))
			{
				return colorMap[part];
			}
		}

		return defaultColor;
	}
}

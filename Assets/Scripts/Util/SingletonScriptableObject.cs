using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
	private static T instance;

	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load<T>(typeof(T).Name);
			}

			return instance;
		}
	}
}


#if UNITY_EDITOR

public class MakeScriptableObject
{
	
	[MenuItem("Assets/Create/My Scriptable Object")]
	public static void CreateMyAsset()
	{
		var asset = ScriptableObject.CreateInstance<GameSettings>();

		AssetDatabase.CreateAsset(asset, "Assets/Resources/GameSettings.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}

#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageSaveEditor {

	public static void SavePrefab(GameObject go, string name) {
		
		//prefabの保存フォルダパス
		string prefabDirPath = Application.dataPath + "/Prefabs/";
		if (!Directory.Exists(prefabDirPath)) {
			//prefab保存用のフォルダがなければ作成する
			Directory.CreateDirectory(prefabDirPath);
		}

		//prefabの保存ファイルパス
		string prefabPath = prefabDirPath + name + ".prefab";
		string nameSuffix = "";

		for (int i = 1; i < 100; i++) {
			if (!File.Exists(prefabPath)) {
				//prefabファイルがなければ作成する
				File.Create(prefabPath);
				nameSuffix = name + i + ".prefab";
				break;
			}

			prefabPath = prefabDirPath + name + i + ".prefab";
		}


		//prefabの保存
		#if UNITY_EDITOR
		UnityEditor.PrefabUtility.SaveAsPrefabAsset(go, "Assets/Prefabs/" + nameSuffix);
		UnityEditor.AssetDatabase.SaveAssets();
		#endif
	}

}
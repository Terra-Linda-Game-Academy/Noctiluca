using Levels;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Util.ConcreteWeightedItems;

namespace Util.Editor {
	public class PoolViewer<T, Tw, Tp> : VisualElement where Tw : WeightedItem<T>, new() where Tp : Pool<T, Tw> {
		public PoolViewer(SerializedProperty poolProp, Object parent) {
			Tp pool = poolProp.objectReferenceValue as Tp;

			if (!AssetDatabase.Contains(parent)) {
				Add(new Label("Object is being created, cannot show pool"));
				return;
			}

			if (pool == null && parent != null) {
				pool      = ScriptableObject.CreateInstance<Tp>();
				pool.name = "Connections";

				AssetDatabase.AddObjectToAsset(pool, parent);

				poolProp.objectReferenceValue = pool;

				poolProp.serializedObject.ApplyModifiedProperties();

				AssetDatabase.SaveAssets();
				Undo.RegisterCreatedObjectUndo(pool, $"Added pool to {parent.name}");
			}

			SerializedObject   poolObj        = new SerializedObject(pool);
			SerializedProperty serializedList = poolObj.FindProperty("_items");

			ManagedListViewer<WeightedConnection> listViewer =
				new ManagedListViewer<WeightedConnection>(serializedList,
				                                          ManagedListViewer<WeightedConnection>.Options.NoSize,
				                                          poolProp.displayName);
			Add(listViewer);
		}
	}
}
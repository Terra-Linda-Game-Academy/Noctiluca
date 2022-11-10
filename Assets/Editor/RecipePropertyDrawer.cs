using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RecipePool.Recipe))]
public class RecipePropertyDrawer : PropertyDrawer
{

    Rect left, middle, right, bottom;
    SerializedProperty fluid, item, product, description;
    string descriptionString;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Setup
        left = new Rect(position.x,position.y, position.width/3, position.height /2 );
        middle = new Rect(position.x + position.width / 3, position.y, position.width / 3, position.height /2);
        right = new Rect(position.x + (position.width / 3)*2, position.y, position.width / 3, position.height/ 2);
        bottom = new Rect(position.x, position.y + position.height/2, position.width, position.height / 2);

        fluid = property.FindPropertyRelative("fluid");
        item = property.FindPropertyRelative("item");
        product = property.FindPropertyRelative("product");
        description = property.FindPropertyRelative("description");

        EditorGUI.PropertyField(left, fluid, new GUIContent("Fluid"));
        EditorGUI.PropertyField(middle, item, new GUIContent("Item"));
        EditorGUI.PropertyField(right, product, new GUIContent("Product"));

        descriptionString = description.stringValue;
        descriptionString = EditorGUI.TextField(bottom, new GUIContent("Description/Info"), descriptionString);
        description.stringValue = descriptionString;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 2f;
    }
}

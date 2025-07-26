#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Attributes
{
    [CustomPropertyDrawer(typeof(MaxAttribute))]
    public class MaxAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MaxAttribute maxAttribute = (MaxAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = Mathf.Min(EditorGUI.FloatField(position, label, property.floatValue), maxAttribute.Max);
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = Mathf.Min(EditorGUI.IntField(position, label, property.intValue), (int)maxAttribute.Max);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use Max with float or int.");
            }
        }
    }
}
#endif
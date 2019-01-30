using UnityEditor;

[CustomEditor(typeof(AnimatedObject))]
public class charComponentEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AnimatedObject cc = (AnimatedObject)target;
        
        cc.FlipX = EditorGUILayout.Toggle("Flip X", cc.FlipX);
        cc.FlipY = EditorGUILayout.Toggle("Flip Y", cc.FlipY);

        base.OnInspectorGUI();
        EditorUtility.SetDirty(target);
    }
}

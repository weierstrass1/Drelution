using UnityEditor;

[CustomEditor(typeof(MobileObject), true)]
public class MobileObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MobileObject t = (MobileObject)target;

        EditorGUILayout.LabelField("Options");

        t.Solid = EditorGUILayout.Toggle("Solid:", t.Solid);
        t.GuidedByTerrain = EditorGUILayout.Toggle("Guided By Terrain:", t.GuidedByTerrain);
        t.AffectedByTerrain = EditorGUILayout.Toggle("Affected By Terrain:", t.AffectedByTerrain);
        t.IgnoreBelowInteraction = EditorGUILayout.Toggle("Ignore Below Interaction:", t.IgnoreBelowInteraction);
        t.XDirection = EditorGUILayout.Toggle("H. Direction:", t.XDirection);
        t.YDirection = EditorGUILayout.Toggle("V. Direction:", t.YDirection);
        t.BlockedAngleDetector = EditorGUILayout.Toggle("Blocked Angle Detector:", t.BlockedAngleDetector);
        t.BlockedFromBelow = EditorGUILayout.Toggle("Blocked From Below:", t.BlockedFromBelow);
        t.BlockedFromLeft = EditorGUILayout.Toggle("Blocked From Left:", t.BlockedFromLeft);
        t.BlockedFromRight = EditorGUILayout.Toggle("Blocked From Right:", t.BlockedFromRight);
        t.BlockedFromAbove = EditorGUILayout.Toggle("Blocked From Above:", t.BlockedFromAbove);

        EditorGUILayout.LabelField("Physics Variables");

        t.Mass = EditorGUILayout.IntField("Mass:", t.Mass);
        t.TerrainAngle = EditorGUILayout.FloatField("Terrain Angle:", t.TerrainAngle);
        t.MinXSpeed = EditorGUILayout.FloatField("Min X Speed:", t.MinXSpeed);
        t.MinYSpeed = EditorGUILayout.FloatField("Min Y Speed:", t.MinYSpeed);
        t.MaxXSpeed = EditorGUILayout.FloatField("Max X Speed:", t.MaxXSpeed);
        t.MaxYSpeed = EditorGUILayout.FloatField("Max Y Speed:", t.MaxYSpeed);
        t.XSpeed = EditorGUILayout.FloatField("X Speed:", t.XSpeed);
        t.YSpeed = EditorGUILayout.FloatField("Y Speed:", t.YSpeed);
        t.XAcceleration = EditorGUILayout.FloatField("X Acceleration:", t.XAcceleration);
        t.YAcceleration = EditorGUILayout.FloatField("Y Acceleration:", t.YAcceleration);
        t.XFriction = EditorGUILayout.FloatField("X Friction:", t.XFriction);
        t.YFriction = EditorGUILayout.FloatField("Y Friction:", t.YFriction);
        t.XGravity = EditorGUILayout.FloatField("X Gravity:", t.XGravity);
        t.YGravity = EditorGUILayout.FloatField("Y Gravity:", t.YGravity);

        EditorGUILayout.LabelField("Detectors");

        base.OnInspectorGUI();

        EditorUtility.SetDirty(target);
    }
}

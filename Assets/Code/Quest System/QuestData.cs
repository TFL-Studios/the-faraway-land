using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public DialogBlock mainDialog;
    public DialogBlock[] charSpecificExtraDialog;
}

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public string characterName;

    [Header("Journal Character Sheet")]
    public Sprite portraitSilhouette;
    public Sprite portraitColored;
}

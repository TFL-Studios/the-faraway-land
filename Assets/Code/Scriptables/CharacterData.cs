using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public string characterName;
    public Sprite characterSprite; // TODO: incluir poses e expressoes

    [Header("Quests")]
    public int[] givenQuests; // TODO: colocar o scriptable da quest, maybe

    [Header("Journal Character Sheet")]
    public Sprite portraitSilhouette;
    public Sprite portraitColored;
}

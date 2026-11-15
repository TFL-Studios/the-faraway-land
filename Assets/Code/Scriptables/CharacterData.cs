using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public string characterName;
    public Sprite characterSprite; // TODO: incluir poses e expressoes

    [Header("Dialog")]
    public DialogBlock genericDialog;
    public Sprite characterDialogSprite;

    [Header("Journal Character Sheet")]
    public Sprite portraitSilhouette;
    public Sprite portraitColored;

    [Header("AI")]
    public UnityEditor.MonoScript stateMachiune;
}

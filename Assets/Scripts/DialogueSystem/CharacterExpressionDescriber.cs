using UnityEngine;

[CreateAssetMenu(fileName = "CharacterExpressionDescriber", menuName = "Scriptable Objects/CharacterExpressionDescriber")]
public class CharacterExpressionDescriber : ScriptableObject
{
    public string charName;
    public string[] charExpressionName;
    public Texture[] charExpressionSprite;
}

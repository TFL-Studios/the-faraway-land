using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Controller : Interactable
{
    [Header("NPC")]
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private CharacterAffinity _currentAffinity = CharacterAffinity.Stranger; // TODO: mover pra GameManager
    [SerializeField] private DialogBlock genericDialog; // TODO: passar pro CharacterData
    
    [Header("GameObject Config")]
    [SerializeField] private SpriteRenderer _npcGraphics;

    public override bool Interact()
    {
        DialogBlock dialog = genericDialog;
        bool foundGiven = false;

        foreach (int questID in this._characterData.givenQuests)
        {
            if (questID != GameManager.Instance.activeQuestIndex) continue;
            
            dialog = GameManager.Instance.quests[questID].questData.mainDialog;
            GameManager.Instance.activeQuestIndex++; // TODO: inclusir a bool
            foundGiven = true;
            break;
        }

        bool hasQuestGeneric = false;
        int charDialogIndex = 0;
        foreach (DialogBlock charDialog in GameManager.Instance.quests[GameManager.Instance.activeQuestIndex].questData.charSpecificExtraDialog)
        {
            List<CharacterData> cars = charDialog.characters.ToList();
            if (cars.Contains(this._characterData))
            {
                hasQuestGeneric = true;
                break;
            }
            charDialogIndex++;
        }

        if (!foundGiven && hasQuestGeneric)
        {
            dialog = GameManager.Instance.quests[GameManager.Instance.activeQuestIndex].questData.charSpecificExtraDialog[charDialogIndex];
        }

        GameManager.Instance.StartDialog(dialog);

        //foreach (Dialog dialogdialog in dialog.dialog)
        //{
        //    Debug.Log($"{dialog.characters[dialogdialog.talking]}: {dialogdialog.speechs}");
        //}

        this.AjustAffinity(Random.Range(-7, 8));
        Debug.Log(this._currentAffinity);

        return true;
    }

    public void AjustAffinity(int increment) // TODO: mover pra GameManager
    {
        int ajustedAffinity = (int)this._currentAffinity + increment;
        this._currentAffinity = (CharacterAffinity)Mathf.Clamp(ajustedAffinity, (int)CharacterAffinity.Bad3, (int)CharacterAffinity.Family);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string name = "NPC";

        if (this._characterData)
        {
            name = this._characterData.characterName.ToUpper().Replace(" ", "_");
            this._npcGraphics.sprite = this._characterData.characterSprite;
        }

        this.transform.parent.name = $"{name}_Environment";
        this.name = $"{name}_Controller";
        this.transform.GetChild(0).name = $"{name}_Graphic";
    }
#endif
}

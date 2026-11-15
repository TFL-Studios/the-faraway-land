using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerGenerator : Editor
{
    private static InputActionAsset _asset;

    private static readonly Dictionary<string, string> _valueLookupTable = new Dictionary<string, string>()
    {
        { "Axis", "float" },
        { "Button", "bool" },
        { "DiscreteButton", "bool" },
        { "Key", "bool" },
        { "Analog", "float" },
        { "Integer", "int" },
        { "Digital", "float" },
        { "Double", "double" },
        { "Vector2", "Vector2" },
        { "Vector3", "Vector3" },
        { "Delta", "Vector2" },
        { "Quaternion", "Quaternion" },
        { "Stick", "Vector2" },
        { "Dpad", "Vector2" },
        { "Touch", "Vector3" },
        { "Pose", "Vector3" },
        { "Bone", "Vector3" },
        { "Eyes", "Vector3" },
    };

    [MenuItem("Tools/Generate Input Handler")]
    public static void GenerateInputHandler()
    {
        // Config Strings
        string assetName = "InputSystem_Actions";
        string destinationFolder = $"{Application.dataPath}/Code/Input System";

        if (!_asset) { _asset = Resources.Load<InputActionAsset>(assetName); }
        Debug.Assert(_asset, "[InputHandlerGenerator] ÍnputActionAsset not Found in Resources!");

        string mapEnumItems = "";
        string mapCaseItems = "";
        
        string mapDeclaration = "";
        string mapInitialization = "";

        string inputDeclaration = "";
        string inputAccessDefinition = "";
        string inputInitialization = "";
        
        for (int i = 0; i < _asset.actionMaps.Count; i++)
        {
            bool lastMapFlag = i == _asset.actionMaps.Count - 1;
            string mapNewLine = (lastMapFlag ? "" : "\n");
            InputActionMap map = _asset.actionMaps[i];

            string mapName = map.name.Replace(" ", "");

            string mapVar = $"_map{mapName}";

            mapEnumItems += $"{mapName},{mapNewLine}{(lastMapFlag ? "" : "\t")}";
            mapCaseItems += $"case EInputMap.{mapName}: if (enable) {mapVar}.Enable(); else {mapVar}.Disable(); break;{mapNewLine}{(lastMapFlag ? "" : "\t\t\t")}";

            mapDeclaration += $"private InputActionMap {mapVar};{mapNewLine}{(lastMapFlag ? "" : "\t")}";
            mapInitialization += $"this.{mapVar} = this._inputActionAsset.FindActionMap(\"{map.name}\");{mapNewLine}{(lastMapFlag ? "" : "\t\t")}";

            inputDeclaration += $"// {map.name} Map\n\t";
            inputAccessDefinition += $"// {map.name} Map\n\t";
            inputInitialization += $"// {map.name} Map\n\t\t\t";

            for (int j = 0; j < map.actions.Count; j++)
            {
                bool lastActionFlag = j == map.actions.Count - 1;
                string actionNewLine = (lastMapFlag && lastActionFlag ? "" : "\n");
                InputAction action = map.actions[j];

                string actionType = action.type == InputActionType.Button ? "bool" : _valueLookupTable[action.expectedControlType];
                string actionName = action.name.Replace(" ", "");

                string inputVar = $"_input{actionName}";

                inputDeclaration += $"private CustomInput<{actionType}> {inputVar};{actionNewLine}{(lastMapFlag && lastActionFlag ? "" : "\t")}";
                inputAccessDefinition += $"public CustomInput<{actionType}> {actionName}Input {{ get {{ return this.{inputVar}; }} }}{actionNewLine}{(lastMapFlag && lastActionFlag ? "" : "\t")}";
                inputInitialization += $"this.{inputVar} = new CustomInput<{actionType}>({mapVar}.FindAction(\"{action.name}\")),{actionNewLine}{(lastMapFlag && lastActionFlag ? "" : "\t\t\t")}";
            }
        }

        string source =
@$"using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputMap
{{
    {mapEnumItems}
}}

public class InputHandler : PersistentSingleton<InputHandler>
{{
    private InputActionAsset _inputActionAsset;
    
    /* Maps */
    {mapDeclaration}

    /* Custom Inputs */
    private ICustomInput[] _allCustomInputs;
    {inputDeclaration}
    
    /* Access */
    {inputAccessDefinition}
    
    protected override void Awake()
    {{
        base.Awake();
        
        if (!this._inputActionAsset)
        {{
            this._inputActionAsset = Resources.Load<InputActionAsset>(""{assetName}"");
        }}

        {mapInitialization}
        
        this._allCustomInputs = new ICustomInput[]
        {{
            {inputInitialization}
        }};
        
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {{
            this._allCustomInputs[i].RegisterCallbacks();
        }}
    }}
    
    private void OnEnable()
    {{
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {{
            this._allCustomInputs[i].Enable();
        }}
    }}

    private void OnDisable()
    {{
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {{
            this._allCustomInputs[i].Disable();
        }}
    }}
    
    public bool SetMapEnabled(EInputMap map, bool enable)
    {{
        switch (map)
        {{
            {mapCaseItems}
            default: return false;
        }}
        return true;
    }}
}}
";
        File.WriteAllText($"{destinationFolder}/InputHandler.cs", source);
        AssetDatabase.Refresh();
        Debug.Log("Successfully generated InputHandler at: " + destinationFolder);
    }
}

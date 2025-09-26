using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour, IGameServices
{
    private const string RebindsKey = "input_rebinds_v1";
    public PlayerInputActions Actions { get; private set; }
    
    private void Awake()
    {
        Actions = new PlayerInputActions();
        LoadRebinds();
        Actions.Enable();
    }
    
    #region Persistence
    public void SaveRebinds()
    {
        var json = Actions.asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, json);
        PlayerPrefs.Save();
    }

    public void LoadRebinds()
    {
        if (PlayerPrefs.HasKey(RebindsKey))
            Actions.asset.LoadBindingOverridesFromJson(PlayerPrefs.GetString(RebindsKey));
    }

    public void ResetRebinds()
    {
        Actions.asset.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(RebindsKey);
        PlayerPrefs.Save();
    }
    #endregion
    
    #region Helpers
    public static int FindBindingIndex(InputAction action, string bindingGroup, string compositePartName = null)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            if (!string.IsNullOrEmpty(bindingGroup) && !b.groups.Contains(bindingGroup)) continue;

            if (string.IsNullOrEmpty(compositePartName))
            {
                if (!b.isPartOfComposite) return i;
            }
            else if (b.isPartOfComposite && b.name == compositePartName)
            {
                return i;
            }
        }
        return -1;
    }

    public string GetDisplayString(string actionPath, string bindingGroup, string compositePartName = null)
    {
        var action = Actions.asset.FindAction(actionPath, true);
        int idx = FindBindingIndex(action, bindingGroup, compositePartName);
        return idx >= 0 ? action.GetBindingDisplayString(idx) : "-";
    }

    // Generic interactive rebind
    
    public void StartRebind(string actionPath, string bindingGroup, string compositePartName,
        Action onComplete, Action onCancel)
    {
        var action = Actions.asset.FindAction(actionPath, true);
        int idx = FindBindingIndex(action, bindingGroup, compositePartName);
        if (idx < 0) { onCancel?.Invoke(); return; }

        action.Disable();
        var op = action.PerformInteractiveRebinding(idx)
            .WithControlsExcluding("Mouse/position")
            .WithControlsExcluding("Mouse/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .WithMatchingEventsBeingSuppressed();

        op.OnComplete(_ =>
        {
            op.Dispose();
            action.Enable();
            SaveRebinds();
            onComplete?.Invoke();
        });

        op.OnCancel(_ =>
        {
            op.Dispose();
            action.Enable();
            onCancel?.Invoke();
        });

        op.Start();
    }
    
    #endregion
}

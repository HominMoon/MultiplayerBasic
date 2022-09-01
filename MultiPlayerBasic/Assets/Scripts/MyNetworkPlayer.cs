using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] TMP_Text displayNameText;
    [SerializeField] Renderer displayColorRenderer;

    #region Server

    [SyncVar(hook = nameof(HandleDisplayTextUPdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    void CmdSetDisplayName(string newDisplayName)
    {
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client

    void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    void HandleDisplayTextUPdated(string oldText, string newText)
    {
        displayNameText.text = newText;
    }

    [ContextMenu("SetMyName")]
    void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc] void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}

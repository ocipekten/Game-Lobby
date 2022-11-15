using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyPanel : MonoBehaviour
{
    public TextMeshProUGUI title;

    private Lobby lobby;

    public void SetLobby(Lobby l)
    {
        this.lobby = l;
        UpdatePanel();
    }

    private void UpdatePanel()
    {
        title.text = lobby.id + ": " + lobby.name;
    }
}

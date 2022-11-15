using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvailableLobbiesPanel : MonoBehaviour
{
    public GameObject lobbyContainerParent;
    Dictionary<Lobby, GameObject> lobbies;

    private void Awake()
    {
        lobbies = new Dictionary<Lobby, GameObject>();
    }

    //private void Start()
    //{
    //    StartCoroutine(UpdateLobbiesInfo(1f));
    //}

    //private IEnumerator UpdateLobbiesInfo(float t)
    //{
    //    while (true)
    //    {
    //        List<Lobby> lobbies = Client.GetAllLobbies();
    //        yield return new WaitForSeconds(t);
    //        DisplayLobbies(lobbies);
    //        yield return new WaitForSeconds(t);
    //    }
    //}

    //private void DisplayLobbies(List<Lobby> lobbies)
    //{
    //    if (this.lobbies == null) return;

    //    foreach (Lobby lobby in lobbies)
    //    {
    //        if (this.lobbies.ContainsKey(lobby)) return;
    //        else
    //        {
    //            Debug.Log(lobby.nameL);
    //            Debug.Log(lobby.id);
    //            GameObject lobbyContainer = new GameObject("lobby");
    //            lobbyContainer.transform.SetParent(lobbyContainerParent.transform);
    //            TextMeshProUGUI textMesh = lobbyContainer.AddComponent<TextMeshProUGUI>();
    //            textMesh.SetText(lobby.id + " " + lobby.nameL);
    //            lobbyContainer.transform.localScale = Vector3.one;
    //            textMesh.color = Color.black;
    //            textMesh.alignment = TextAlignmentOptions.Center;
    //            textMesh.fontSize = 15f;
    //            this.lobbies[lobby] = lobbyContainer;
    //        }
    //    }
    //}
}

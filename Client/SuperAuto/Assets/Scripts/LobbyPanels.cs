using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPanels : MonoBehaviour, IPointerClickHandler
{
    // Sprite asset
    [SerializeField]
    private TMP_SpriteAsset spriteAsset;

    // Availables panel references
    private Transform availablesPanel;
    private Transform availablesPanelContent;

    // Create panel references
    private Transform createPanel;
    private Button createButton;
    private TMP_InputField createNameField;
    
    // Current panel references
    private Transform currentPanel;
    private TextMeshProUGUI currentPanelTitle;
    private Transform currentPanelContent;
    private Button exitButton;
    private Button startButton;

    // Other references
    private Button viewLobbiesButton;
    private Button createLobbyButton;
    private Button logoutButton;

    private Dictionary<int, LobbyRow> lobbyRows;
    private Dictionary<string, UserRow> userRows;
    private bool doUpdateLobbyRows;
    private bool doUpdateUserRows;

    private class LobbyRow
    {
        public GameObject gameObject;
        public Lobby lobby;

        public LobbyRow(Lobby l, GameObject r)
        {
            this.lobby = l;
            this.gameObject = r;
        }
    }

    private class UserRow
    {
        public GameObject gameObject;
        public User user;

        public UserRow(User u, GameObject g)
        {
            this.user = u;
            this.gameObject = g;
        }
    }

    private void Awake()
    {
        lobbyRows = new Dictionary<int, LobbyRow>();
        userRows = new Dictionary<string, UserRow>();
        doUpdateLobbyRows = false;
        doUpdateUserRows = false;
    }

    private void Start()
    {
        availablesPanel = transform.Find("Availables Panel");
        availablesPanelContent = availablesPanel.Find("Scroll View/Viewport/Content");

        currentPanel = transform.Find("Current Panel");
        currentPanelTitle = currentPanel.Find("Title").GetComponent<TextMeshProUGUI>();
        currentPanelContent = currentPanel.Find("Scroll View/Viewport/Content");
        exitButton = currentPanel.Find("Exit Button").GetComponent<Button>();
        exitButton.onClick.AddListener(OnClickExit);
        startButton = currentPanel.Find("Start Button").GetComponent<Button>();
        startButton.onClick.AddListener(OnClickStart);

        createPanel = transform.Find("Create Panel");
        createButton = createPanel.Find("Create Button").GetComponent<Button>();
        createNameField = createPanel.Find("Form/LobbyNameField").GetComponent<TMP_InputField>();
        createButton.onClick.AddListener(OnClickCreate);

        viewLobbiesButton = transform.Find("AvailableMenuButton").GetComponent<Button>();
        viewLobbiesButton.onClick.AddListener(() => {
            OnClickExit();
            ShowPanel(Panel.Availables); 
        });
        createLobbyButton = transform.Find("CreateMenuButton").GetComponent<Button>();
        createLobbyButton.onClick.AddListener(() => ShowPanel(Panel.Create));
        logoutButton = transform.Find("LogoutButton").GetComponent<Button>();
        logoutButton.onClick.AddListener(Logout);
        

        StartCoroutine(UpdateAvailablesPanel(1f));
        StartCoroutine(UpdateCurrentPanel(1f));
        ShowPanel(Panel.Availables);
    }

    private enum Panel
    {
        Availables,
        Create,
        Current,
    }

    private void ShowPanel(Panel t)
    {
        switch (t)
        {
            case Panel.Availables:
                doUpdateLobbyRows = true;
                doUpdateUserRows = false;

                availablesPanel.gameObject.SetActive(true);
                createPanel.gameObject.SetActive(false);
                currentPanel.gameObject.SetActive(false);
                break;
            case Panel.Create:
                doUpdateLobbyRows = false;
                doUpdateUserRows = false;

                availablesPanel.gameObject.SetActive(false);
                createPanel.gameObject.SetActive(true);
                currentPanel.gameObject.SetActive(false);
                break;
            case Panel.Current:
                doUpdateLobbyRows = false;
                doUpdateUserRows = true;

                availablesPanel.gameObject.SetActive(false);
                createPanel.gameObject.SetActive(false);
                currentPanel.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnClickStart()
    {
        Client.clientOperations.DoStartLobby();
    }

    private void OnClickCreate()
    {
        Client.clientOperations.DoCreatelobby(createNameField.text);
        ShowPanel(Panel.Current);
    }

    private void OnClickExit()
    {
        Client.clientOperations.DoExitLobby();
        ShowPanel(Panel.Availables);
    }

    private IEnumerator UpdateAvailablesPanel(float rate)
    {
        while (true)
        {
            yield return new WaitUntil(() => doUpdateLobbyRows);

            Dictionary<int, Lobby> lobbies = Client.clientOperations.DoGetLobbies();

            foreach (LobbyRow value in lobbyRows.Values)
            {
                Destroy(value.gameObject);
            }

            lobbyRows.Clear();

            foreach (KeyValuePair<int, Lobby> entry in lobbies)
            {
                GameObject gameObject = CreateLobbyRow(entry.Value);
                lobbyRows.Add(entry.Key, new LobbyRow(entry.Value,gameObject));
            }

            Canvas.ForceUpdateCanvases();

            yield return new WaitForSecondsRealtime(rate);
        }
    }

    private GameObject CreateLobbyRow(Lobby l)
    {
        {
            GameObject gameObject = new GameObject(l.id, typeof(TextMeshProUGUI));
            gameObject.transform.parent = availablesPanelContent;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
            string text = string.Format("{0}-{1}", l.name, l.owner.username);
            gameObject.GetComponent<TextMeshProUGUI>().text = text;
            gameObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

            return gameObject;
        }
    }

    private IEnumerator UpdateCurrentPanel(float rate)
    {
        while (true)
        {
            yield return new WaitUntil(() => doUpdateUserRows);

            (Lobby,bool) ans = Client.clientOperations.DoGetLobby();

            Lobby currentLobby = ans.Item1;
            bool isOwner = ans.Item2;

            currentPanelTitle.text = currentLobby.id + ": " + currentLobby.name;

            foreach (UserRow value in userRows.Values)
            {
                Destroy(value.gameObject);
            }

            userRows.Clear();
            userRows.Add(currentLobby.owner.id, new UserRow(currentLobby.owner, CreateUserRow(currentLobby.owner, true)));

            foreach (User item in currentLobby.GetPlayers())
            {
                GameObject gameObject = CreateUserRow(item, false);
                userRows.Add(item.id, new UserRow(item, gameObject));
            }

            if (isOwner) startButton.gameObject.SetActive(true);
            else startButton.gameObject.SetActive(false);

            Canvas.ForceUpdateCanvases();
            yield return new WaitForSecondsRealtime(rate);
        }
    }

    private GameObject CreateUserRow(User u, bool addCrown)
    {
        GameObject gameObject = new GameObject(u.id, typeof(TextMeshProUGUI));
        gameObject.transform.parent = currentPanelContent;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
        string text = string.Format("{0}", u.username);
        if (addCrown) text = "<sprite index= 0> " + text;
        gameObject.GetComponent<TextMeshProUGUI>().text = text;
        gameObject.GetComponent<TextMeshProUGUI>().spriteAsset = spriteAsset;
        gameObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        return gameObject;
    }

    private void Logout()
    {
        Client.clientOperations.DoLogout();
        SceneManager.LoadScene("LoginScene");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (LobbyRow value in lobbyRows.Values)
        {
            if (value.gameObject == eventData.pointerCurrentRaycast.gameObject)
            {
                Client.clientOperations.DoJoinLobby(value.lobby);
                ShowPanel(Panel.Current);
                return;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static ClientOperations clientOperations;

    //Login page fields
    [SerializeField]
    private TMP_InputField tmpUname;
    [SerializeField]
    private TMP_InputField tmpPass;

    public void _Login()
    {
        string u = tmpUname.text;
        string p = tmpPass.text;
        Login(u, p);
    }

    public void _SignupGo()
    {
        SceneManager.LoadScene("SignupScene");
    }

    public void _SignupBack()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void _SignUp()
    {
        Signup(tmpUname.text, tmpPass.text);
        SceneManager.LoadScene("LoginScene");
    }

    public static void Login(string uname, string pass)
    {
        string hostname = "ec2-54-175-91-171.compute-1.amazonaws.com";
        int port = 50001;

        clientOperations = new ClientOperations();
        clientOperations.SetServer(hostname, port);
        clientOperations.Connect();

        if (clientOperations.DoLogin(uname, pass))
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            clientOperations.Disconnect();
        }
    }

    public static void Signup(string u, string p)
    {
        string hostname = "ec2-54-175-91-171.compute-1.amazonaws.com";
        int port = 50001;

        clientOperations = new ClientOperations();
        clientOperations.SetServer(hostname, port);
        clientOperations.Connect();

        clientOperations.DoSignup(u, p);
        clientOperations. Disconnect();
    }   
}

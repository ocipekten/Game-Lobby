using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Message
{
    private string type;
    private Dictionary<string, string> parameters;

    static string PJOIN = "&";
    static string VJOIN = "=";

    public Message()
    {
        Clear();
    }

    public Message(string s)
    {
        Unmarshal(s);
    }

    public override string ToString()
    {
        return Marshal();
    }

    public void Clear()
    {
        SetType(MCmds.GOOD);
        this.parameters = new Dictionary<string, string>();
    }

    public void SetType(MCmds type)
    {
        this.type = type.ToString();
    }

    public string GetType()
    {
        return this.type;
    }

    public void AddParameter(string pname, string pvalue)
    {
        if (parameters == null) parameters = new Dictionary<string, string>();
        this.parameters[pname] = pvalue;
    }

    public string GetParameter(string pname)
    {
        return this.parameters[pname];
    }

    public string Marshal()
    {
        string parameters = "";
        if (this.parameters != null && this.parameters.Count != 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string key in this.parameters.Keys)
            {
                stringBuilder.Append(key);
                stringBuilder.Append(VJOIN);
                stringBuilder.Append(this.parameters[key]);
                stringBuilder.Append(PJOIN);
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            parameters = stringBuilder.ToString();            
        }

        return string.Format("{0}{1}{2}", parameters.Length.ToString("D4"), this.type, parameters);
    }

    public void Unmarshal(string value)
    {
        if (value != null && value.Length > 0)
        {
            string[] parameters = value.Split(new string[] { PJOIN }, System.StringSplitOptions.None);
            if (parameters.Length < 1) return;
            foreach (string parameter in parameters)
            {
                string[] kv = parameter.Split(new string[] { VJOIN }, System.StringSplitOptions.None);
                if (kv.Length < 2) return;
                AddParameter(kv[0], kv[1]);
            }
        }
    }
}

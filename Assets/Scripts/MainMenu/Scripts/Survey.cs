using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Survey : MonoBehaviour
{
    [SerializeField] InputField feedback;

    private string url = "";
    public void Send()
    {
        StartCoroutine(Post(feedback.text));
    }

    IEnumerator Post(string s1)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.2033066643", s1); 
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        
        yield return www.SendWebRequest();

    }
}

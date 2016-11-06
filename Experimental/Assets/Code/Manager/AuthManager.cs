using UnityEngine;
using System.Net;
using System.Text;
using System.IO;

public class AuthManager : MonoBehaviour
{

	void Start ()
    {
        var request = (HttpWebRequest)WebRequest.Create("http://localhost:8000/coconut/create_user/");

        var postData = "user_name=test";
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        Debug.LogError("Sending request");
        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        Debug.LogError(responseString);
	}
	
	void Update ()
    {
	
	}
}

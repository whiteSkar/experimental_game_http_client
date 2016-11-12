using UnityEngine;
using System.Net;
using System.Text;
using System.IO;
using SimpleJSON;

public class AuthManager : MonoBehaviour
{
    enum ResponseStatus
    {
        SUCCESS = 1,
        FAIL = 2,
        INVALID_ARGUMENT = 3,
        CREATE_USER = 4,
        USER_WITH_SAME_DEVICE_ID_ALREADY_EXIST = 5,
        USER_WITH_SAME_NAME_ALREADY_EXIST = 6,
    }

    void Start()
    {
        var request = (HttpWebRequest)WebRequest.Create("http://localhost:8000/coconut/authenticate_user/");

        var device_id = SystemInfo.deviceUniqueIdentifier;
        Debug.LogError("device_id=" + device_id);
        var postData = "device_id=" + device_id;
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
        var responseJson = JSON.Parse(responseString);
        Debug.LogError(responseJson);

        ResponseStatus responseStatus = (ResponseStatus)responseJson["status"].AsInt;
        switch (responseStatus)
        {
            case ResponseStatus.SUCCESS:
                Debug.LogError(responseStatus.ToString() + ": " + responseJson["user"].Value);
                break;
            case ResponseStatus.FAIL:
            case ResponseStatus.INVALID_ARGUMENT:
                Debug.LogError(responseStatus.ToString() + ": " + responseJson["msg"].Value);
                break;
            case ResponseStatus.CREATE_USER:
                make_create_user_request(device_id, "whiteSkar"); //TODO: get from UI
                break;
            default:
                Debug.LogError("Unhandled response status" + responseStatus.ToString());
                break;
        }
    }
	
	void make_create_user_request(string device_id, string user_name)
    {
        var request = (HttpWebRequest)WebRequest.Create("http://localhost:8000/coconut/create_user/");

        var postData = "device_id=" + device_id + "&user_name=" + user_name;
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
        var responseJson = JSON.Parse(responseString);
        Debug.LogError(responseJson);

        ResponseStatus responseStatus = (ResponseStatus) responseJson["status"].AsInt;
        switch (responseStatus)
        {
            case ResponseStatus.SUCCESS:
            case ResponseStatus.USER_WITH_SAME_DEVICE_ID_ALREADY_EXIST:
                Debug.LogError(responseStatus.ToString() + ": " + responseJson["user"].Value);
                break;
            case ResponseStatus.FAIL:
            case ResponseStatus.INVALID_ARGUMENT:
            case ResponseStatus.USER_WITH_SAME_NAME_ALREADY_EXIST:
                Debug.LogError(responseStatus.ToString() + ": " + responseJson["msg"].Value);
                break;
            default:
                Debug.LogError("Unhandled response status" + responseStatus.ToString());
                break;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;

public class LoginDialogController : MonoBehaviour
{
    public Text userNameText;
    public Text errorMsgText;

    private AuthManager authManager;

    public void setup(AuthManager authManager)
    {
        this.authManager = authManager;

        errorMsgText.gameObject.SetActive(false);
    }

    public void onCreateUserButtonClicked()
    {
        errorMsgText.gameObject.SetActive(false);

        var userName = userNameText.text.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            errorMsgText.text = "USER NAME IS EMPTY";
            errorMsgText.gameObject.SetActive(true);
        }
        else
        {
            Action<string> successCallback = (msg) =>
            {
                gameObject.SetActive(false);
            };

            Action<string> failureCallback = (msg) =>
            {
                errorMsgText.text = msg.ToUpper();
                errorMsgText.gameObject.SetActive(true);
            };

            authManager.sendCreateUserRequest(userName, successCallback, failureCallback);
        }
    }
}

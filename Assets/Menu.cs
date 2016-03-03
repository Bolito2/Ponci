using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;

public class Menu : MonoBehaviour {

    public void InvitarAmigo()
    {
        PlayGamesPlatform.Instance.TurnBased.CreateWithInvitationScreen(1, 1, 0, OnMatchStarted);
    }

    // Callback:
    void OnMatchStarted(bool success, TurnBasedMatch match)
    {
        if (success)
        {
            Debug.Log("xdlol");
        }
        else
        {
            Debug.Log("error");
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class GameControlWithRequests : NetworkBehaviour
{
    public static GameControlWithRequests instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Screw you");
            Destroy(gameObject);
        }
    }
    public struct Seat
    {
        public NetworkObject player;
        public string username;

        public Seat(NetworkObject player, string username)
        {
            this.player = player;
            this.username = username;
        }
    }
    public List<Seat> seats = new List<Seat>();
    public NetworkManager nm;
    public override void OnStartServer()
    {
        // CallStartBattle();
    }
    public override void OnStartClient()
    {
        // base.OnStartClient();
    }


    [ServerRpc(RequireOwnership = false)]
    public void SendServerTeamJoinReq(NetworkObject player, string username)
    {
        seats.Add(new Seat(player, username));
        Debug.Log(seats.Count);
        if (seats.Count != 2) // TODO fix this later
        {
            return;
        }

        foreach (Seat seat in seats)
        {
            if (IsClientInitialized)
            {
                TeamAddLogic(seat.player, seat.username);
            }
            ClientsReceiveTeamAdd(seat.player, seat.username);
        }
        Debug.Log("Battle Starting");
        CallStartBattle(); // TODO fix this you idiot, does not run on server side if not host
        // Debug.Log(teams.Count);
        // Debug.Log(nm.ServerManager.Clients.Count);
    }
    [ObserversRpc(ExcludeServer = true)]
    private void ClientsReceiveTeamAdd(NetworkObject player, string username)
    {
        TeamAddLogic(player, username);
    }

    private void TeamAddLogic(NetworkObject player, string username)
    {
        Debug.Log($"'{player.name}' added to new team");
        // TODO this is not a thing rn
    }



    [ObserversRpc]
    private void CallStartBattle()
    {
        // bi.StartBattle(teams);
        // TODO  this is nto a thing rn
    }


    public void EndGame()
    {
        nm.ClientManager.StopConnection(); // TODO fix this
        if (!IsServerInitialized)
        {
            SceneSwitcher.StaticSwitchScene("PlayAgain");
            return;
        }
        ClientsReceiveEndGame();
        nm.ServerManager.StopConnection(true);
        SceneSwitcher.StaticSwitchScene("PlayAgain");

    }

    [ObserversRpc]
    private void ClientsReceiveEndGame()
    {
        nm.ClientManager.StopConnection();
    }
}

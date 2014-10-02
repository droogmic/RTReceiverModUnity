using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	private const string typeName = "ReceiverMod";
	private const string gameName = "TestRoom";
	private const int maxConnections = 64;
	private const int port = 26062;
	private const bool useNAT = false;
	//private const bool useNAT = !Network.HavePublicAddress();

	private bool isLocal = false;
	private bool DirConn = true;
	private bool useMaster = false;
	private string ipAdd = "127.0.0.1";

	public GameObject playerPrefab;
	public GameObject cameraPrefab;

//	private BallScript ballObjScript;

	// Use this for initialization
	void Start () {
		//DebugConsole.cout(Network.HavePublicAddress ());
		DebugConsole.Log("NetInit");
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI()
	{
		if ((!Network.isClient && !Network.isServer)||!isLocal)
		{
			
			useMaster = GUI.Toggle(new Rect(100, 40, 250, 20), useMaster, "Use Master Server?");
			
			if (GUI.Button(new Rect(100, 100, 250, 80), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 200, 250, 80), "Refresh Hosts"))
				RefreshHostList();
			
			if (GUI.Button(new Rect(100, 300, 250, 80), "Direct Connect"))
				DirectConnect();

			if (GUI.Button(new Rect(100, 400, 250, 80), "Single Player"))
				SinglePlayer();
			
			if (DirConn == true)
				ipAdd = GUI.TextField(new Rect (100, 550, 250, 100), ipAdd, 30);
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}

		}
		else
		{
			if (Network.connections.Length >= 1)
			for (int i=0; i<=Network.connections.Length-1; i++)
			{
				string netlabelstr = "Connection: " + Network.connections[i].externalIP + "  ip: " + Network.connections[i].ipAddress + "  guid: " + Network.connections[i].guid + "  avgping: " + Network.GetAveragePing(Network.connections[i]) + "  lastping: " + Network.GetLastPing(Network.connections[i]);
				GUI.Label(new Rect(200, 20 + 50*i, 800, 50), netlabelstr);
			}

		}
	}

	void OnServerInitialized()
	{
		DebugConsole.Log("Server Initializied");
		//SpawnPlayer();
		SpawnCamera();
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) 
		{
			hostList = MasterServer.PollHostList ();
			DebugConsole.Log("Server List Found");
		}
	}

	void OnConnectedToServer()
	{
		DebugConsole.Log("Server Joined");
		SpawnPlayer();
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		string logstr = "Player Joined: " + player.ToString();
		DebugConsole.Log (logstr);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		DebugConsole.Log ("Player Lost: " + player.ToString());

		Network.DestroyPlayerObjects (player);

	}



	private void StartServer()
	{
		Network.InitializeServer(maxConnections, port, useMaster);

		if (useMaster == true)
			MasterServer.RegisterHost(typeName, gameName);
	}

	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		if (useMaster == true)
			MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	private void DirectConnect()
	{
		Network.Connect(ipAdd, port);
	}

	private void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, new Vector3(Random.Range(-5f, 5f), 30f, Random.Range(-5f, 5f)), Quaternion.identity, 0);
	}

	private void SpawnCamera()
	{
		Instantiate(cameraPrefab, new Vector3(0f, 40f, 0f), Quaternion.identity);
	}

	private void SinglePlayer()
	{
		SpawnLocalPlayer();
	}

	private void SpawnLocalPlayer()
	{
		Instantiate(playerPrefab, new Vector3(Random.Range(-5f, 5f), 30f, Random.Range(-5f, 5f)), Quaternion.identity);
	}

}

using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// Client class shows how to implement and use TcpClient in Unity.
/// </summary>
public class TCPClient : MonoBehaviour
{
	#region Public Variables
	[Header("Network")]
	public static string ipAdress = "127.0.0.1";
	public static int port = 64209;
	public static float waitingMessagesFrequency = 1;
	#endregion

	#region Static Private m_Variables
	private static TcpClient m_Client;
	private static NetworkStream m_NetStream = null;
	private static byte[] m_Buffer = new byte[49152];
	private static int m_BytesReceived = 0;
	private static string m_ReceivedMessage = "";

	// flag to detect whether coroutine is still running to workaround coroutine being stopped after saving scripts while running in Unity
	private int nCoroutineRunning = 0;
	#endregion
	
	void OnValidate ()
	{
	}

	void OnEnable ()
	{
		StartClient();
	}

	void OnDisable ()
	{
		CloseClient();
	}

	public void OnApplicationQuit ()
	{
		CloseClient();
	}

	void Update ()
	{
		if (EnsureConnection()) {

			if (nCoroutineRunning == 0) {

				//Debug.Log ("starting ReadSerialLoop* coroutine from " + this.name);

				switch (Application.platform) {

				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WindowsPlayer:
                        //				case RuntimePlatform.OSXEditor:
                        //				case RuntimePlatform.OSXPlayer:

                        // Each instance has its own coroutine but only one will be active
					StartCoroutine (ListenServerMessages ());
					break;

				default:
                        // Each instance has its own coroutine but only one will be active
					StartCoroutine (ListenServerMessages ());
					break;

				}
			} else {
				if (nCoroutineRunning > 1) {
					Debug.Log (nCoroutineRunning + " coroutines in " + name);
				}

				nCoroutineRunning = 0;
			}
		}
	}

	public static bool IsConnected(){
		return m_Client != null;
	}

	public static bool EnsureConnection(){
        StartClient();
		return IsConnected();
	}

    public static void Write(string message){
        if (EnsureConnection()){
            SendMessageToServer(message);
        }
    }

	//Start client and stablish connection with server
	public static void StartClient()
	{
		//Early out
		if (m_Client != null)
		{
			// ClientLog($"There is already a runing client on {ipAdress}::{port}", Color.red);
			return;
		}

		try
		{
			//Create new client
			m_Client = new TcpClient();
			//Set and enable client
			m_Client.Connect(ipAdress, port);
			ClientLog($"Client Started on {ipAdress}::{port}", Color.green);
		}
		catch (SocketException)
		{
			ClientLog($"The Hapticlabs TCP connection was not found on {ipAdress}::{port}", Color.red);
			CloseClient();
		}
	}

	#region Communication Client<->Server
	//Coroutine waiting server messages
	private IEnumerator ListenServerMessages()
	{
		//early out if there is nothing connected
		if (!m_Client.Connected)
			yield break;

		//Stablish Client NetworkStream information
		m_NetStream = m_Client.GetStream();

		//Start Async Reading from Server and manage the response on MessageReceived function
		do
		{
			// ClientLog("Client is listening server msg...", Color.yellow);
			//Start Async Reading from Server and manage the response on MessageReceived function
			m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, null);

			if (m_BytesReceived > 0)
			{
				OnMessageReceived(m_ReceivedMessage);
				m_BytesReceived = 0;
			}

			yield return new WaitForSeconds(waitingMessagesFrequency);

		} while (m_BytesReceived >= 0 && m_NetStream != null && m_Client != null);
		//Communication is over
	}

	//What to do with the received message on client
	protected virtual void OnMessageReceived(string receivedMessage)
	{
		ClientLog($"Msg recived on Client: <b>{receivedMessage}</b>", Color.green);
		switch (receivedMessage)
		{
			case "Close":
				CloseClient();
				break;
			default:
				ClientLog($"Received message <b>{receivedMessage}</b>, has no special behaviuor", Color.red);
				break;
		}
	}

	//Send custom string msg to server
	public static void SendMessageToServer(string messageToSend)
	{
		try
		{
			m_NetStream = m_Client.GetStream();
		}
		catch (Exception)
		{
			ClientLog("Non-Connected Socket exception", Color.red);
			CloseClient();
			return;
		}

		//early out if there is nothing connected
		if (!m_Client.Connected)
		{
			ClientLog("Socket Error: Stablish Server connection first", Color.red);
			return;
		}

		//Build message to server
		byte[] encodedMessage = Encoding.ASCII.GetBytes(messageToSend); //Encode message as bytes

		//Start Sync Writing
		m_NetStream.Write(encodedMessage, 0, encodedMessage.Length);
		ClientLog($"Msg sent to Server: <b>{messageToSend}</b>", Color.blue);
	}

	//AsyncCallback called when "BeginRead" is ended, waiting the message response from server
	private void MessageReceived(IAsyncResult result)
	{
		if (result.IsCompleted && m_Client.Connected)
		{
			//build message received from server
			m_BytesReceived = m_NetStream.EndRead(result);
			m_ReceivedMessage = Encoding.ASCII.GetString(m_Buffer, 0, m_BytesReceived);
		}
	}
	#endregion

	#region Close Client
	//Close client connection
	public static void CloseClient()
	{
		ClientLog("Client Closed", Color.red);

		//Reset everything to defaults
		if (m_Client != null && m_Client.Connected)
			m_Client.Close();

		if (m_Client != null)
			m_Client = null;
	}
	#endregion

	#region ClientLog
	//Custom Client Log - With Text Color
	protected static void ClientLog(string msg, Color color)
	{
		Debug.Log($"<b>Client:</b> {msg}");
	}
	//Custom Client Log - Without Text Color
	protected virtual void ClientLog(string msg)
	{
		Debug.Log($"<b>Client:</b> {msg}");
	}
	#endregion

}
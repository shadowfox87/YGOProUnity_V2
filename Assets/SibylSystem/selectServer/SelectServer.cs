using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

class serverT
{
	public string name;
	public string ip;
	public string port;
}
public class SelectServer : WindowServantSP
{
	UIPopupList list;
	UIPopupList serversList;
	List<serverT> servers;
	UIInput inputIP;
	UIInput inputPort;
	UIInput inputPsw;
	UIInput inputVersion;
	string currentClientVersion = "0x" + String.Format("{0:X}", Config.ClientVersion);

	public override void initialize()
	{
		servers = new List<serverT>();

		createWindow(Program.I().new_ui_selectServer);
		UIHelper.registEvent(gameObject, "exit_", onClickExit);
		UIHelper.registEvent(gameObject, "face_", onClickFace);
		UIHelper.registEvent(gameObject, "join_", onClickJoin);
		UIHelper.registEvent(gameObject, "roomList_", onClickRoomList);
		UIHelper.registEvent(gameObject, "quickSingle_", onQuickSingle);
		UIHelper.registEvent(gameObject, "quickMatch_", onQuickMatch);
		UIHelper.registEvent(gameObject, "quickAI_", onQuickAI);
		UIHelper.registEvent(gameObject, "quickTag_", onQuickTag);
		serversList = UIHelper.getByName<UIPopupList>(gameObject, "server");
		serversList.fontSize = 20;
		serversList.value = Config.Get("serversPicker", "Custom");
		UIHelper.registEvent(gameObject, "server", pickServer);
		UIHelper.getByName<UIInput>(gameObject, "name_").value = Config.Get("name", "YGOPro2 User");
		UIHelper.getByName<UIInput>(gameObject, "name_").defaultText = "YGOPro2 User";
		list = UIHelper.getByName<UIPopupList>(gameObject, "history_");
		UIHelper.registEvent(gameObject, "history_", onSelected);
		name = Config.Get("name", "YGOPro2 User");
		inputIP = UIHelper.getByName<UIInput>(gameObject, "ip_");
		inputIP.defaultText = "szefoserver.ddns.net";
		inputPort = UIHelper.getByName<UIInput>(gameObject, "port_");
		inputPort.defaultText = "7210";
		inputPsw = UIHelper.getByName<UIInput>(gameObject, "psw_");
		inputPsw.defaultText = "";
		inputVersion = UIHelper.getByName<UIInput>(gameObject, "version_");
		set_version(currentClientVersion);
		SetActiveFalse();
		string[] lines = File.ReadAllLines("config/servers.conf");
		foreach (string s in lines)
		{
			serverT add = new serverT();
			string[] mats = s.Split("->");
			add.name = mats[0];
			string[] mats2 = mats[1].Split(",");
			add.ip = mats2[0];
			add.port = mats2[1];
			servers.Add(add);
			serversList.items.Add(add.name);
		}
		serverT custom = new serverT() { name = name = "Custom", ip = "", port = "" };
		servers.Add(custom);
		serversList.items.Add(custom.name);
	}
	private void pickServer()
	{
		string server = serversList.value;
		//[TCG/OCG]Szefoserver
		//[TCG]Koishi
		//[OCG]Mercury233
		foreach (serverT s in servers)
		{
			if (s.name == server)
			{
				inputIP.value = s.ip;
				inputPort.value = s.port;
				inputVersion.value = currentClientVersion;
				Config.Set("serversPicker", s.name);
				break;
			}
		}

	}

	public void onQuickSingle()
	{
		if (!isShowed)
		{
			return;
		}
		onQuick("");
	}
	public void onQuickAI()
	{
		if (!isShowed)
		{
			return;
		}
		onQuick("AI");
	}
	public void onQuickTag()
	{
		if (!isShowed)
		{
			return;
		}
		onQuick("T");
	}
	void printFile()
	{
		list.Clear();
		if (File.Exists("config/hosts.conf") == false)
		{
			File.Create("config/hosts.conf").Close();
		}
		string txtString = File.ReadAllText("config/hosts.conf");
		string[] lines = txtString.Replace("\r", "").Split("\n");
		for (int i = 0; i < lines.Length; i++)
		{
			list.AddItem(lines[i]);
		}
	}
	public void onQuickMatch()
	{
		if (!isShowed)
		{
			return;
		}
		onQuick("M");
	}
	private void onQuick(string psw)
	{
		string Name = UIHelper.getByName<UIInput>(gameObject, "name_").value;
		string ipString = UIHelper.getByName<UIInput>(gameObject, "ip_").value;
		string portString = UIHelper.getByName<UIInput>(gameObject, "port_").value;
		string pswString = psw;
		string versionString = UIHelper.getByName<UIInput>(gameObject, "version_").value;
		if (versionString == "")
		{
			UIHelper.getByName<UIInput>(gameObject, "version_").value = currentClientVersion;
			versionString = currentClientVersion;
		}
		KF_onlineGame(Name, ipString, portString, versionString, pswString);
	}
	public void onClickRoomList()
	{
		if (!isShowed)
		{
			return;
		}
		string Name = UIHelper.getByName<UIInput>(gameObject, "name_").value;
		string ipString = UIHelper.getByName<UIInput>(gameObject, "ip_").value;
		string portString = UIHelper.getByName<UIInput>(gameObject, "port_").value;
		string pswString = "L";
		string versionString = UIHelper.getByName<UIInput>(gameObject, "version_").value;
		if (versionString == "")
		{
			UIHelper.getByName<UIInput>(gameObject, "version_").value = currentClientVersion;
			versionString = currentClientVersion;
		}
		KF_onlineGame(Name, ipString, portString, versionString, pswString);
	}

	void onSelected()
	{
		if (list != null)
		{
			readString(list.value);
		}
	}

	private void readString(string str)
	{
		str = str.Substring(1, str.Length - 1);
		string version = "", remain = "";
		string[] splited;
		splited = str.Split(")");
		try
		{
			version = splited[0];
			remain = splited[1];
		}
		catch (Exception)
		{
		}
		splited = remain.Split(":");
		string ip = "";
		try
		{
			ip = splited[0];
			remain = splited[1];
		}
		catch (Exception)
		{
		}
		splited = remain.Split(" ");
		string psw = "", port = "";
		try
		{
			port = splited[0];
			psw = splited[1];
		}
		catch (Exception)
		{
		}
		inputIP.value = ip;
		inputPort.value = port;
		inputPsw.value = psw;
		//inputVersion.value = version;
	}

	public override void show()
	{
		base.show();
		Program.I().room.RMSshow_clear();
		printFile();
		pickServer();
		Program.charge();
	}

	public override void preFrameFunction()
	{
		base.preFrameFunction();
		Menu.checkCommend();
	}

	void onClickExit()
	{
		//Program.I().roomList.hide();
		Program.I().shiftToServant(Program.I().menu);
		if (TcpHelper.tcpClient != null)
		{
			if (TcpHelper.tcpClient.Connected)
			{
				TcpHelper.tcpClient.Close();
			}
		}
	}

	public void set_version(string str)
	{
		UIHelper.getByName<UIInput>(gameObject, "version_").value = str;
	}

	void onClickJoin()
	{
		if (!isShowed)
		{
			return;
		}
		string Name = UIHelper.getByName<UIInput>(gameObject, "name_").value;
		string ipString = UIHelper.getByName<UIInput>(gameObject, "ip_").value;
		string portString = UIHelper.getByName<UIInput>(gameObject, "port_").value;
		string pswString = UIHelper.getByName<UIInput>(gameObject, "psw_").value;
		string versionString = UIHelper.getByName<UIInput>(gameObject, "version_").value;
		if (versionString == "")
		{
			UIHelper.getByName<UIInput>(gameObject, "version_").value = currentClientVersion;
			versionString = currentClientVersion;
		}
		KF_onlineGame(Name, ipString, portString, versionString, pswString);

	}

	public void KF_onlineGame(string Name, string ipString, string portString, string versionString, string pswString = "")
	{
		name = Name;
		Config.Set("name", name);
		if (ipString == "" || portString == "" || versionString == "")
		{
			RMSshow_onlyYes("", InterString.Get("非法输入！请检查输入的主机名。"), null);
		}
		else
		{
			if (name != "")
			{
				if (pswString != "L")
				{
					string fantasty = "(" + versionString + ")" + ipString + ":" + portString + " " + pswString;
					list.items.Remove(fantasty);
					list.items.Insert(0, fantasty);
					list.value = fantasty;
					if (list.items.Count > 5)
					{
						list.items.RemoveAt(list.items.Count - 1);
					}
					string all = "";
					for (int i = 0; i < list.items.Count; i++)
					{
						all += list.items[i] + "\r\n";
					}
					File.WriteAllText("config/hosts.conf", all);
				}
				(new Thread(() => { TcpHelper.join(ipString, name, portString, pswString, versionString); })).Start();
			}
			else
			{
				RMSshow_onlyYes("", InterString.Get("昵称不能为空。"), null);
			}
		}
	}

	GameObject faceShow = null;

	public string name = "";

	void onClickFace()
	{
		name = UIHelper.getByName<UIInput>(gameObject, "name_").value;
		RMSshow_face("showFace", name);
		Config.Set("name", name);
	}

}
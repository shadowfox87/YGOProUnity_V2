using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

public class RoomList : WindowServantSP
{
    UIselectableList superScrollView = null;
    List<string[]> listOfRooms = new List<string[]>();
    public override void initialize()
    {
        createWindow(Program.I().new_ui_RoomList);
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        UIHelper.registEvent(gameObject, "refresh_", onRefresh);
        UIHelper.getByName<UIToggle>(gameObject, "hideAIrooms_").value = UIHelper.fromStringToBool(Config.Get("hideAIrooms_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "hideStarted_").value = UIHelper.fromStringToBool(Config.Get("hideStarted_", "1"));
        UIHelper.registEvent(gameObject, "hideAIrooms_", save);
        UIHelper.registEvent(gameObject, "hideStarted_", save);
        superScrollView = gameObject.GetComponentInChildren<UIselectableList>();
        superScrollView.selectedAction = onSelected;
        superScrollView.install();
        SetActiveFalse();
    }

    private void save()
    {
        Config.Set("hideAIrooms_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "hideAIrooms_").value));
        Config.Set("hideStarted_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "hideStarted_").value));
    }

    private void onRefresh()
    {
        Program.I().selectServer.onClickRoomList();
    }

    public void UpdateList(List<string[]> roomList)
    {
        listOfRooms.Clear();
        listOfRooms.AddRange(roomList);
        printFile();
    }
    public void onClickExit()
    {
        hide();
    }
    public override void show()
    {
        base.show();
    }

    private void printFile()
    {
        superScrollView.clear();
        superScrollView.toTop();
        if (UIHelper.getByName<UIToggle>(gameObject, "hideAIrooms_").value)
        {
            listOfRooms.RemoveAll(s => s[11].Contains("AI"));
        }
        if (UIHelper.getByName<UIToggle>(gameObject, "hideStarted_").value)
        {
            listOfRooms.RemoveAll(s => Convert.ToInt32(s[10]) != 0);
        }
        listOfRooms.TrimExcess();
        listOfRooms = listOfRooms.OrderBy(s => s[3]).ToList();
        foreach (string[] room in listOfRooms)
        {
            superScrollView.add(room[9]);
        }
        //for (int i = 0; i < listOfRooms.Count; i++)
        //{

        //    //if (listOfRooms[i].Length > 4)
        //    //{
        //    //    //if (/*listOfRooms[i].Substring(fileInfos[i].Name.Length - 4, 4) == ".lua"*/)
        //    //    //{
        //    //    //   // superScrollView.add(listOfRooms[i].Substring(0, listOf.Name.Length - 4));
        //    //    //}
        //    //}
        //}
    }

    string selectedString = string.Empty;
    void onSelected()
    {
        if (!isShowed)
        {
            return;
        }
        if (selectedString == superScrollView.selectedString)
        {
            string roomPsw = listOfRooms.Find(s => s[9] == selectedString)[2];
            JoinRoom(superScrollView.selectedString,roomPsw);
        }
        selectedString = superScrollView.selectedString;
    }

    void JoinRoom(string selectedString,string roomPsw)
    {
        string Name = UIHelper.getByName<UIInput>(Program.I().selectServer.gameObject, "name_").value;
        string ipString = UIHelper.getByName<UIInput>(Program.I().selectServer.gameObject, "ip_").value;
        string portString = UIHelper.getByName<UIInput>(Program.I().selectServer.gameObject, "port_").value;
        string pswString = roomPsw;
        string versionString = UIHelper.getByName<UIInput>(Program.I().selectServer.gameObject, "version_").value;
        if (versionString == "")
        {
            UIHelper.getByName<UIInput>(Program.I().selectServer.gameObject, "version_").value = "0x1348";
            versionString = "0x1348";
        }
        Program.I().roomList.hide();
        Program.I().selectServer.KF_onlineGame(Name, ipString, portString, versionString, pswString);
    }
}
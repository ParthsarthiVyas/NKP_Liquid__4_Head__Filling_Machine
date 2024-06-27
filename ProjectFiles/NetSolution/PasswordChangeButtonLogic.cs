#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.Core;
using FTOptix.Recipe;
using FTOptix.AuditSigning;
using FTOptix.Alarm;
using FTOptix.WebUI;
using FTOptix.SQLiteStore;
using FTOptix.MicroController;
using FTOptix.CommunicationDriver;
using FTOptix.RAEtherNetIP;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.OPCUAClient;
#endregion

public class PasswordChangeButtonLogic : BaseNetLogic
{
    [ExportMethod]
    public void ChangePassword()
    {
        
        var passchgdialogbox = LogicObject.GetAlias("PasswordExpiredDialogType") as DialogType;
        if (passchgdialogbox == null)
        {
            Log.Error("PasswordChangeButtonLogic", "Missing PasswordExpiredDialogType alias");
            return;
        }

        var CurrUserName = Session.User;
        var ownerButton = (Button)Owner;
        passchgdialogbox.GetVariable("ShowCurrentUser").Value = true;
        passchgdialogbox.GetVariable("ShowLableText").Value = false;
        passchgdialogbox.GetVariable("ShowFirstLogonText").Value = false;
        ownerButton.OpenDialog(passchgdialogbox, CurrUserName.NodeId);
        
    }
}

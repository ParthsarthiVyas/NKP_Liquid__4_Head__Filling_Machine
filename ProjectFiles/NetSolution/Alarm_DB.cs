#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.Recipe;
using FTOptix.EventLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.Report;
using FTOptix.Retentivity;
using FTOptix.AuditSigning;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using System.IO;
using System.Threading;
using FTOptix.RAEtherNetIP;
using FTOptix.Modbus;
#endregion

public class Alarm_DB : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        //Label Message2 = (Label)LogicObject.Owner.Owner.Get("Label4");
        //Message2.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        //Label Message2 = (Label)LogicObject.Owner.Owner.Get("Label4");
        //Message2.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    [ExportMethod]

    public void backup1()
    {
        //  Label Message2 = (Label)LogicObject.Owner.Owner.Get("Label4");
        string message1 = Project.Current.GetVariable("Model/Backupmessage2").Value;
        
        string filePath = Project.Current.GetVariable("Model/Databasepath/AlarmDB_Path").Value;

        if (File.Exists(filePath))
        {
            Console.WriteLine("File exists.");
           // Message2.Text = "Alarm DB Backup Created Successfully";
           message1 = "Alarm DB Backup Created Successfully";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Alarm DB Backup Created Successfully", Session.User.BrowseName, "Database Backup");
        }
        else
        {
            Console.WriteLine("File does not exist.");
            //Message2.Text = "Error!, Alarm DB Backup Could Not Created.";
            message1 = "Error!, Alarm DB Backup Could Not Created.";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Error!, Alarm DB Backup Could Not Created.", Session.User.BrowseName, "Database Backup");
        }
        Project.Current.GetVariable("Model/Backupmessage2").Value = message1;  
        Thread.Sleep(3000);
        //Message2.Text = "";
        Project.Current.GetVariable("Model/Backupmessage2").Value = "";
    }

}

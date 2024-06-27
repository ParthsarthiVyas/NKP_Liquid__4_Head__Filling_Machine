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
using System.Xml.Linq;
using FTOptix.RAEtherNetIP;
using FTOptix.Modbus;
#endregion

public class Audit_DB : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        //Label Message1 = (Label)LogicObject.Owner.Owner.Owner.Get("Label3");
        //Message1.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        //   Label Message1 = (Label)LogicObject.Owner.Owner.Owner.Get("Label3");
        // Message1.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    [ExportMethod]

    public void backup()
    {
        //Label Message1 = (Label)LogicObject.Owner.Owner.Owner.Get("Label3");
        string message1 = Project.Current.GetVariable("Model/Backupmessage1").Value;
        string filePath = Project.Current.GetVariable("Model/Databasepath/AuditDB_Path").Value;

        if (File.Exists(filePath))
        {
            Console.WriteLine("File exists.");
            //Message1.Text = "Audit DB Backup Created Successfully";
            message1 = "Audit DB Backup Created Successfully";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Audit DB Backup Created Successfully", Session.User.BrowseName, "Database Backup");
        }
        else
        {
            Console.WriteLine("File does not exist.");
            //Message1.Text = "Error!, Audit DB Backup Could Not Created.";
            message1 = "Error!, Audit DB Backup Could Not Created.";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Error!, Audit DB Backup Could Not Created.", Session.User.BrowseName, "Database Backup");
        }
        Project.Current.GetVariable("Model/Backupmessage1").Value = message1;
        Thread.Sleep(3000);
        //Message3.Text = "";
        Project.Current.GetVariable("Model/Backupmessage1").Value = "";
    }


}

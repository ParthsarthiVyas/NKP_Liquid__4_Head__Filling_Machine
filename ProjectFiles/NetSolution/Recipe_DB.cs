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

public class Recipe_DB : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        // Label Message3 = (Label)LogicObject.Owner.Owner.Get("Label5");
        //Message3.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        //Label Message3 = (Label)LogicObject.Owner.Owner.Get("Label5");
        //Message3.Text = "";
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    [ExportMethod]

    public void backup()
    {
        // Label Message3 = (Label)LogicObject.Owner.Owner.Get("Label5");
        string message1 = Project.Current.GetVariable("Model/Backupmessage3").Value;
        string filePath = Project.Current.GetVariable("Model/Databasepath/RecipeDB_Path").Value;

        if (File.Exists(filePath))
        {
            Console.WriteLine("File exists.");
            //Message3.Text = "Recipe DB Backup Created Successfully";
            message1 = "Recipe DB Backup Created Successfully";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Recipe DB Backup Created Successfully", Session.User.BrowseName, "Database Backup");
        }
        else
        {
            Console.WriteLine("File does not exist.");
            //Message3.Text = "Error!, Audit DB Backup Could Not Created.";
            message1 = "Error!, Audit DB Backup Could Not Created.";
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Error!, Recipe DB Backup Could Not Created.", Session.User.BrowseName, "Database Backup");
        }
        Project.Current.GetVariable("Model/Backupmessage3").Value = message1;
        Thread.Sleep(3000);
        //Message3.Text = "";
        Project.Current.GetVariable("Model/Backupmessage3").Value = "";
    }


}

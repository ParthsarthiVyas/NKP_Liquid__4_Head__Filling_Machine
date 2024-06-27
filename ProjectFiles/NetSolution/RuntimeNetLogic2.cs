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

public class RuntimeNetLogic2 : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        //Label Message = (Label)Project.Current.Get("User_Management/Datamanagement/Rectangle1/Rectangle2/Rectangle1/HorizontalLayout2/HorizontalLayout3/VerticalLayout1/Rectangle1/HorizontalLayout1/VerticalLayout1/Label2");
        //Label Message;  = (Label)LogicObject.Owner.Owner.Owner.Children.Get("Label2");
      //  Message.Text = "";
      string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        // Label Message = (Label)Project.Current.Get("User_Management/Datamanagement/Rectangle1/Rectangle2/Rectangle1/HorizontalLayout2/HorizontalLayout3/VerticalLayout1/Rectangle1/HorizontalLayout1/VerticalLayout1/Label2");
        //Label Message = (Label)LogicObject.Owner.Owner.Owner.Children.Get("Label2");
        //Message.Text = ""; 
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        message1 = "";
    }

    [ExportMethod]

    public void backup()
    {
        //Label Message = (Label) LogicObject.Owner.Owner.Owner.Children.Get("Label2");
        string message1 = Project.Current.GetVariable("Model/Backupmessage").Value;
        
        string filePath = Project.Current.GetVariable("Model/Databasepath/BachDB_Path").Value;

            if (File.Exists(filePath))
            {
               Console.WriteLine("File exists.");
            //Message.Text = "Batch DB Backup Created Successfully";
                message1 = "Batch DB Backup Created Successfully";
                AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
                 RecipeSavedLog.LogIntoAudit("DB", "Batch DB Backup Created Successfully", Session.User.BrowseName, "Database Backup");
                
            }
            else
            {
                Console.WriteLine("File does not exist.");
              message1 = "Error!, Batch DB Backup Could Not Created.";
                AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
             RecipeSavedLog.LogIntoAudit("DB", "Error!, Batch DB Backup Could Not Created.", Session.User.BrowseName, "Database Backup");
            }
        Project.Current.GetVariable("Model/Backupmessage").Value = message1;
        Thread.Sleep(3000);
        // Message .Text = "";
        Project.Current.GetVariable("Model/Backupmessage").Value = "";
    }


}

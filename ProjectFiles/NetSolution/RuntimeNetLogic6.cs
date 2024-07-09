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
using System.Threading;
using FTOptix.RAEtherNetIP;
using FTOptix.Modbus;
using System.IO;
#endregion

public class RuntimeNetLogic6 : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        string message1 = Project.Current.GetVariable("Model/RestoreDBMessage").Value;
        message1 = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        string message1 = Project.Current.GetVariable("Model/RestoreDBMessage").Value;
        message1 = "";
    }

    [ExportMethod]
    public void restore(string selectedpath)
    {
        SQLiteStore db = (SQLiteStore)Project.Current.Get<Store>("DataStores/Recipe_DB");
        string message1 = Project.Current.GetVariable("Model/RestoreDBMessage").Value;
        ResourceUri selectedpath1 = selectedpath;
        if (selectedpath.StartsWith("file:///"))
        {
            selectedpath = selectedpath.Substring("file:///".Length);
        }

        // Extract the last part of the path after the final '/'
        string fileName = Path.GetFileName(selectedpath);

        // Perform the restore operation only if the extracted file name matches the expected format
        if (!string.IsNullOrEmpty(fileName) && fileName.StartsWith("RecipeDB") && fileName.EndsWith(".bak"))
        {
            db.Restore(selectedpath1);

            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Recipe DB Restore", Session.User.BrowseName, "Database Restore");
            message1 = "Database is Restoring";
        }
        else
        {
            AuditTrailLogging RecipeSavedLog = new AuditTrailLogging();
            RecipeSavedLog.LogIntoAudit("DB", "Recipe DB Wrong Selection", Session.User.BrowseName, "Database Restore");
            message1 = "Selected Backup Is Not Recipe Database, Please Select Proper Database.";
        }

        Project.Current.GetVariable("Model/RestoreDBMessage").Value = message1;
        Thread.Sleep(3000);
        Project.Current.GetVariable("Model/RestoreDBMessage").Value = "";
    }
}

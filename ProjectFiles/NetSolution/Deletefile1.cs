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
using FTOptix.Report;
using FTOptix.MicroController;
using FTOptix.Retentivity;
using FTOptix.AuditSigning;
using FTOptix.CommunicationDriver;
using FTOptix.System;
using FTOptix.RAEtherNetIP;
using FTOptix.SerialPort;
using FTOptix.UI;
using FTOptix.Core;
using FTOptix.Alarm;
using System.IO;
using System.Threading;
using FTOptix.OPCUAServer;
using FTOptix.OPCUAClient;
#endregion

public class Deletefile1 : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label1");
        resultLabel.Text = "";
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label1");
        resultLabel.Text = "";
    }

    [ExportMethod]
    public void deletefile(string Selectedpath)
    {
        string filePath = Selectedpath;
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label1");
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("File deleted successfully.");
                resultLabel.Text = "File deleted successfully.";
            }
            else
            {
                Console.WriteLine("File does not exist.");
                resultLabel.Text = "File does not exist.";
            }
            Thread.Sleep(5000);
            resultLabel.Text = "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

}

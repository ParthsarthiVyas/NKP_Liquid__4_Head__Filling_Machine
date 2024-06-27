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
using FTOptix.CommunicationDriver;
using FTOptix.AuditSigning;
using FTOptix.System;
using FTOptix.RAEtherNetIP;
using FTOptix.SerialPort;

using FTOptix.Core;
using System.IO;
using System.Threading;
using FTOptix.OPCUAServer;
using FTOptix.OPCUAClient;
#endregion

public class File_move_to_usb : BaseNetLogic
{


    public override void Start()
    {
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label2");
        resultLabel.Text = "";

    }

    public override void Stop()
    {
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label2");
        resultLabel.Text = "";
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void movepdf(string Selectedpath, string Filename1)
    {
        string sourceFolder = Selectedpath;
        string destinationFolder = @"/storage/usb1/"; // Destinationpath;

        string input = Selectedpath;
        char delimiter = '/';
        string[] parts = input.Split(delimiter);

        Filename1 = parts[parts.Length - 1];
        string fileName = Filename1; // Replace with the actual file name

        string sourceFilePath = sourceFolder;
        string destinationFilePath = Path.Combine(destinationFolder, fileName);
        Label resultLabel = (Label)LogicObject.Owner.Owner.Get("Label2");

        try
        {
            // Check if the file exists in the source folder
            if (File.Exists(sourceFilePath))
            {
                // Check if the destination folder exists, create it if it doesn't
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                // Copy the file to the destination folder
                File.Copy(sourceFilePath, destinationFilePath);
                Console.WriteLine("File copied successfully.");
                resultLabel.Text = "File copied successfully.";
            }
            else
            {
                Console.WriteLine("File does not exist in the source folder.");
                resultLabel.Text = "File does not exist in the source folder.";
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

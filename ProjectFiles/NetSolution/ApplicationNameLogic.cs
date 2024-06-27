#region Using directives
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.SerialPort;
using FTOptix.Alarm;
using FTOptix.Recipe;
using FTOptix.Report;
using FTOptix.Store;
using FTOptix.AuditSigning;
using FTOptix.EventLogger;
using FTOptix.SQLiteStore;
using FTOptix.MicroController;
using FTOptix.CommunicationDriver;
using FTOptix.RAEtherNetIP;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.OPCUAClient;
#endregion

public class ApplicationNameLogic : BaseNetLogic
{
    public override void Start()
    {
        Label label = Owner as Label;
        label.Text = Project.Current.BrowseName;
    }

    public override void Stop()
    {
    }
}

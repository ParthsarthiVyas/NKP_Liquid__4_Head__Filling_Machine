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
using FTOptix.System;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.AuditSigning;
using FTOptix.SerialPort;
using FTOptix.Core;
using System.Threading;
using FTOptix.RAEtherNetIP;
#endregion

public class RuntimeNetLogic1 : BaseNetLogic
{
    public override void Start()
    {
        onTimeout = (MethodInvocation)LogicObject.Get("OnTimeout");
        if (onTimeout == null)
            throw new CoreConfigurationException("Unable to find OnTimeout method invocation");
        // Initialize and start the monitoring thread with a 500ms interval
        periodicTask = new PeriodicTask(MonitorPowerValue, 1000, LogicObject);
        periodicTask.Start();
    }

    public override void Stop()
    {
        // Stop the monitoring thread
        periodicTask.Dispose();
        periodicTask = null;
    }

    private void MonitorPowerValue()
    {

        //var powerVariable = LogicObject.GetVariable("power");
        var powerVariable = Project.Current.GetVariable("Model/Powerfail").Value;
        if (powerVariable == null)
        {

            throw new Exception("Variable 'power' not found.");
        }

        int powerValue = powerVariable;
        if (powerValue == 96)
        {
            onTimeout.Invoke();
            
        }
        
    }

    private MethodInvocation onTimeout;
    private PeriodicTask periodicTask;
}


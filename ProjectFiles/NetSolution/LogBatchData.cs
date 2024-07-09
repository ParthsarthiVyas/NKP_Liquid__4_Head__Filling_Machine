#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Core;
using FTOptix.WebUI;
using FTOptix.DataLogger;
using System.Data;
using Microsoft.VisualBasic;
using System.Globalization;
using FTOptix.Alarm;
using FTOptix.RAEtherNetIP;
#endregion

public class LogBatchData : BaseNetLogic
{
    [ExportMethod]
    public void BatchDataLogging()
    {

        Project.Current.GetVariable("Model/BatchData_Station_1/BatchStopTime_1").Value = Project.Current.GetVariable("NetLogic/mm_dd_yyyy_clock1/datetime").Value;


        Store myStore = Project.Current.Get<Store>("DataStores/EDB_Batch_Report");

        Table myTable = myStore.Tables.Get<Table>("BatchInfo");



        string batchno = Project.Current.GetVariable("Model/BatchData_Station_1/BatchNumber").Value;
        string batchstart = Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value;
        string batchstop = Project.Current.GetVariable("Model/BatchData_Station_1/BatchStopTime_1").Value;
        string prodname = Project.Current.GetVariable("Model/BatchData_Station_1/ProductName").Value;
        string Username = Project.Current.GetVariable("Model/BatchData_Station_1/Operatorname").Value;
        int totalcount = Project.Current.GetVariable("Model/BatchData_Station_1/TotalCounts").Value;
        int rejectcount = Project.Current.GetVariable("Model/BatchData_Station_1/RejectCounts").Value;
        int goodcount = Project.Current.GetVariable("Model/BatchData_Station_1/GoodCounts").Value;
        string Companyname = Project.Current.GetVariable("Model/BatchData_Station_1/CompanyName").Value;
        float Batchsize = Project.Current.GetVariable("Model/BatchData_Station_1/Batch_size").Value;
        string Recipename = Project.Current.GetVariable("Model/BatchData_Station_1/Recipename").Value;
        string EquipmentName = Project.Current.GetVariable("Model/BatchData_Station_1/EquipmentName").Value;
        string EquipmentId = Project.Current.GetVariable("Model/BatchData_Station_1/Equipmentid").Value;
        float Productsize = Project.Current.GetVariable("Model/BatchData_Station_1/Product_size").Value;
        float rejection = Project.Current.GetVariable("Model/BatchData_Station_1/%rejection").Value;
        int Duration_Hour = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Hour").Value;
        int Duration_Second = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Second").Value;
        int Duration_Minute = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Minute").Value;

        //string batchstop1 = batchstop.ToString("MMM d, yyyy, h:mm:ss g(en-US)");



        /* object[,] rawValues = new object[1, 14]; // [Raw, Column]; Column = number columns in Table of Audit event logger  database 
         rawValues[0, 0] = DateTime.Now;
         rawValues[0, 1] = batchno;
         rawValues[0, 2] = batchstart;
         rawValues[0, 3] = batchstop;
         rawValues[0, 4] = prodname;
         rawValues[0, 5] = Username;
         rawValues[0, 6] = totalcount;
         rawValues[0, 7] = rejectcount;
         rawValues[0, 8] = goodcount;
         rawValues[0, 9] = Batchsize;
         rawValues[0, 10] = Recipename;
         rawValues[0, 11] = Companyname;
         rawValues[0, 12] = EquipmentName;
         rawValues[0, 13] = EquipmentId;
        */
        string Selectquery = "UPDATE BatchInfo SET BatchStopTime = '" + batchstop + "', TotalCounts =" + totalcount + ", RejectCounts = " + rejectcount + ", GoodCounts =" + goodcount + ", rejection =" + rejection + " , Duration_Hour =" + Duration_Hour + " , Duration_Minute =" + Duration_Minute + " , Duration_Second =" + Duration_Second + " WHERE BatchNumber = '" + batchno + "'";
        //Object[,] ResultSet;
        //String[] Header;
        myStore.Query(Selectquery, out String[] Header, out Object[,] ResultSet);
        Log.Info("Batch,Batch Stored");


        // string[] columns = new string[14] { "LocalTimeStamp", "BatchNumber", "BatchStartTime", "BatchStopTime", "ProductName", "Username", "TotalCounts", "RejectCounts", "GoodCounts", "BatchSize", "Recipename", "Companyname", "EquipmentName", "EquipmentId" };
        //myTable.Insert(columns, rawValues);



        BatchAudit BatchStart = new BatchAudit();
        BatchStart.LogIntoAudit("Batch Stop", "Batch Stop, Batch No. " + batchno, Session.User.BrowseName, "BatchStop");
        Project.Current.GetVariable("Model/Event_Message").Value = "Batch Stop, Batch NO." + Project.Current.GetVariable("Model/BatchData_Station_1/BatchNumber").Value;

        Project.Current.GetVariable("Model/BatchData_Station_1/BatchRunning").Value = false;
        Project.Current.GetVariable("Model/BatchData_Station_1/Batch_start_stop_plc").Value = true;

    }


}

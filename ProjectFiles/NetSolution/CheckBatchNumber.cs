#region Using directives
using System;
using System.Text.RegularExpressions;
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
using FTOptix.Alarm;
using FTOptix.RAEtherNetIP;
using FTOptix.EventLogger;
using FTOptix.Modbus;
#endregion

public class CheckBatchNumber : BaseNetLogic
{
    /*
    private Button _confirmButton;
    private UANode msglabel;
    
    public override void Start()
    {
       _confirmButton = Owner.Get<Button>("Confirm");
       msglabel = Project.Current.Get<Label>("UI/Screens/08Reports/BatchNumberDialog/LabelMsg");
    }
    
    private void BatchNoVar_VariableChange(object sender, VariableChangeEventArgs e)
    {
        CheckEnteredBacthNumber();
    }
    */
    [ExportMethod]
    public void CheckEnteredBacthNumber(string CheckBatchName)
    {
        //Label MsgLabel = Project.Current.Get<Label>("UI/Screens/08Reports/BatchNumberDialog/LabelMsg");
        //Button ownerbtn = Owner.Get<Button>("Confirm");
        //DialogType BatchNumCheckResultDialog = Project.Current.Get<DialogType>("UI/Screens/08Reports/BatchNumberDialog");
        string EnteredBatchNum = CheckBatchName;

        // Validate the batch number for special characters
        if (string.IsNullOrEmpty(EnteredBatchNum))
        {
            ShowMessage("Please enter a valid batch number");
            //Button BatchMsg = Project.Current.Get<Button>("UI/Screens/08Reports/BatchReportGenerate/Button1");
            //BatchMsg.Text = "Please enter a valid batch number";
            //BatchMsg.Visible = false;
        }
        else if (ContainsSpecialCharacters(EnteredBatchNum))
        {
            ShowMessage("Special characters are not allowed in batch number");
        }
        else
        {
            var myStore = Project.Current.Get<Store>("DataStores/EDB_Batch_Report");
            string SetSQLQuery = LogicObject.GetVariable("SQLQuery").Value;
            myStore.Query(SetSQLQuery, out string[] header, out object[,] resultSet);
            int lastval = resultSet.GetLength(0) - 1;
            if (resultSet.GetLength(0) > 0)
            {
                for (int i = 0; i < resultSet.GetLength(0); i++)
                {
                    string BatchNum = resultSet[i, 0].ToString();
                    if (BatchNum.Equals(EnteredBatchNum, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMessage("Batch number already exists, please enter a new one");
                        BatchAudit BatchStart = new BatchAudit();
                        BatchStart.LogIntoAudit("Batch Start", "Batch Already Exist, Batch No. " + CheckBatchName, Session.User.BrowseName, "BatchStart");
                        goto exitloop;
                    }
                    else if (i == lastval)
                    {
                        StartBatch();
                        ShowMessage("");
                        BatchDataLogging();
                    }
                }
            }
            else
            {
                StartBatch();
                ShowMessage("");
                BatchDataLogging();
            }

        exitloop:
            ;
        }
    }

    private bool ContainsSpecialCharacters(string input)
    {
        // Regular expression to check for special characters
        return Regex.IsMatch(input, @"[^a-zA-Z0-9]");
    }

    private void StartBatch()
    {
        Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value = Project.Current.GetVariable("NetLogic/mm_dd_yyyy_clock1/datetime").Value;
        // Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value = DateTime.Now;
        Project.Current.GetVariable("Model/BatchData_Station_1/BatchStopTime_1").Value = Project.Current.GetVariable("NetLogic/mm_dd_yyyy_clock1/datetime").Value;
        Project.Current.GetVariable("Model/BatchData_Station_1/BatchRunning").Value = true;
        Project.Current.GetVariable("Model/BatchData_Station_1/TotalCounts").Value = 0;
        Project.Current.GetVariable("Model/BatchData_Station_1/RejectCounts").Value = 0;
        Project.Current.GetVariable("Model/BatchData_Station_1/GoodCounts").Value = 0;
        Project.Current.GetVariable("Model/BatchData_Station_1/%rejection").Value = 0;
        Project.Current.GetVariable("Model/BatchData_Station_1/Sampled_product").Value = 0;
        Project.Current.GetVariable("Model/Event_Message").Value = "Batch Start, Batch NO." + Project.Current.GetVariable("Model/BatchData_Station_1/BatchNumber").Value;
        Project.Current.GetVariable("Model/BatchData_Station_1/Batch_start_stop_plc").Value = true;

    }

    private void ShowMessage(string message)
    {
        //Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/Label1");
        //BatchMsg.Text = message;
        //BatchMsg.Visible = true;
        Label BatchMsg = Project.Current.Get<Label>("UI/Screens/Batch/Batch_Data_Station1/Backgroung/Rectangle1/MessageText");

        BatchMsg.Text = message;
        BatchMsg.Visible = true;
        if (delayedTask != null)
            delayedTask?.Dispose();

        delayedTask = new DelayedTask(DelayedAction, 5000, LogicObject);
        delayedTask.Start();
    }

    public void BatchDataLogging()
    {
        //string batchno = Project.Current.GetVariable("Model/Batch/Batch_Object/Batch_name").Value;
        //Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value = DateTime.Now;
        Project.Current.GetVariable("Model/BatchData_Station_1/BatchStopTime_1").Value = Project.Current.GetVariable("NetLogic/mm_dd_yyyy_clock1/datetime").Value;
        // Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value = DateTime.Now;
        Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value = Project.Current.GetVariable("NetLogic/mm_dd_yyyy_clock1/datetime").Value;
        Store myStore = Project.Current.Get<Store>("DataStores/EDB_Batch_Report");
        Table myTable = myStore.Tables.Get<Table>("BatchInfo");

        string batchno = Project.Current.GetVariable("Model/BatchData_Station_1/BatchNumber").Value;
        string batchstart = Project.Current.GetVariable("Model/BatchData_Station_1/BatchStartTime_1").Value;
        string batchstop = Project.Current.GetVariable("Model/BatchData_Station_1/BatchStopTime_1").Value;
        string prodname = Project.Current.GetVariable("Model/BatchData_Station_1/ProductName").Value;
        string Username = Project.Current.GetVariable("Model/BatchData_Station_1/Operatorname").Value;
        float totalcount = Project.Current.GetVariable("Model/BatchData_Station_1/TotalCounts").Value;
        float rejectcount = Project.Current.GetVariable("Model/BatchData_Station_1/RejectCounts").Value;
        float goodcount = Project.Current.GetVariable("Model/BatchData_Station_1/GoodCounts").Value;
        string Companyname = Project.Current.GetVariable("Model/BatchData_Station_1/CompanyName").Value;
        float Batchsize = Project.Current.GetVariable("Model/BatchData_Station_1/Batch_size").Value;
        string Recipename = Project.Current.GetVariable("Model/BatchData_Station_1/Recipename").Value;
        string EquipmentName = Project.Current.GetVariable("Model/BatchData_Station_1/EquipmentName").Value;
        string EquipmentId = Project.Current.GetVariable("Model/BatchData_Station_1/Equipmentid").Value;
        float Productsize = Project.Current.GetVariable("Model/BatchData_Station_1/Product_size").Value;
        float rejection = Project.Current.GetVariable("Model/BatchData_Station_1/%rejection").Value;
        int Duration_Hour = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Hour").Value;
        int Duration_Minute = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Minute").Value;
        int Duration_Second = Project.Current.GetVariable("Model/BatchData_Station_1/Duration_Second").Value;
        int Sampled_product = Project.Current.GetVariable("Model/BatchData_Station_1/Sampled_product").Value;

        //////////////////////////////////Machine_Parameter (Timer setting)//////////////////////////////////////////////////////
        Int32 Machine_Speed = Project.Current.GetVariable("Model/BatchData_Station_1/Machine_Speed").Value;
        float Inffed_turn_table = Project.Current.GetVariable("Model/BatchData_Station_1/Inffed_turn_table").Value;
        float Rubber_Stopper = Project.Current.GetVariable("Model/BatchData_Station_1/Rubber_Stopper").Value;
        float Infeed_Conveyor = Project.Current.GetVariable("Model/BatchData_Station_1/Infeed_Conveyor").Value;
        float Outfeed_Conveyor = Project.Current.GetVariable("Model/BatchData_Station_1/Outfeed_Conveyor").Value;
        float Fix_Conveyor = Project.Current.GetVariable("Model/BatchData_Station_1/Fix_Conveyor").Value;
        Int32 Star_wheel_holding_time = Project.Current.GetVariable("Model/BatchData_Station_1/Star_wheel_holding_time").Value;
        float Fill_servo_up_speed = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_up_speed").Value;
        float Fill_servo_down_speed = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_down_speed").Value;
        float servo_forword = Project.Current.GetVariable("Model/BatchData_Station_1/60_servo_forword").Value;
        float servo_reverse = Project.Current.GetVariable("Model/BatchData_Station_1/60_servo_reverse").Value;
        Int32 Second_speed_for_min_acc_at_infeed = Project.Current.GetVariable("Model/BatchData_Station_1/Second_speed_for_min_acc_at_infeed").Value;
        float Infeed_conveyor_speed_in_hold = Project.Current.GetVariable("Model/BatchData_Station_1/Infeed_conveyor_speed_in_hold").Value;
        float Outfeed_conveyor_speed_in_hold = Project.Current.GetVariable("Model/BatchData_Station_1/Outfeed_conveyor_speed_in_hold").Value;

        Int16 Number_of_doses = Project.Current.GetVariable("Model/BatchData_Station_1/Number_of_doses").Value;
        Int32 Auto_cycle_delay = Project.Current.GetVariable("Model/BatchData_Station_1/Auto_cycle_delay").Value;
        float Fill_servo_1_fill_position = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_1_fill_position").Value;
        float Fill_servo_2_fill_position = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_2_fill_position").Value;
        float Fill_servo_3_fill_position = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_3_fill_position").Value;
        float Fill_servo_4_fill_position = Project.Current.GetVariable("Model/BatchData_Station_1/Fill_servo_4_fill_position").Value;
        float Air_pressure_set_low_value = Project.Current.GetVariable("Model/BatchData_Station_1/Air_pressure_set_low_value").Value;
        float Air_pressure_set_high_value = Project.Current.GetVariable("Model/BatchData_Station_1/Air_pressure_set_high_value").Value;
        float rubber_stopper_vac_pre_set_low_value = Project.Current.GetVariable("Model/BatchData_Station_1/rubber_stopper_vac_pre_set_low_value").Value;
        float N2_pressure_set_low_value = Project.Current.GetVariable("Model/BatchData_Station_1/N2_pressure_set_low_value").Value;
        float N2_pressure_set_high_value = Project.Current.GetVariable("Model/BatchData_Station_1/N2_pressure_set_high_value").Value;

        float Rejection_station_vac_pre_set_low_vallue = Project.Current.GetVariable("Model/BatchData_Station_1/Rejection_station_vac_pre_set_low_vallue").Value;
        float Buffer_tank_weight_cell_set_low_value = Project.Current.GetVariable("Model/BatchData_Station_1/Buffer_tank_weight_cell_set_low_value").Value;
        float product_temp_set_low_value = Project.Current.GetVariable("Model/BatchData_Station_1/product_temp_set_low_value").Value;
        float product_temp_set_high_value = Project.Current.GetVariable("Model/BatchData_Station_1/product_temp_set_high_value").Value;
       



        object[,] rawValues = new object[1, 49]; // [Row, Column]; Column = number of columns in the BatchInfo table
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
        rawValues[0, 14] = Productsize;
        rawValues[0, 15] = rejection;
        rawValues[0, 16] = Duration_Hour;
        rawValues[0, 17] = Duration_Minute;
        rawValues[0, 18] = Duration_Second;
        rawValues[0, 19] = Sampled_product;
        rawValues[0, 20] = Machine_Speed;
        rawValues[0, 21] = Inffed_turn_table;
        rawValues[0, 22] = Rubber_Stopper;
        rawValues[0, 23] = Infeed_Conveyor;
        rawValues[0, 24] = Outfeed_Conveyor;
        rawValues[0, 25] = Fix_Conveyor;
        rawValues[0, 26] = Star_wheel_holding_time;
        rawValues[0, 27] = Fill_servo_up_speed;
        rawValues[0, 28] = Fill_servo_down_speed;
        rawValues[0, 29] = servo_forword;
        rawValues[0, 30] = servo_reverse;
        rawValues[0, 31] = Second_speed_for_min_acc_at_infeed;
        rawValues[0, 32] = Infeed_conveyor_speed_in_hold;
        rawValues[0, 33] = Outfeed_conveyor_speed_in_hold;
        rawValues[0, 34] = Number_of_doses;
        rawValues[0, 35] = Auto_cycle_delay;
        rawValues[0, 36] = Fill_servo_1_fill_position;
        rawValues[0, 37] = Fill_servo_2_fill_position;
        rawValues[0, 38] = Fill_servo_3_fill_position;
        rawValues[0, 39] = Fill_servo_4_fill_position;
        rawValues[0, 40] = Air_pressure_set_low_value;
        rawValues[0, 41] = Air_pressure_set_high_value;
        rawValues[0, 42] = rubber_stopper_vac_pre_set_low_value;
        rawValues[0, 43] = N2_pressure_set_low_value;
        rawValues[0, 44] = N2_pressure_set_high_value;
        rawValues[0, 45] = Rejection_station_vac_pre_set_low_vallue;
        rawValues[0, 46] = Buffer_tank_weight_cell_set_low_value;
        rawValues[0, 47] = product_temp_set_low_value;
        rawValues[0, 48] = product_temp_set_high_value;

        string[] columns = new string[49] { "LocalTimeStamp", "BatchNumber", "BatchStartTime", "BatchStopTime", "ProductName", "Username", "TotalCounts", "RejectCounts", "GoodCounts", "BatchSize", "Recipename", "Companyname", "EquipmentName", "EquipmentId", "Productsize", "rejection", "Duration_Hour", "Duration_Minute", "Duration_Second", "Sampled_Product", "Machine_Speed", "Inffed_turn_table", "Rubber_Stopper", "Infeed_Conveyor", "Outfeed_Conveyor", "Fix_Conveyor", "Star_wheel_holding_time", "Fill_servo_up_speed", "Fill_servo_down_speed", "servo_forword", "servo_reverse", "Second_speed_for_min_acc_at_infeed", "Infeed_conveyor_speed_in_hold", "Outfeed_conveyor_speed_in_hold", "Number_of_doses", "Auto_cycle_delay", "Fill_servo_1_fill_position", "Fill_servo_2_fill_position", "Fill_servo_3_fill_position", "Fill_servo_4_fill_position", "Air_pressure_set_low_value", "Air_pressure_set_high_value", "rubber_stopper_vac_pre_set_low_value", "N2_pressure_set_low_value", "N2_pressure_set_high_value", "Rejection_station_vac_pre_set_low_vallue", "Buffer_tank_weight_cell_set_low_value", "product_temp_set_low_value", "product_temp_set_high_value" };
        myTable.Insert(columns, rawValues);

        BatchAudit BatchStart = new BatchAudit();
        BatchStart.LogIntoAudit("Batch Start", "Batch Start, Batch No. " + batchno, Session.User.BrowseName, "BatchStart");
    }

    private void DelayedAction(DelayedTask task)
    {
        if (task.IsCancellationRequested)
            return;

        //Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/Label1");
        //BatchMsg.Text = "";
        //BatchMsg.Visible = false;
        Label BatchMsg = Project.Current.Get<Label>("UI/Screens/Batch/Batch_Data_Station1/Backgroung/Rectangle1/MessageText");
        BatchMsg.Text = "";
        BatchMsg.Visible = false;
        delayedTask?.Dispose();
    }

    private DelayedTask delayedTask;
    //private IUAVariable BatchNoVar;
}

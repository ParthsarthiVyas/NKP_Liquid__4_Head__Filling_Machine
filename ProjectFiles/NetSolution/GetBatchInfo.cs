#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.Store;
using FTOptix.Core;
using FTOptix.WebUI;
using FTOptix.Alarm;
using FTOptix.RAEtherNetIP;
#endregion

public class GetBatchInfo : BaseNetLogic
{
    public override void Start()
    {
        SelBatchNoVar = Project.Current.GetVariable("Model/Report/Batch_report/BatchNumber");
        SelBatchNoVar.VariableChange += SelBatchNoVar_VariableChange;
        
        LoadBatchInfo();
    }

    private void SelBatchNoVar_VariableChange(object sender, VariableChangeEventArgs e)
    {
        LoadBatchInfo();
    }

    private void LoadBatchInfo()
    {
        var myStore = Project.Current.Get<Store>("DataStores/EDB_Batch_Report");
        string batchsquery = Project.Current.GetVariable("Model/Report/Batch_report/Batch_Query").Value;
        //Log.Warning(batchsquery);
        myStore.Query(batchsquery, out string[] header, out object[,] resultSet);
        //int LastVal = resultSet.GetLength(0) - 1;
        //Log.Warning("Number Results = " + resultSet.GetLength(0));

        string batchno = "";
        string batchstart ="";
        string batchstop = "";
        string prodname =   "";
        string Username = "";
        float totalcount =  0;
        float rejectcount = 0;
        float goodcount = 0;
        string Companyname = "";
        float Batchsize = 0;
        string Recipename = "";
        string EquipmentName = "";
        string EquipmentId = "";
        float Productsize = 0;
        float rejection = 0;
        int duration_hour = 0;
        int duration_second = 0;
        int duration_minute = 0;
        int Sampled_product = 0;

        //////////////////////////////////Set_Parameter//////////////////////////////////////////////////////
        int Machine_Speed = 0;
        float Inffed_turn_table = 0;
        float Rubber_Stopper = 0;
        float Infeed_Conveyor = 0;
        float Outfeed_Conveyor = 0;
        float Fix_Conveyor = 0;
        float Star_wheel_holding_time = 0;
        float Fill_servo_up_speed = 0;
        float Fill_servo_down_speed = 0;
        float servo_forword = 0;
        float servo_reverse = 0;
        int Second_speed_for_min_acc_at_infeed = 0;
        float Infeed_conveyor_speed_in_hold = 0;
        float Outfeed_conveyor_speed_in_hold = 0;
        int Number_of_doses = 0;
        int Auto_cycle_delay = 0;
        float Fill_servo_1_fill_position = 0;
        float Fill_servo_2_fill_position = 0;
        float Fill_servo_3_fill_position = 0;
        float Fill_servo_4_fill_position = 0;
        float Air_pressure_set_low_value = 0;
        float Air_pressure_set_high_value = 0;
        float rubber_stopper_vac_pre_set_low_value = 0;
        float N2_pressure_set_low_value = 0;
        float N2_pressure_set_high_value = 0;
        float Rejection_station_vac_pre_set_low_vallue = 0;
        float Buffer_tank_weight_cell_set_low_value = 0;
        float product_temp_set_low_value = 0;
        float product_temp_set_high_value = 0;
       




        if (resultSet.GetLength(0) > 0)
        {
            
            batchno = resultSet[0, 0].ToString();
            batchstart = resultSet[0, 1].ToString();
            batchstop = resultSet[0, 2].ToString();
            prodname  = resultSet[0, 3].ToString();
            Username = resultSet[0, 4].ToString();
            float.TryParse(resultSet[0, 5].ToString(), out totalcount);
            float.TryParse(resultSet[0, 6].ToString(), out rejectcount);
            float.TryParse(resultSet[0, 7].ToString(), out goodcount);
            float.TryParse(resultSet[0, 8].ToString(), out Batchsize);
            Recipename = resultSet[0, 9].ToString();
            Companyname = resultSet[0, 10].ToString();
            EquipmentName = resultSet[0, 11].ToString();
            EquipmentId = resultSet[0, 12].ToString();
            float.TryParse(resultSet[0, 13]?.ToString(), out Productsize);
            float.TryParse(resultSet[0, 14]?.ToString(), out rejection);
            duration_hour  = int.Parse(resultSet[0, 15].ToString());
            duration_minute = int.Parse(resultSet[0, 16].ToString());
            duration_second = int.Parse(resultSet[0, 17].ToString());
            Sampled_product = int.Parse(resultSet[0, 18].ToString());
            Machine_Speed = int.Parse(resultSet[0, 19]. ToString());
            float.TryParse(resultSet[0, 20]?.ToString(), out Inffed_turn_table);
            float.TryParse(resultSet[0, 21]?.ToString(), out Rubber_Stopper);
            float.TryParse(resultSet[0, 22]?.ToString(), out Infeed_Conveyor);
            float.TryParse(resultSet[0, 23]?.ToString(), out Outfeed_Conveyor);
            float.TryParse(resultSet[0, 24]?.ToString(), out Fix_Conveyor);
            float.TryParse(resultSet[0, 25]?.ToString(), out Star_wheel_holding_time);
            float.TryParse(resultSet[0, 26]?.ToString(), out Fill_servo_up_speed);
            float.TryParse(resultSet[0, 27]?.ToString(), out Fill_servo_down_speed);
            float.TryParse(resultSet[0, 28]?.ToString(), out servo_forword);
            float.TryParse(resultSet[0, 29]?.ToString(), out servo_reverse);
            Second_speed_for_min_acc_at_infeed = int.Parse(resultSet[0, 30].ToString()); 
            float.TryParse(resultSet[0, 31]?.ToString(), out Infeed_conveyor_speed_in_hold);
            float.TryParse(resultSet[0, 32]?.ToString(), out Outfeed_conveyor_speed_in_hold);
            Number_of_doses = int.Parse(resultSet[0, 33].ToString());
            Auto_cycle_delay = int.Parse(resultSet[0, 34].ToString());
            float.TryParse(resultSet[0, 35]?.ToString(), out Fill_servo_1_fill_position);
            float.TryParse(resultSet[0, 36]?.ToString(), out Fill_servo_2_fill_position);
            float.TryParse(resultSet[0, 37]?.ToString(), out Fill_servo_3_fill_position);
            float.TryParse(resultSet[0, 39]?.ToString(), out Fill_servo_4_fill_position);
            float.TryParse(resultSet[0, 40]?.ToString(), out Air_pressure_set_low_value);
            float.TryParse(resultSet[0, 41]?.ToString(), out Air_pressure_set_high_value);
            float.TryParse(resultSet[0, 42]?.ToString(), out rubber_stopper_vac_pre_set_low_value);
            float.TryParse(resultSet[0, 43]?.ToString(), out N2_pressure_set_low_value);
            float.TryParse(resultSet[0, 44]?.ToString(), out N2_pressure_set_high_value);
            float.TryParse(resultSet[0, 45]?.ToString(), out Rejection_station_vac_pre_set_low_vallue);
            float.TryParse(resultSet[0, 46]?.ToString(), out Buffer_tank_weight_cell_set_low_value);
            float.TryParse(resultSet[0, 47]?.ToString(), out product_temp_set_low_value);
            float.TryParse(resultSet[0, 48]?.ToString(), out product_temp_set_high_value);
        }

         Project.Current.GetVariable("Model/Report/Batch_report/BatchNumber").Value = batchno;
         Project.Current.GetVariable("Model/Report/Batch_report/ProductName").Value = prodname;
         Project.Current.GetVariable("Model/Report/Batch_report/Operatorname").Value = Username;
         Project.Current.GetVariable("Model/Report/Batch_report/TotalCounts").Value = totalcount;
         Project.Current.GetVariable("Model/Report/Batch_report/RejectCounts").Value = rejectcount;
         Project.Current.GetVariable("Model/Report/Batch_report/GoodCounts").Value = goodcount;
         Project.Current.GetVariable("Model/Report/Batch_report/CompanyName").Value = Companyname;
         Project.Current.GetVariable("Model/Report/Batch_report/Batch_size").Value = Batchsize;
         Project.Current.GetVariable("Model/Report/Batch_report/Recipename").Value = Recipename;
         Project.Current.GetVariable("Model/Report/Batch_report/EquipmentName").Value = EquipmentName;
         Project.Current.GetVariable("Model/Report/Batch_report/Equipmentid").Value = EquipmentId;
         Project.Current.GetVariable("Model/Report/Batch_report/BatchStartTime_1").Value = batchstart;
         Project.Current.GetVariable("Model/Report/Batch_report/BatchStopTime_1").Value = batchstop;
         Project.Current.GetVariable("Model/Report/Batch_report/Product_size").Value = Productsize;
         Project.Current.GetVariable("Model/Report/Batch_report/%rejection").Value = rejection;

         Project.Current.GetVariable("Model/Report/Batch_report/Duration_Hour").Value = duration_hour;
         Project.Current.GetVariable("Model/Report/Batch_report/Duration_Minute").Value = duration_minute;
         Project.Current.GetVariable("Model/Report/Batch_report/Duration_Second").Value = duration_second;
         Project.Current.GetVariable("Model/Report/Batch_report/Sampled_product").Value = Sampled_product;
         Project.Current.GetVariable("Model/Report/Batch_report/Machine_Speed").Value = Machine_Speed;
         Project.Current.GetVariable("Model/Report/Batch_report/Inffed_turn_table").Value = Inffed_turn_table;
         Project.Current.GetVariable("Model/Report/Batch_report/Rubber_Stopper").Value = Rubber_Stopper;
         Project.Current.GetVariable("Model/Report/Batch_report/Infeed_Conveyor").Value = Infeed_Conveyor;
         Project.Current.GetVariable("Model/Report/Batch_report/Outfeed_Conveyor").Value = Outfeed_Conveyor;
         Project.Current.GetVariable("Model/Report/Batch_report/Fix_Conveyor").Value = Fix_Conveyor;
         Project.Current.GetVariable("Model/Report/Batch_report/Star_wheel_holding_time").Value = Star_wheel_holding_time;
         Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_up_speed").Value = Fill_servo_up_speed;
         Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_down_speed").Value = Fill_servo_down_speed;
         Project.Current.GetVariable("Model/Report/Batch_report/60_servo_forword").Value = servo_forword;

         Project.Current.GetVariable("Model/Report/Batch_report/60_servo_reverse").Value = servo_reverse;
         Project.Current.GetVariable("Model/Report/Batch_report/Second_speed_for_min_acc_at_infeed").Value = Second_speed_for_min_acc_at_infeed;
         Project.Current.GetVariable("Model/Report/Batch_report/Infeed_conveyor_speed_in_hold").Value = Infeed_conveyor_speed_in_hold;
         Project.Current.GetVariable("Model/Report/Batch_report/Outfeed_conveyor_speed_in_hold").Value = Outfeed_conveyor_speed_in_hold;
         Project.Current.GetVariable("Model/Report/Batch_report/Number_of_doses").Value = Number_of_doses;
         Project.Current.GetVariable("Model/Report/Batch_report/Auto_cycle_delay").Value = Auto_cycle_delay;
         Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_1_fill_position").Value = Fill_servo_1_fill_position;
         Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_2_fill_position").Value = Fill_servo_2_fill_position;
        Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_3_fill_position").Value = Fill_servo_3_fill_position;
        Project.Current.GetVariable("Model/Report/Batch_report/Fill_servo_4_fill_position").Value = Fill_servo_4_fill_position;
        Project.Current.GetVariable("Model/Report/Batch_report/Air_pressure_set_low_value").Value = Air_pressure_set_low_value;
        Project.Current.GetVariable("Model/Report/Batch_report/Air_pressure_set_high_value").Value = Air_pressure_set_high_value;
        Project.Current.GetVariable("Model/Report/Batch_report/rubber_stopper_vac_pre_set_low_value").Value = rubber_stopper_vac_pre_set_low_value;
        Project.Current.GetVariable("Model/Report/Batch_report/N2_pressure_set_low_value").Value = N2_pressure_set_low_value;
        Project.Current.GetVariable("Model/Report/Batch_report/N2_pressure_set_high_value").Value = N2_pressure_set_high_value;
        Project.Current.GetVariable("Model/Report/Batch_report/Rejection_station_vac_pre_set_low_vallue").Value = Rejection_station_vac_pre_set_low_vallue;
        Project.Current.GetVariable("Model/Report/Batch_report/Buffer_tank_weight_cell_set_low_value").Value = Buffer_tank_weight_cell_set_low_value;
        Project.Current.GetVariable("Model/Report/Batch_report/product_temp_set_low_value").Value = product_temp_set_low_value;
        Project.Current.GetVariable("Model/Report/Batch_report/product_temp_set_high_value").Value = product_temp_set_high_value;
  

    }

    private IUAVariable SelBatchNoVar;
}


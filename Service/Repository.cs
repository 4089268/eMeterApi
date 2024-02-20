using System;
using System.Data;
using System.Data.SqlClient;
using eMeterApi;

namespace eMeterApi.Service
{
    public class Repository
    {

        private readonly string connectionString;

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertData( MeterData meterData)
        {

            // Create INSERT command with parameters
            string insertQuery = @"
                INSERT INTO MeterDataTable (StartCode, MeterType, MeterAddress, ControlCode, DataLength, DataId, Ser, CfUnit, 
                CummulativeFlow, CfUnitSetDay, DayliCumulativeAmount, ReverseCfUnit, ReverseCumulativeFlow, FlowRateUnit, FlowRate, 
                Temperature, DevDate, DevTime, Status, Valve, Battery, Battery1, Empty, ReverseFlow, OverRange, WaterTemp, 
                EEAlarm, Reserved, CheckSum, EndMark, RegistrationDate) 
                VALUES (@StartCode, @MeterType, @MeterAddress, @ControlCode, @DataLength, @DataId, @Ser, @CfUnit, @CummulativeFlow, 
                @CfUnitSetDay, @DayliCumulativeAmount, @ReverseCfUnit, @ReverseCumulativeFlow, @FlowRateUnit, @FlowRate, 
                @Temperature, @DevDate, @DevTime, @Status, @Valve, @Battery, @Battery1, @Empty, @ReverseFlow, @OverRange, 
                @WaterTemp, @EEAlarm, @Reserved, @CheckSum, @EndMark, @RegistrationDate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Assign parameter values
                    command.Parameters.AddWithValue("@StartCode", meterData.StartCode);
                    command.Parameters.AddWithValue("@MeterType", meterData.MeterType);
                    command.Parameters.AddWithValue("@MeterAddress", meterData.MeterAddress);
                    command.Parameters.AddWithValue("@ControlCode", meterData.ControlCode);
                    command.Parameters.AddWithValue("@DataLength",meterData.DataLenght);
                    command.Parameters.AddWithValue("@DataId", meterData.DataId);
                    command.Parameters.AddWithValue("@Ser", meterData.Ser);
                    command.Parameters.AddWithValue("@CfUnit", meterData.CfUnit);
                    command.Parameters.AddWithValue("@CummulativeFlow", meterData.CummulativeFlow);
                    command.Parameters.AddWithValue("@CfUnitSetDay", meterData.CfUnitSetDay);
                    command.Parameters.AddWithValue("@DayliCumulativeAmount", meterData.DayliCumulativeAmount);
                    command.Parameters.AddWithValue("@ReverseCfUnit", meterData.ReverseCfUnit);
                    command.Parameters.AddWithValue("@ReverseCumulativeFlow", meterData.ReverseCumulativeFlow);
                    command.Parameters.AddWithValue("@FlowRateUnit", meterData.FlowRateUnit);
                    command.Parameters.AddWithValue("@FlowRate", meterData.FlowRate);
                    command.Parameters.AddWithValue("@Temperature", meterData.Temperature);
                    command.Parameters.AddWithValue("@DevDate", meterData.DevDate);
                    command.Parameters.AddWithValue("@DevTime", meterData.DevTime);
                    command.Parameters.AddWithValue("@Status", meterData.Status);
                    command.Parameters.AddWithValue("@Valve", meterData.Valve);
                    command.Parameters.AddWithValue("@Battery", meterData.Battery);
                    command.Parameters.AddWithValue("@Battery1", meterData.Battery1);
                    command.Parameters.AddWithValue("@Empty", meterData.Empty);
                    command.Parameters.AddWithValue("@ReverseFlow", meterData.ReverseFlow);
                    command.Parameters.AddWithValue("@OverRange", meterData.OverRange);
                    command.Parameters.AddWithValue("@WaterTemp", meterData.WaterTemp);
                    command.Parameters.AddWithValue("@EEAlarm", meterData.EEAlarm);
                    command.Parameters.AddWithValue("@Reserved", meterData.Reserved);
                    command.Parameters.AddWithValue("@CheckSum", meterData.CheckSume);
                    command.Parameters.AddWithValue("@EndMark",meterData. EndMark);
                    command.Parameters.AddWithValue("@RegistrationDate", DateTime.Now); // Set the current date and time

                    // Open the connection and execute the command
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        
    }
}
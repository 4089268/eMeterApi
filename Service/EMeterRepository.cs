using System;
using System.Data;
using System.Data.SqlClient;
using eMeterApi;

namespace eMeterApi.Service
{
    public class EMeterRepository
    {

        private readonly string connectionString;

        public EMeterRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertData(MeterData meterData, string? groupId)
        {
            // Create INSERT command with parameters
            string insertQuery = @"
                INSERT INTO MeterDataTable (StartCode, MeterType, MeterAddress, ControlCode, DataLength, DataId, Ser, CfUnit, 
                CummulativeFlow, CfUnitSetDay, DayliCumulativeAmount, ReverseCfUnit, ReverseCumulativeFlow, FlowRateUnit, FlowRate, 
                Temperature, DevDate, DevTime, Status, Valve, Battery, Battery1, Empty, ReverseFlow, OverRange, WaterTemp, 
                EEAlarm, Reserved, CheckSum, EndMark, RegistrationDate, GroupId) 
                VALUES (@StartCode, @MeterType, @MeterAddress, @ControlCode, @DataLength, @DataId, @Ser, @CfUnit, @CummulativeFlow, 
                @CfUnitSetDay, @DayliCumulativeAmount, @ReverseCfUnit, @ReverseCumulativeFlow, @FlowRateUnit, @FlowRate, 
                @Temperature, @DevDate, @DevTime, @Status, @Valve, @Battery, @Battery1, @Empty, @ReverseFlow, @OverRange, 
                @WaterTemp, @EEAlarm, @Reserved, @CheckSum, @EndMark, @RegistrationDate, @GroupId)";

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
                    command.Parameters.AddWithValue("@GroupId", groupId); 

                    // Open the connection and execute the command
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        
        public IEnumerable<MeterData> GetAll()
        {    
            var dataList = new List<MeterData>();

            string selectQuery = "SELECT * FROM MeterDataTable";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Read the data and create MeterData objects
                        while (reader.Read())
                        {
                            MeterData data = new MeterData
                            {
                                StartCode = reader["StartCode"].ToString() ?? "",
                                MeterType = reader["MeterType"].ToString() ?? "",
                                MeterAddress = reader["MeterAddress"].ToString() ?? "",
                                ControlCode = reader["ControlCode"].ToString() ?? "", 
                                DataLenght = Convert.ToInt32(reader["DataLength"]),
                                DataId = reader["DataId"].ToString() ?? "",
                                Ser = reader["Ser"].ToString() ?? "",
                                CfUnit = reader["CfUnit"].ToString() ?? "",
                                CummulativeFlow = Convert.ToDouble(reader["CummulativeFlow"]),
                                CfUnitSetDay = reader["CfUnitSetDay"].ToString() ?? "",
                                DayliCumulativeAmount = Convert.ToDouble(reader["DayliCumulativeAmount"]),
                                ReverseCfUnit = reader["ReverseCfUnit"].ToString() ?? "",
                                ReverseCumulativeFlow = Convert.ToDouble(reader["ReverseCumulativeFlow"]),
                                FlowRateUnit = reader["FlowRateUnit"].ToString() ?? "",
                                FlowRate = Convert.ToDouble(reader["FlowRate"]),
                                Temperature = Convert.ToDouble(reader["Temperature"]),
                                DevDate = reader["DevDate"].ToString() ?? "",
                                DevTime = reader["DevTime"].ToString() ?? "",
                                Status = reader["Status"].ToString() ?? "",
                                Valve = reader["Valve"].ToString() ?? "",
                                Battery = reader["Battery"].ToString() ?? "",
                                Battery1 = reader["Battery1"].ToString() ?? "",
                                Empty = reader["Empty"].ToString() ?? "",
                                ReverseFlow = reader["ReverseFlow"].ToString() ?? "",
                                OverRange = reader["OverRange"].ToString() ?? "",
                                WaterTemp = reader["WaterTemp"].ToString() ?? "",
                                EEAlarm = reader["EEAlarm"].ToString() ?? "",
                                Reserved = reader["Reserved"].ToString() ?? "",
                                CheckSume = reader["CheckSum"].ToString() ?? "",
                                EndMark = reader["EndMark"].ToString() ?? ""
                            };

                            // Add the MeterData object to the list
                            dataList.Add(data);
                        }
                    }
                }
                connection.Close();
            }

            return dataList;
        }

    }
}
using System;
using System.Text;
using eMeterApi;

namespace eMeterApi.Service
{

    public class ProcessBuffer
    {
        public static MeterData ProcessData( string stringBuffer )
        {
            byte[] buffer = Convert.FromHexString( stringBuffer );

            string start_code = BitConverter.ToString(buffer, 0, 1).Replace("-", "");
            string meter_type = BitConverter.ToString(buffer, 1, 1).Replace("-", "");
            
            
            string meter_addr = BitConverter.ToString(buffer, 8, 1).Replace("-", "") +
                                BitConverter.ToString(buffer, 6, 2).Replace("-", "") +
                                BitConverter.ToString(buffer, 2, 4).Replace("-", "");

            // string meter_addr = BitConverter.ToString(buffer, 8, 1).Replace("-", "") +
            //                     BitConverter.ToString(buffer, 6, 2).SwapBytes(0, 2).Replace("-", "") +
            //                     BitConverter.ToString(buffer, 2, 4).SwapBytes(0, 3).Replace("-", "");


            string control_code = BitConverter.ToString(buffer, 9, 1).Replace("-", "");
            int data_length = buffer[10];
            string data_id = BitConverter.ToString(buffer, 11, 2).Replace("-", "");
            string serial = BitConverter.ToString(buffer, 13, 1).Replace("-", "");
            string cf_unit = BitConverter.ToString(buffer, 14, 1).Replace("-", "");
            double cumulative_flow = BitConverter.ToInt32(buffer, 15).SwapEndian() / 100.0;
            string cf_unit_set_day = BitConverter.ToString(buffer, 19, 1).Replace("-", "");
            double daily_cumulative_amount = BitConverter.ToInt32(buffer, 20).SwapEndian() / 100.0;
            string reverse_cf_unit = BitConverter.ToString(buffer, 24, 1).Replace("-", "");
            double reverse_cumulative_flow = BitConverter.ToInt32(buffer, 25).SwapEndian() / 100.0;
            string flow_rate_unit = BitConverter.ToString(buffer, 29, 1).Replace("-", "");
            double flow_rate = BitConverter.ToInt32(buffer, 30).SwapEndian() / 100.0;
            double temperature = Convert.ToInt32(BitConverter.ToString(buffer, 35, 1) +
                                                BitConverter.ToString(buffer, 34, 1), 16) / 100.0;
            
            string dev_date = BitConverter.ToString(buffer, 40, 1) + "-" +
                            BitConverter.ToString(buffer, 41, 1) + "-" +
                            BitConverter.ToString(buffer, 42, 2).Replace("-", "");
            // string dev_date = BitConverter.ToString(buffer, 40, 1) + "-" +
            //                   BitConverter.ToString(buffer, 41, 1) + "-" +
            //                   BitConverter.ToString(buffer, 42, 2).SwapBytes(0, 1).Replace("-", "");


            string time = BitConverter.ToString(buffer, 39, 1) + ":" +
                        BitConverter.ToString(buffer, 38, 1) + ":" +
                        BitConverter.ToString(buffer, 37, 1);
            string alarm = Convert.ToString(BitConverter.ToInt16(buffer, 44), 2).PadLeft(16, '0');
            string apertura = alarm.Substring(6, 2) == "00" ? "open" :
                            alarm.Substring(6, 2) == "01" ? "closed" :
                            alarm.Substring(6, 2) == "11" ? "anormal" : "otro";
            string bateria = alarm[5] == '0' ? "normal" : "low battery";
            string bateria_1 = alarm[15] == '0' ? "normal" : "alarma";
            string empty = alarm[14] == '0' ? "normal" : "alarma";
            string reverse_flow = alarm[13] == '0' ? "normal" : "alarma";
            string over_range = alarm[12] == '0' ? "normal" : "alarma";
            string water_temp = alarm[11] == '0' ? "normal" : "alarma";
            string ee_alarm = alarm[10] == '0' ? "normal" : "alarma";
            string reserved = BitConverter.ToString(buffer, 46, 1).Replace("-", "");
            string check_sume = BitConverter.ToString(buffer, 47, 1).Replace("-", "");
            string end_mark = BitConverter.ToString(buffer, 48, 1).Replace("-", "");

            var _dataModel = new MeterData();
            _dataModel.StartCode = start_code;
            _dataModel.MeterType = meter_type;
            _dataModel.MeterAddress = meter_addr;
            _dataModel.ControlCode = control_code;
            _dataModel.DataLenght = data_length;
            _dataModel.DataId = data_id;
            _dataModel.Ser = serial;
            _dataModel.CfUnit = cf_unit;
            _dataModel.CummulativeFlow = cumulative_flow;
            _dataModel.CfUnitSetDay = cf_unit_set_day;
            _dataModel.DayliCumulativeAmount = daily_cumulative_amount;
            _dataModel.ReverseCfUnit = reverse_cf_unit;
            _dataModel.ReverseCumulativeFlow = reverse_cumulative_flow;
            _dataModel.FlowRateUnit = flow_rate_unit;
            _dataModel.FlowRate = flow_rate;
            _dataModel.Temperature = temperature;
            _dataModel.DevDate = dev_date;
            _dataModel.DevTime = time;
            _dataModel.Status = alarm;
            _dataModel.Valve = apertura;
            _dataModel.Battery = bateria;
            _dataModel.Battery1 = bateria_1;
            _dataModel.Empty = empty;
            _dataModel.ReverseFlow = reverse_flow;
            _dataModel.OverRange = over_range;
            _dataModel.WaterTemp = water_temp;
            _dataModel.EEAlarm = ee_alarm;
            _dataModel.Reserved = reserved;
            _dataModel.CheckSume = check_sume;
            _dataModel.EndMark = end_mark;

            return _dataModel;
        }
    }

    public static class ExtensionMethods
    {
        public static int SwapEndian(this int value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(value).SwapBytes(0, 3), 0);
        }

        public static byte[] SwapBytes(this byte[] value, int index1, int index2)
        {
            byte temp = value[index1];
            value[index1] = value[index2];
            value[index2] = temp;
            return value;
        }

        public static byte[] ConvertFromHexString(this string hexString)
        {
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return buffer;
        }
    }

}
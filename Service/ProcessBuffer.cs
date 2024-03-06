using System;
using System.Buffers;
using System.Text;
using eMeterApi;

namespace eMeterApi.Service
{

    public class ProcessBuffer
    {
        public static MeterData ProcessData( string stringBuffer )
        {

            string[] buffer = SplitString( stringBuffer );

            string start_code = buffer[0];

            string meter_type = buffer[1];

            string meter_addr = buffer[8] +
                                buffer[7] +
                                buffer[6] +
                                buffer[5] +
                                buffer[4] +
                                buffer[3] +
                                buffer[2];
            
            string control_code = buffer[9];

            int data_length = Convert.ToInt32( buffer[10], 16 );

            string data_id = buffer[11] + buffer[12];

            string serial = buffer[13];

            string cf_unit = buffer[14];

            double cumulative_flow = Convert.ToDouble( buffer[18] + buffer[17] + buffer[16] + buffer[15] ) / 100;

            string cf_unit_set_day = buffer[19];

            double daily_cumulative_amount = Convert.ToDouble( buffer[23] + buffer[22] + buffer[21] + buffer[20]) / 100;

            string reverse_cf_unit = buffer[24];

            double reverse_cumulative_flow = Convert.ToDouble( buffer[28] + buffer[27] + buffer[26] + buffer[25] ) / 100;
            
            string flow_rate_unit = buffer[29];

            double flow_rate = Convert.ToDouble( buffer[33] + buffer[32] + buffer[31] + buffer[30] ) / 100;

            double temperature = Convert.ToDouble( buffer[35] + buffer[34] ) / 100;
            
            string dev_date = $"{buffer[40]}-{buffer[41]}-{buffer[43]}{buffer[42]}";
            
            string time = $"{buffer[39]}:{buffer[38]}:{buffer[37]}";


            var _preAlarm =  Convert.ToInt32( buffer[44] + buffer[45], 16);
            string alarm = Convert.ToString( _preAlarm, 2).PadLeft(16, '0');

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
            
            string reserved = buffer[46];

            string check_sume = buffer[47];

            string end_mark = buffer[48];

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

        private static string[] SplitString( string input)
        {
            int chunkSize = 2;
            var chunks = new List<string>();
            for( int i = 0; i < input.Length; i+= chunkSize )
            {
                if( i + chunkSize <= input.Length)
                {
                    string chunk = input.Substring(i, chunkSize);
                    chunks.Add( chunk );    
                }
            }
            return chunks.ToArray();
        }
    }

}
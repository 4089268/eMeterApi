using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eMeter.Helpers;
using eMeter.Models;
using eMeterApi.Data;
using eMeterApi.Entities;
using eMeterApi.Models;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace eMeter.Service
{
    public class DeviceService
    {

        private readonly EMeterContext eMeterContext;
        private readonly ILogger<DeviceService> logger;
        private readonly SaasSettings saasSettings;

        public DeviceService( EMeterContext eMeterContext, ILogger<DeviceService> logger, IOptions<SaasSettings> OptionsSaasSettings){
            this.eMeterContext = eMeterContext;
            this.logger = logger;
            this.saasSettings = OptionsSaasSettings.Value;
        }

        public IEnumerable<Device> GetDevices(out int totalItems, int chunk = 25, int page = 0, IEnumerable<string>? groupsId = null, IEnumerable<string>? batteryStatus = null, IEnumerable<string>? valveStatus = null, string? search = null )
        {

            var devicesQuery = eMeterContext.Devices.AsQueryable();

            if(groupsId != null)
            {
                devicesQuery = devicesQuery.Where( device => 
                    groupsId.Select(groupId => 
                        groupId.ToLower()
                    ).Contains(device.GroupId.ToLower())
                );
            }

            if(batteryStatus != null)
            {
                devicesQuery = devicesQuery.Where( device => 
                    batteryStatus.Select(item => 
                        item.ToLower()
                    ).Contains(device.Battery.ToLower())
                );
            }

            if(valveStatus != null)
            {
                devicesQuery = devicesQuery.Where( device => 
                    valveStatus.Select(item => 
                        item.ToLower()
                    ).Contains(device.Valve.ToLower())
                );
            }
            
            if( !String.IsNullOrEmpty(search)){
                devicesQuery = devicesQuery.Where( device => device.MeterAddress.Contains(search));
            }

            totalItems = devicesQuery.Count();
            
            if( chunk == 0){
                return devicesQuery.ToList();
            }

            return devicesQuery.Skip(chunk * page )
                .Take(chunk)
                .ToList();
        }

        public DeviceDetails? GetDeviceInfo(string deviceAddress)
        {
            // Get device info
            var device = eMeterContext.Devices.Where( item => item.MeterAddress == deviceAddress).FirstOrDefault();
            if(device == null){
                logger.LogWarning($"Device addres {deviceAddress} was not found in the database at DeviceService.GetDeviceInfo");
                return null;
            }

            // Get las measurements
            var measurementsRaw = eMeterContext.MeterDataTables
                .OrderByDescending( item => item.RegistrationDate)
                .Where( item => item.MeterAddress == deviceAddress )
                .Take(25)
                .ToList();
            // Process data
            List<Measurement> measurements = measurementsRaw.Select( item => MeasurementAdapter.FromMeterDataTable(item) ).ToList();

            // Prepare response
            return new DeviceDetails {
                DeviceAddress = deviceAddress,
                Device = device,
                Measurement = measurements
            };
        }

        public IEnumerable<KeyValuePair<string, int>> GetMeasurementChart( string deviceAddress ){

            // Create list of the last 15 days
            var currentData = DateTime.Now;
            List<DateTime> lastDays = new List<DateTime>();
            for( int i = 0; i<15; i++){
                DateTime previousDate = currentData.AddDays(-i);
                lastDays.Add( previousDate );
            }
            lastDays = lastDays.OrderBy(item => item.Ticks).ToList();

            // Get data by day
            List<KeyValuePair<string, int>> charValues = new List<KeyValuePair<string, int>>();
            foreach( var date in lastDays){
                var total = eMeterContext.MeterDataTables
                    .Where( item => item.MeterAddress == deviceAddress)
                    .Where( item => item.RegistrationDate!.Value.Date == date.Date )
                    .Count();
                
                charValues.Add( new KeyValuePair<string, int>( date.ToString("MMM dd").ToUpper(), total ));
            }

            return charValues;

        }
    
        public  async Task<bool> CloseValve( string deviceId){
            // TODO: Injet the httpclient

            // Create a new instance of HttpClientHandler
            var credentials = $"{saasSettings.User}:{saasSettings.Password}";
            
            // Encode the credentials string in Base64
            var credentialsBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));


            var request = new HttpRequestMessage( HttpMethod.Post, $"{saasSettings.Endpoint}/rest/nodes/{deviceId}/payloads/dl?port=2&confirmed=true&mode=fail_on_busy&data_format=hex");
            var content = new StringContent("{\n\"data\":\"6810aaaaaaaaaaaaaa0404a01700997616\"\n}", null, "application/json");

            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialsBase64 );

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            Console.WriteLine("(-) Close valve:" + response.StatusCode );
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> OpenValve( string deviceId){

            // Create a new instance of HttpClientHandler
            var credentials = $"{saasSettings.User}:{saasSettings.Password}";
            
            // Encode the credentials string in Base64
            var credentialsBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));


            var request = new HttpRequestMessage( HttpMethod.Post, $"{saasSettings.Endpoint}/rest/nodes/{deviceId}/payloads/dl?port=2&confirmed=true&mode=fail_on_busy&data_format=hex");
            var content = new StringContent("{\n\"data\":\"6810aaaaaaaaaaaaaa0404a01700553216\"\n}", null, "application/json");

            request.Content = content;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentialsBase64 );

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            Console.WriteLine("(-) Open valve:" + response.StatusCode );
            return response.IsSuccessStatusCode;

        }

    }

}
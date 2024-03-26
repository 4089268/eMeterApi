using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeter.Helpers;
using eMeter.Models;
using eMeterApi.Data;
using eMeterApi.Models;

namespace eMeter.Service
{
    public class MeasurementService
    {

        private readonly EMeterContext eMeterContext;
        private readonly ILogger<MeasurementService> logger;

        public MeasurementService( EMeterContext eMeterContext, ILogger<MeasurementService> logger){
            this.eMeterContext = eMeterContext;
            this.logger = logger;
        }
        

        public IEnumerable<Measurement> GetMeasurement( DateTime from, DateTime to, out int totalItems, int chunk = 0, int page = 0, string? deviceAddress = null)
        {
            
            var query = this.eMeterContext.MeterDataTables
                .OrderByDescending( e => e.RegistrationDate)
                .Where(  item => from.Date <= item.RegistrationDate!.Value.Date && item.RegistrationDate!.Value.Date <= to.Date )
                .AsQueryable();

            if( !string.IsNullOrEmpty(deviceAddress) ){
                query = query.Where( item => item.MeterAddress == deviceAddress);
            }

            totalItems = query.Count();

            IEnumerable<Measurement> data = Array.Empty<Measurement>();
            if( chunk == 0){
                data = query.Select( item => MeasurementAdapter.FromMeterDataTable(item) ).ToList();
            }else{
                data = query
                    .Skip( chunk * page)
                    .Take( chunk)
                    .Select( item => MeasurementAdapter.FromMeterDataTable(item) )
                    .ToList();
            }

            return data;

        }


    }
}   
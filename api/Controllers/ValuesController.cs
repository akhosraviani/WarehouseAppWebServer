using api.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class ValuesController : ApiController
    {

        AshaERPEntities db = new AshaERPEntities();
        // GET api/values
        public string Get()
        {
            return "123";
            //var items = db.WMInv_Part.Take(10).ToList();
            //return items;
        }

        // GET api/values/5
        public string Get(string id)
        {
            var partSerial = db.WMInv_PartSerial.Where(serial => serial.Code == id).FirstOrDefault();
            //var partCode = db.WMInv_Part.Where(part => part.Code == partSerial.PartCode).FirstOrDefault();

            string jsonValue = JsonConvert.SerializeObject(partSerial.PartCode);
            return jsonValue;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            if (value != null)
                System.IO.File.WriteAllText(@"D:\7799_Other\pds\WebServiceTest.txt", value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            if (value != null)
                System.IO.File.WriteAllText(@"D:\7799_Other\pds\WebServiceTest.txt", value);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

using api.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace api.Controllers
{
    public class SDSO_ShipmentController : ApiController
    {
        [Route("asha/SDSO_Shipment/GetAll")]
        [HttpGet]
        public object GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM SDSO_Shipment"
                                                        , con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Route("asha/SDSO_Shipment/GetAllLoading")]
        [HttpGet]
        public object GetAllOpen()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM SDSO_Shipment WHERE FormStatusCode = 'Shp_Loading'"
                                                        , con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        [Route("asha/SDSO_Shipment/{id}")]
        [HttpGet]
        public object Get(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM SDSO_Shipment WHERE Code = '" + id + "'"
                                                        , con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Route("asha/SDSO_Shipment/{id}/Details")]
        [HttpGet]
        public object GetDetails(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(WMLog_Driver.IDNumber, RIGHT(WMLog_Driver.DriverCode, 10)) as IDNumber, WMLog_Driver.Title AS DriverName, WMLog_Driver.CellPhoneNumber, WMLog_Vehicle.CarrierNumber, " +
                                                       "SDSO_Shipment.BillOfLadingNO, SDSO_Shipment.TruckWeight, dbo.MiladiTOShamsi(SDSO_Shipment.ReceptionDate) AS ReceptionDate " +
                                                       "FROM SDSO_Shipment INNER JOIN  WMLog_Driver ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode INNER JOIN " +
                                                       "WMLog_Vehicle ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code WHERE SDSO_Shipment.Code='" + id + "'"
                                                        , con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Route("asha/SDSO_Shipment/{shipmentCode}")]
        [HttpPost]
        // POST asha/SDSO_Shipment/{phlWMCode}
        public HttpResponseMessage Post(string shipmentCode, [FromBody]object value)
        {
            JObject jsonValue = value as JObject;
            int seq = 0;

            if (jsonValue != null)
            {
                JToken PartSerialToken;
                jsonValue.TryGetValue("PartSerialCode", out PartSerialToken);
                var ShipmentAuthorizeCode = (string)jsonValue["ShipmentAuthorizeCode"]; 

                if (PartSerialToken.Count() <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
           

                Dictionary<string, object> row = new Dictionary<string, object>();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                for (int i = 0; i < PartSerialToken.Count(); i++)
                {
                    var PartSerialCode = PartSerialToken.ElementAt(i).ToString();
                    row = new Dictionary<string, object>();
                    row.Add("ProductSerialCode", PartSerialCode);

                    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                                 ConnectionStrings["AshaERPEntities"].ConnectionString))
                    {

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        using (SqlCommand cmd = new SqlCommand("EXEC SDSO_001_ShipmentPartSerial @ShipmentAuthorizeCode, @shipmentCode, @PartSerialCode, 'Ship', '1', '', 1"
                                                                , con))
                        {
                            if (String.IsNullOrEmpty(ShipmentAuthorizeCode))
                                cmd.Parameters.AddWithValue("@ShipmentAuthorizeCode", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@ShipmentAuthorizeCode", ShipmentAuthorizeCode);

                            if (String.IsNullOrEmpty(shipmentCode))
                                cmd.Parameters.AddWithValue("@shipmentCode", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@shipmentCode", shipmentCode);

                            if (String.IsNullOrEmpty(PartSerialCode))
                                cmd.Parameters.AddWithValue("@PartSerialCode", DBNull.Value);
                            else
                                cmd.Parameters.AddWithValue("@PartSerialCode", PartSerialCode);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                row.Add("Message", "Saved Correctly");
                            }
                            catch (Exception e)
                            {
                                row.Add("Message", e.Message);
                            }
                        }
                    }
                    rows.Add(row);
                }
                return Request.CreateResponse(HttpStatusCode.OK, rows);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("asha/SDSO_Shipment/{phlWMCode}/{sequence}")]
        [HttpPut]
        // PUT asha/PhysicalWarehousing/5
        public HttpResponseMessage Put(string phlWMCode, int sequence, [FromBody]object value)
        {
            JObject jsonValue = value as JObject;

            if (jsonValue != null)
            {
                var quantity = (decimal)jsonValue["QTY"];
                var location = (string)jsonValue["Location"];
                var lotCode = (string)jsonValue["LotCode"];

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                             ConnectionStrings["AshaERPEntities"].ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("UPDATE WMPhl_PhysicalWarehousingDetail SET LocationCode=@LocationCode, LotCode=@LotCode, FirstCountQty=@FirstCountQty WHERE PhlWMCode=@PhlWMCode AND Sequence=@Sequence"
                                                            , con))
                    {
                        if (String.IsNullOrEmpty(location))
                        {
                            cmd.Parameters.AddWithValue("@LocationCode", DBNull.Value);
                        }
                        else
                            cmd.Parameters.AddWithValue("@LocationCode", location);

                        if (String.IsNullOrEmpty(location))
                        {
                            cmd.Parameters.AddWithValue("@LotCode", DBNull.Value);
                        }
                        else
                            cmd.Parameters.AddWithValue("@LotCode", lotCode);

                        cmd.Parameters.AddWithValue("@FirstCountQty", quantity);
                        cmd.Parameters.AddWithValue("@PhlWMCode", phlWMCode);
                        cmd.Parameters.AddWithValue("@Sequence", sequence);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        catch(Exception e)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("asha/SDSO_Shipment/{shipmentCode}/{PartSerialCode}/{ShipmentAuthorizeCode}")]
        [HttpDelete]
        // DELETE asha/SDSO_Shipment/5
        public HttpResponseMessage Delete(string shipmentCode, string PartSerialCode, string ShipmentAuthorizeCode)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EXEC SDSO_001_ShipmentPartSerial @ShipmentAuthorizeCode, @shipmentCode, @PartSerialCode, 'Delete', '1', '', 1"
                                                                , con))
                {
                    if (String.IsNullOrEmpty(ShipmentAuthorizeCode))
                        cmd.Parameters.AddWithValue("@ShipmentAuthorizeCode", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ShipmentAuthorizeCode", ShipmentAuthorizeCode);

                    if (String.IsNullOrEmpty(shipmentCode))
                        cmd.Parameters.AddWithValue("@shipmentCode", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@shipmentCode", shipmentCode);

                    if (String.IsNullOrEmpty(PartSerialCode))
                        cmd.Parameters.AddWithValue("@PartSerialCode", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@PartSerialCode", PartSerialCode);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    catch (Exception e)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}

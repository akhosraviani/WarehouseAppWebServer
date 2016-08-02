using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage Post(string shipmentCode, [FromBody]object value)
        {
            JObject jsonValue = value as JObject;
            int seq = 0;

            if (jsonValue != null)
            {
                JToken PartSerialToken;
                jsonValue.TryGetValue("PartSerialCode", out PartSerialToken);

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

                        using (SqlCommand cmd = new SqlCommand("SDSO_001_ShipmentPartSerial"
                                                                , con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // set up the parameters
                            cmd.Parameters.Add("@shipmentCode", SqlDbType.NVarChar, 64);
                            cmd.Parameters.Add("@PartSerialCode", SqlDbType.NVarChar, 64);
                            cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 64);
                            cmd.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                            cmd.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                            // set parameter values
                            cmd.Parameters["@shipmentCode"].Value = shipmentCode;
                            cmd.Parameters["@PartSerialCode"].Value = PartSerialCode;
                            cmd.Parameters["@Mode"].Value = "ship";
                            cmd.Parameters["@CreatorCode"].Value = "1";
                            cmd.Parameters["@ReturnMessage"].Value = "";
                            cmd.Parameters["@ReturnValue"].Value = 1;

                            try
                            {
                                cmd.ExecuteNonQuery();
                                // read output value from @NewId
                                string returnMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value);
                                int returnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                                row.Add("Message", returnMessage);
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

        [Route("asha/SDSO_Shipment/CompleteLoading/{shipmentCode}")]
        [HttpPut]
        public HttpResponseMessage Put(string shipmentCode, [FromBody]object value)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            row = new Dictionary<string, object>();

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                                  ConnectionStrings["AshaERPEntities"].ConnectionString))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();

                using (SqlCommand cmd = new SqlCommand("SDSO_001_ShipmentStatus"
                                                        , con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@ShipmentCode", SqlDbType.NVarChar, 64);
                    cmd.Parameters.Add("@StatusCode", SqlDbType.NVarChar, 64);
                    cmd.Parameters.Add("@NewStatusCode", SqlDbType.NVarChar, 64);
                    cmd.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                    cmd.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                    cmd.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@shipmentCode"].Value = shipmentCode;
                    cmd.Parameters["@StatusCode"].Value = "Shp_Loading";
                    cmd.Parameters["@NewStatusCode"].Value = "Shp_SecondWeighing";
                    cmd.Parameters["@PositionCode"].Value = "Pos_999";
                    cmd.Parameters["@CreatorCode"].Value = "1";
                    cmd.Parameters["@ReturnMessage"].Value = "";
                    cmd.Parameters["@ReturnValue"].Value = 1;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        // read output value from @NewId
                        string returnMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value);

                        row.Add("Message", returnMessage);
                    }
                    catch (Exception e)
                    {
                        row.Add("Message", e.Message);
                    }
                }
            }
            rows.Add(row);
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Route("asha/SDSO_Shipment/{shipmentCode}/{PartSerialCode}")]
        [HttpDelete]
        // DELETE asha/SDSO_Shipment/5
        public HttpResponseMessage Delete(string shipmentCode, string PartSerialCode)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EXEC SDSO_001_ShipmentPartSerial @shipmentCode, @PartSerialCode, 'Delete', '1', '', 1"
                                                                , con))
                {
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

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
    public class SDSO_ShipmentAuthorizeController : ApiController
    {
        [Route("asha/SDSO_ShipmentAuthorize/GetAll")]
        [HttpGet]
        public object GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM SDSO_ShipmentAuthorize"
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

        [Route("asha/SDSO_ShipmentAuthorize/GetAllLoading")]
        [HttpGet]
        public object GetAllOpen()
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
        [Route("asha/SDSO_ShipmentAuthorize/{id}")]
        [HttpGet]
        public object Get(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT SDSO_ShipmentAuthorize.Code, SDSO_ShipmentAuthorize.Title, " +
                                                       "SDSO_ShipmentAuthorize.ProductCode, WMInv_Part.Title AS ProductTitle " +
                                                       "FROM WMLog_TransportDetail INNER JOIN SDSO_ShipmentAuthorize " +
                                                       "ON WMLog_TransportDetail.ShipmentAuthorizeCode = SDSO_ShipmentAuthorize.Code " +
                                                       "INNER JOIN SDSO_Shipment ON WMLog_TransportDetail.TransportCode = SDSO_Shipment.TransportCode " +
                                                       "INNER JOIN WMInv_Part ON SDSO_ShipmentAuthorize.ProductCode = WMInv_Part.Code " +
                                                       "WHERE SDSO_Shipment.Code = '" + id + "'"
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

        [Route("asha/SDSO_ShipmentAuthorize/{id}/Details")]
        [HttpGet]
        public object GetDetails(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT SDSO_ShipmentDetail.PartSerialCode, SDSO_ShipmentDetail.ShipmentAuthorizeCode " + 
                                                       "FROM SDSO_Shipment INNER JOIN SDSO_ShipmentDetail ON SDSO_Shipment.Code = SDSO_ShipmentDetail.ShipmentCode " +
                                                       "WHERE SDSO_Shipment.Code = '" + id + "'"
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

        [Route("asha/SDSO_ShipmentAuthorize/{phlWMCode}")]
        [HttpPost]
        // POST asha/PhysicalWarehousing/{phlWMCode}
        public HttpResponseMessage Post(string phlWMCode, [FromBody]object value)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        [Route("asha/SDSO_ShipmentAuthorize/{phlWMCode}/{sequence}")]
        [HttpPut]
        // PUT asha/PhysicalWarehousing/5
        public HttpResponseMessage Put(string phlWMCode, int sequence, [FromBody]object value)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        // DELETE asha/Authenticate/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}

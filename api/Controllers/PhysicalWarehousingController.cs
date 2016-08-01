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
    public class PhysicalWarehousingController : ApiController
    {
        [Route("asha/PhysicalWarehousing/GetAll")]
        [HttpGet]
        public object GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Code, Title, InventoryCode FROM WMPhl_PhysicalWarehousing"
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

        [Route("asha/PhysicalWarehousing/GetAllOpen")]
        [HttpGet]
        public object GetAllOpen()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Code, Title, InventoryCode FROM WMPhl_PhysicalWarehousing WHERE FormStatusCode = 'Open'"
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
        [Route("asha/PhysicalWarehousing/{id}")]
        [HttpGet]
        public object Get(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM WMPhl_PhysicalWarehousing WHERE Code = '" + id + "'"
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

        [Route("asha/PhysicalWarehousing/{id}/Details")]
        [HttpGet]
        public object GetDetails(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT A.Sequence AS Sequence, A.PartCode AS PartCode, B.Title AS PartTitle, A.FirstCountQty AS QTY, A.UnitOfMeasureCode AS UOM FROM WMPhl_PhysicalWarehousingDetail AS A INNER JOIN WMInv_Part AS B ON A.PartCode = B.Code WHERE PhlWMCode = '" + id + "'"
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

        [Route("asha/PhysicalWarehousing/{phlWMCode}")]
        [HttpPost]
        // POST asha/PhysicalWarehousing/{phlWMCode}
        public HttpResponseMessage Post(string phlWMCode, [FromBody]object value)
        {
            JObject jsonValue = value as JObject;
            int seq = 0;

            if (jsonValue != null)
            {
                var quantity = (decimal)jsonValue["QTY"];
                var UOM = (string)jsonValue["UOM"];
                var location = (string)jsonValue["Location"];
                var lotCode = (string)jsonValue["LotCode"];
                var partCode = (string)jsonValue["PartCode"];

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                             ConnectionStrings["AshaERPEntities"].ConnectionString))
                {

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT MAX(Sequence) AS Sequence FROM WMPhl_PhysicalWarehousingDetail WHERE PhlWMCode = '" + phlWMCode + "'"
                                                       , con))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if(dt.Rows.Count == 1)
                            seq = Convert.ToInt32(dt.Rows[0][0])+1;
                        else
                            seq = 1;
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO WMPhl_PhysicalWarehousingDetail(PhlWMCode, Sequence, PartCode, LocationCode, LotCode, UnitOfMeasureCode, FirstCountQty, CompletionStatus, CreatorCode, CreationDate, Note) VALUES(@PhlWMCode, @Sequence, @PartCode, @LocationCode, @LotCode, @UnitOfMeasureCode, @FirstCountQty, 0, 1, GETDATE(), @PartCode)"
                                                            , con))
                    {
                        if (String.IsNullOrEmpty(location))
                            cmd.Parameters.AddWithValue("@LocationCode", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@LocationCode", location);

                        if (String.IsNullOrEmpty(location))
                            cmd.Parameters.AddWithValue("@LotCode", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@LotCode", lotCode);

                        cmd.Parameters.AddWithValue("@FirstCountQty", quantity);
                        cmd.Parameters.AddWithValue("@PhlWMCode", phlWMCode);
                        cmd.Parameters.AddWithValue("@Sequence", seq);
                        cmd.Parameters.AddWithValue("@PartCode", partCode);
                        cmd.Parameters.AddWithValue("@UnitOfMeasureCode", UOM);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            return Request.CreateResponse(HttpStatusCode.OK, seq);
                        }
                        catch (Exception e)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("asha/PhysicalWarehousing/{phlWMCode}/{sequence}")]
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

        // DELETE asha/Authenticate/5
        public void Delete(int id)
        {
            Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}

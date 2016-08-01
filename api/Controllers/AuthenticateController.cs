using api.Data;
using Newtonsoft.Json;
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
    public class AuthenticateController : ApiController
    {

        public static string EncryptString(string s)
        {
            string sKey = "3xp10r3r";
            MemoryStream ms = new MemoryStream();

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(s);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        public static string DecryptString(string s)
        {
            string sKey = "3xp10r3r";
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(s));

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostream = new CryptoStream(ms, desdecrypt, CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(cryptostream);
            return reader.ReadToEnd();
        } 
        // GET asha/Authenticate
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        [Route("asha/Authenticate/{username}/{password}")]
        [HttpGet]
        public HttpResponseMessage ValidateUsers(string username, string password) 
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row = new Dictionary<string,object>();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            var pwd = EncryptString(password + username.Length.ToString() + username.ToLower());
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                                                         ConnectionStrings["AshaERPEntities"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT SIScr_User.Code, HREA_Personnel.Title, HREA_Personnel.PersonnelImage FROM SIScr_User inner join HREA_Personnel on SIScr_User.Code = HREA_Personnel.UserCode" +
                                                       " WHERE SIScr_User.Title = '" + username + "' AND SIScr_User.Password = '" + pwd + "'"
                                                        , con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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
                    else
                    {
                        row.Add("Status", "InValid");
                        rows.Add(row);
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
        // GET asha/Authenticate/arash?pass=123
        public HttpResponseMessage Get(string id, string pass)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        // POST asha/Authenticate
        public HttpResponseMessage Post([FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        // PUT asha/Authenticate/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
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

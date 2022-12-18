using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace TestAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet()]
        public ActionResult Get()
        {           
            SqlConnection cnn;
            string connetionString = "Data Source=DESKTOP-09Q6SHE\\SQLEXPRESS;Initial Catalog=Project_Client;User ID=sa;Password=Gjorgji1@";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                DataSet dt = new DataSet();
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "GetClient";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt, "test");
                List<Client> clients = new List<Client>();

                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        Client client = new Client();
                        client.Name = dt.Tables[0].Rows[i]["Name"].ToString();
                        client.Address = dt.Tables[0].Rows[i]["Address"].ToString();
                        client.BirthDate = dt.Tables[0].Rows[i]["BirthDate"].ToString();

                        clients.Add(client);
                    }

                }
                return Ok(clients);

            } catch (Exception)
            {
                return null;
            } finally
            {
                cnn.Close();
            }
           
        }
        [HttpPut]
        public int Put([FromBody] Client client)
        {           
            int returnvalue;            
            SqlConnection cnn;
            string connetionString = "Data Source=DESKTOP-09Q6SHE\\SQLEXPRESS;Initial Catalog=Project_Client;User ID=sa;Password=Gjorgji1@";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "InsertClient";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ClientName", SqlDbType.NVarChar, 256)).Value = client.Name;
                cmd.Parameters.Add(new SqlParameter("@ClientAddress", SqlDbType.NVarChar, 256)).Value = client.Address;
                cmd.Parameters.Add(new SqlParameter("@ClientBirthDate", SqlDbType.NVarChar, 256)).Value = client.BirthDate;
                cmd.Parameters.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                returnvalue = int.Parse(cmd.Parameters["@return_value"].Value.ToString());                

                return returnvalue;
            }
            catch(Exception)
            {
                return 0;
            }finally
            {
                cnn.Close();
            }
        }
    }
}

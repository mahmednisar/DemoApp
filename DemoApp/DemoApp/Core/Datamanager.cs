using DemoApp.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Core
{
    public class Datamanager
    {

        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;
        private CurrentUser user;
        public Datamanager(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _config = configuration;
            _httpContext = httpContext;
            user = (CurrentUser)httpContext.HttpContext.Items["User"];
        }


        private SqlConnection NewConnection(string compName = null)
        {
            if (compName == null)
            {
                if (user != null)
                {
                    compName = user.CompID;
                }
                else
                {
                    compName = "demo";
                }
            }
            var conn = new SqlConnection(_config.GetConnectionString(compName));

            return conn;
        }


        public DataSet GetDataSet(string Procedure,List<DataParam> datas, string compName= null ) {
            DataSet dataSet = new DataSet();
            var cmd = new SqlCommand(Procedure, NewConnection(compName))
            {
                 CommandType= CommandType.StoredProcedure, CommandTimeout=0
            };
            foreach (var data in datas) { cmd.Parameters.AddWithValue(data.Name.Trim(), data.Value); }
            var dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataSet);
            return dataSet;
        }
    }

    public class DataParam { 
        public string Name { get; set; } 
        public object Value{ get; set; } 
    }
}

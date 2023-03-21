using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MauiApp3.Data.Interfeses
{
    internal interface IDataBase
    {
        bool NewConnection(string[] values);
        bool ChangeConnection();
        void OpenConn(string str);
        void CloseCon();
        void OpenConn();
        DataSet ConnDataSet(string str);
        SqlDataReader Conn(string str);
        bool ConnChange(string str, List<SqlParameter> list);
    }
}

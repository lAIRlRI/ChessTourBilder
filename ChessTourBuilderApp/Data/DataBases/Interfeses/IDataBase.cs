using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.DataBases.Interfeses
{
    internal interface IDataBase
    {
        SqlDataReader Conn(string str);

        DataSet ConnDataSet(string str);

        bool ConnChange(string str);

        bool ConnChange(string str, List<SqlParameter> list);

        void OpenConn(string str);

        void OpenConn(string str, List<SqlParameter> list);

        void OpenConn();

        void CloseCon();

        bool ChangeConnection();

        string GetTables();

        bool ConnChangeTemp(string str);

        string NewConnection(string[] values);

        bool ConnChangeFull(string str, List<SqlParameter> list);

        SqlDataReader ConnFull(string str);

        DataSet ConnDataSetFull(string str);

        bool ConnChangeFull(string str);
    }
}

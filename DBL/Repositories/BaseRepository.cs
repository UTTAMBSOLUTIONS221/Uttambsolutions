using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    namespace DBL.Repositories
    {
        public abstract class BaseRepository
        {
            protected string _connString;

            public BaseRepository(string connectionString)
            {
                _connString = connectionString;
            }
            public string Getdatacount(string tableName)
            {
                return string.Format("Select count(Idxno) From {0}", tableName);
            }

            public string DeleteStatement(string tableName, string idColumn)
            {
                return string.Format("Delete From {0} Where {1} = @Id", tableName, idColumn);
            }

            public string FindStatement0(string tableName, string idColumn)
            {
                return string.Format("Select * From {0} Where {1} = @Id", tableName, idColumn);
            }
            public string FindStatement1(string tableName, string idColumn, string idColumn1)
            {
                return string.Format("Select * From {0} Where {1} = @Id and {2} = @Id1", tableName, idColumn, idColumn1);
            }
            public string FindStatement(string tableName, string idColumn)
            {
                return string.Format("Select * From {0} Where {1} = @Id", tableName, idColumn);
            }
            public string FindStatement(string tableName, string idColumn, int top)
            {
                return string.Format("Select top {2} * From {0} Where {1} = @Id", tableName, idColumn, top);
            }

            public string GetAllMohoroCareers(string tableName)
            {
                return string.Format("Select * From {0} Where getdate()<= Expirydate", tableName);
            }

            public string GetAllStatementbystatus(string tableName, string column)
            {
                return string.Format("Select * From {0} Where {1} = @Column", tableName, column);
            }

            public string GetAllStatement(string tableName, string idColumn)
            {
                return string.Format("Select * From {0} order by {1} desc", tableName, idColumn);
            }
            public string GetAllStatementbyofset(string tableName, int Offsetid)
            {
                return string.Format("SELECT  * FROM {0} ORDER BY Productcode OFFSET {1} ROWS FETCH NEXT 10 ROWS ONLY", tableName, Offsetid);
            }
            public string GetAllStatementbyidandoffset(string tableName, string idColumn, int Offsetid)
            {
                return string.Format("SELECT  * FROM {0} WHERE {1} = @Id ORDER BY Productcode OFFSET {2} ROWS FETCH NEXT 10 ROWS ONLY", tableName, idColumn, Offsetid);
            }
            public string GetAllStatementactive(string tableName)
            {
                return string.Format("Select * From {0} WHERE Isactive=1", tableName);
            }
            public string GetAllStatement(string tableName)
            {
                return string.Format("Select * From {0}", tableName);
            }
            public string GetTabledatacount(string tableName)
            {
                return string.Format("Select Count(Idxno) From {0}", tableName);
            }

            public string GetAllStatement(string tableName, int top)
            {
                return string.Format("Select top {1} * From {0}", tableName, top);
            }

            public string UpdateStatement(string tableName, string column, string idColumn)
            {
                return string.Format("Update {0} Set {1}=@Column Where {2} = @Id", tableName, column, idColumn);
            }

            public string UpdateTwoTableStatement(string tableName, string column, string idColumn, string idColumn1)
            {
                return string.Format("Update {0} Set {1}=@Column Where {2} = @Id and {3}=@Id1", tableName, column, idColumn, idColumn1);
            }
            public string FindStatementone(string tableName, string idColumn, string idColumnone, string idColumntwo)
            {
                return string.Format("Select * From {0} Where {1} = @Id and {2} = @Idone and {3}=@Idtwo", tableName, idColumn, idColumnone, idColumntwo);
            }
        }
    }
}

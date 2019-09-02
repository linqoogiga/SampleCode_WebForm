using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;


namespace DataBaseKernel
{
    public class CStarTravel_DBAccess
    {
        public static DataSet GetDataSet(string p_SQL, OleDbParameter[] parameters)
        {
            OleDbConnection conn = new OleDbConnection("Provider=OraOLEDB.Oracle;Data Source=STARTRIP_5_1_22;Persist Security Info=True;Password=starsys;User ID=starsys");
            DataSet TpDS = new DataSet();
            using (var adapter = new OleDbDataAdapter(p_SQL, conn))
            {
                try
                {
                    // Set the parameters.
                    if (parameters != null)
                    {
                        foreach (OleDbParameter t in parameters)
                        {
                            if (t != null)
                            {
                                adapter.SelectCommand.Parameters.AddWithValue(t.ParameterName, t.Value);
                            }
                        }
                    }
                    // Open the connection and fill the DataSet.
                    conn.Open();
                    adapter.Fill(TpDS, "ZA");
                    //EventLog.Write("GetDataSetFromAdapter queryString:" + adapter.SelectCommand.CommandText);
                    adapter.SelectCommand.Parameters.Clear();
                }
                catch (OleDbException se)
                {
                    return null;
                }
                finally
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
            return TpDS;
        }
    }
}
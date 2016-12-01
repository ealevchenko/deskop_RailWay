using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class NeighbourStationsDB : DB
    {
        //private static NeighbourStationsDB neighbourStationsDB;

        //private NeighbourStationsDB() { }

        //public static NeighbourStationsDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (neighbourStationsDB == null)
        //    {
        //        lock (typeof(NeighbourStationsDB))
        //        {
        //            if (neighbourStationsDB == null)
        //                neighbourStationsDB = new NeighbourStationsDB();
        //        }
        //    }

        //    return neighbourStationsDB;
        //}

        public Side getArrivSide(Station statOn, Station statFrom)
        {
            string query = string.Format(
                "if (exists (select * from NeighborStations where id_stat1=@id_statOn and id_stat2=@id_statFrom)) " +
                    "select side_stat1 as side from NeighborStations where id_stat1=@id_statOn and id_stat2=@id_statFrom " +
                "else if (exists (select * from NeighborStations where id_stat1=@id_statFrom and id_stat2=@id_statOn)) " +
                    "select side_stat2 as side from NeighborStations where id_stat1=@id_statFrom and id_stat2=@id_statOn "+
                "else select -1 as side");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@id_statOn", statOn.ID);
            sqlParameters[1] = new SqlParameter("@id_statFrom", statFrom.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            try
            {
                return (Side)Int32.Parse(table.Rows[0]["side"].ToString());
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException || e is NullReferenceException)
                    return Side.Empty;
                else throw;
            }
        }

        public Side getSendSide(Station statFrom, Station statOn)
        {
            string query = string.Format(
                "if (exists (select * from NeighborStations where id_stat1=@id_statFrom and id_stat2=@id_statOn)) " +
                    "select side_stat1 as side from NeighborStations where id_stat1=@id_statFrom and id_stat2=@id_statOn " +
                "else if (exists (select * from NeighborStations where id_stat1=@id_statOn and id_stat2=@id_statFrom)) " +
                    "select side_stat2 as side from NeighborStations where id_stat1=@id_statOn and id_stat2=@id_statFrom "+
                "else select -1 as side");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            try
            {
                sqlParameters[0] = new SqlParameter("@id_statOn", statOn.ID);
                sqlParameters[1] = new SqlParameter("@id_statFrom", statFrom.ID);
                DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
                return (Side)Int32.Parse(table.Rows[0]["side"].ToString());
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException || e is NullReferenceException)
                    return Side.Empty;
                else throw;
            }
        }
    }
}

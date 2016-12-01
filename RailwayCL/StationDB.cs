using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace RailwayCL
{
    public class StationDB : DB
    {
        //private static StationDB stationDB;

        //private StationDB() { }

        //public static StationDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (stationDB == null)
        //    {
        //        lock (typeof(StationDB))
        //        {
        //            if (stationDB == null)
        //                stationDB = new StationDB();
        //        }
        //    }

        //    return stationDB;
        //}

        public List<Station> getStations()
        {
            List<Station> list = new List<Station>();

            string query = string.Format("select id_stat, name, coalesce(outer_side, 1) as side from STATIONS where is_uz = 0 order by name");
            DataTable table = Conn.executeSelectQuery(query, new SqlParameter[0]).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Station station = new Station(Int32.Parse(row["id_stat"].ToString()),
                    row["name"].ToString().Trim());
                station.Outer_side = (Side)Int32.Parse(row["side"].ToString());
                list.Add(station);
            }

            return list;
        }

        public List<Station> getStations(Station stat) // выбор смежных станций
        {
            List<Station> list = new List<Station>();

            string query = string.Format("select id_stat, name, coalesce(outer_side, 1) as side "+
                                         "FROM STATIONS "+
                                         "where id_stat in (select id_stat1 from NeighborStations where id_stat2=@id_stat) " +
                                         "or id_stat in (select id_stat2 from NeighborStations where id_stat1=@id_stat) ");
                                         //"union "+
                                         //"select id_stat, name, coalesce(outer_side, 1) as side "+
                                         //"FROM STATIONS "+
                                         //"WHERE id_stat=@id_stat and (@id_stat in (select id_stat from SHOPS) "+
                                         //"or @id_stat in (select id_stat from GRUZ_FRONTS)) order by name");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            //sqlParameters[1] = new SqlParameter("@is_uz", is_uz);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Station station = new Station(Int32.Parse(row["id_stat"].ToString()),
                    row["name"].ToString().Trim());
                station.Outer_side = (Side)Int32.Parse(row["side"].ToString());
                list.Add(station);
            }
            return list;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class ShopDB : DB
    {
        //private static ShopDB shopDB;

        //private ShopDB() { }

        //public static ShopDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (shopDB == null)
        //    {
        //        lock (typeof(ShopDB))
        //        {
        //            if (shopDB == null)
        //                shopDB = new ShopDB();
        //        }
        //    }

        //    return shopDB;
        //}

        public List<Shop> getShops(Station stat)
        {
            List<Shop> list = new List<Shop>();

            //string query = string.Format("select s.id_shop, s.name, count(vo.id_oper) as vag_amount "+ 
            //"from SHOPS s left join VAGON_OPERATIONS vo on s.id_shop=vo.st_shop and vo.is_hist=0 "+
            //"where s.id_stat=@id_stat "+
            //"group by id_shop, name "+
            //"order by id_shop");
            //SqlParameter[] sqlParameters = new SqlParameter[1];
            //sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            //DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            string query = "[RailCars].[GetShop]";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@idstation", stat.ID);
            DataTable table = Conn.executeProc(query, sqlParameters).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Shop shop = new Shop { ID = Int32.Parse(row["id_shop"].ToString()), Name = row["name"].ToString().Trim(),
                    Stat=stat, Vag_amount=Int32.Parse(row["vag_amount"].ToString())};

                list.Add(shop);
            }

            return list;
        }
    }
}

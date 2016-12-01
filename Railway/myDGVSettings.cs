using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace RailwayUI
{
    public class myDGVSettings
    {
        public static string bindProperty(object property, string propertyName)
        {
            string retValue = "";

            if (propertyName.Contains("."))
            {
                PropertyInfo[] arrayProperties;
                string leftPropertyName;

                leftPropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
                arrayProperties = property.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in arrayProperties)
                {
                    if (propertyInfo.Name == leftPropertyName)
                    {
                        retValue = bindProperty(propertyInfo.GetValue(property, null),
                            propertyName.Substring(propertyName.IndexOf(".") + 1));
                        break;
                    }
                }
            }
            else
            {
                Type propertyType;
                PropertyInfo propertyInfo;

                try
                {
                    propertyType = property.GetType();
                    propertyInfo = propertyType.GetProperty(propertyName);

                    if (propertyInfo.PropertyType == typeof(DateTime))
                        retValue = ((DateTime)propertyInfo.GetValue(property, null)).ToShortDateString();
                    else retValue = propertyInfo.GetValue(property, null).ToString();
                }
                catch (NullReferenceException) { }
            }

            return retValue;
        }

        public static object getCellValue(DataGridView dgv, int rowIdx, int colIdx)
        {
            try
            {
                if ((dgv.Rows[rowIdx].DataBoundItem != null) &&
                    (dgv.Columns[colIdx].DataPropertyName.Contains(".")))
                {
                    return bindProperty(dgv.Rows[rowIdx].DataBoundItem,
                        dgv.Columns[colIdx].DataPropertyName);
                }
            }
            catch { }
            return null;
        }
    }
}

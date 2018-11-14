using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSAP
{
    class MaterialList
    {
        private DatabaseConnection databaseConnection;
        private DataTable table;
        public MaterialList(DataTable table, DatabaseConnection con)
        {
            this.table = table;
            databaseConnection = con;
        }

        public DataTable AddColums()
        {
            table.Columns.Add("Material");
            table.Columns.Add("Quantity(in Tonnes)");
            return table;
        }

        public DataTable AddRows()
        {
            DataRow dr;
            List<String[]> l = databaseConnection.GetMaterial();
            foreach (String[] s in l)
            {
                dr = table.NewRow();
                dr[0] = s[0];
                dr[1] = s[1];
                table.Rows.Add(dr);
            }
            return table;
        }
    }
}

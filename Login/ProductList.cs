using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSAP
{
    class ProductList
    {
        private DatabaseConnection databaseConnection;
        private DataTable table;
        public ProductList(DataTable table, DatabaseConnection con)
        {
            this.table = table;
            databaseConnection = con;
        }

        public DataTable AddColums()
        {
            table.Columns.Add("Manufacture ID");
            table.Columns.Add("Product ID");
            table.Columns.Add("Cost(in crores)");
            table.Columns.Add("Date of Maufacture");
            return table;
        }

        public DataTable AddRows()
        {
            DataRow dr;
            List<String[]> l = databaseConnection.getProducts();
            foreach (String[] s in l)
            {
                dr = table.NewRow();
                dr[0] = s[0];
                dr[1] = s[1];
                dr[2] = s[2];
                dr[3] = s[3];
                table.Rows.Add(dr);
            }
            return table;
        }
    }
}

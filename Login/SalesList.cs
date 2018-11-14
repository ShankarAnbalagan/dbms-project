using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSAP
{
    class SalesList
    {
        private DatabaseConnection databaseConnection;
        private DataTable table;
        public SalesList(DataTable table, DatabaseConnection con)
        {
            this.table = table;
            databaseConnection = con;
        }

        public DataTable AddColums()
        {
            table.Columns.Add("Buyer ID");
            table.Columns.Add("Product ID");
            table.Columns.Add("Vendor ID");
            table.Columns.Add("Quantity(in tonnes)");
            table.Columns.Add("Cost(in crores)");
            table.Columns.Add("Date of Purchase");
            return table;
        }

        public DataTable AddRows()
        {
            DataRow dr;
            List<String[]> l = databaseConnection.getSales();
            foreach (String[] s in l)
            {
                dr = table.NewRow();
                dr[0] = s[0];
                dr[1] = s[1];
                dr[2] = s[2];
                dr[3] = s[3];
                dr[4] = s[4];
                dr[5] = s[5];
                table.Rows.Add(dr);
            }
            return table;
        }
        }
}

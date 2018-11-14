using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSAP
{
    class BuyerList
    {
        private DatabaseConnection databaseConnection;
        private DataTable table;
        public BuyerList(DataTable table, DatabaseConnection con)
        {
            this.table = table;
            databaseConnection = con;
        }

        public DataTable AddColums()
        {
            table.Columns.Add("Buyer ID");
            table.Columns.Add("Buyer Name");
            table.Columns.Add("Buyer Address");
            table.Columns.Add("Buyer Phone no");
            return table;
        }

        public DataTable AddRows()
        {
            DataRow dr;
            List<String[]> l = databaseConnection.getBuyers();
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

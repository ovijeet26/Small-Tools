using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataInserter
{
    class insert
    {
         static void Main(string[] args)
        {
            SqlConnection scon;
            SqlCommand scmd;
            scon = new SqlConnection(@"server=M2381260\MSSQLSERVER2014;initial catalog=Realtime;user id=sa; password=pass");
            Console.WriteLine("Enter the number of iterations");
            int ctr = Int16.Parse(Console.ReadLine());
            Console.WriteLine();
            while (ctr>0)
            {
              
                scmd = new SqlCommand("INSERT INTO SensorData VALUES(SYSDATETIME(), ROUND((RAND()*10), 2));", scon);

                ctr--;

                scon.Open();
                scmd.ExecuteNonQuery();
                scon.Close();
                Thread.Sleep(1500);
                Console.WriteLine("NO "+ctr);
            }
            Console.WriteLine("End of execution");
            Console.ReadLine();
        }
    }
}
    
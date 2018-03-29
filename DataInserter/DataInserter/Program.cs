using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataInserter
{
    class Program
    {
        static void Main1(string[] args)
        {
            SqlConnection scon;
            SqlCommand scmd;
            scon = new SqlConnection(@"server=M2346254\JCISQLDATA;initial catalog=JCIDemo;user id=sa; password=kokojumbo");
            Console.WriteLine("Enter the number of iterations");
            int ctr = Int16.Parse(Console.ReadLine());
            Console.WriteLine();
            while (ctr>0)
            {
                Console.WriteLine("uno");
                scmd = new SqlCommand("insert into Chart.OneMonth values(@n,@p,@c,@v)", scon);

                scmd.Parameters.AddWithValue("@n", "Bangalore-IEC-3");
                scmd.Parameters.AddWithValue("@p", 20000);
                scmd.Parameters.AddWithValue("@c", 20000);
                scmd.Parameters.AddWithValue("@v", 25);

                scon.Open();
                scmd.ExecuteNonQuery();
                scon.Close();

                Thread.Sleep(2000);
                ctr--;
                Console.WriteLine("dos");
                scmd = new SqlCommand("insert into Chart.OneMonth values(sysdatetime(),@v)", scon);

               // scmd.Parameters.AddWithValue("@v", Math.random());

                scon.Open();
                scmd.ExecuteNonQuery();
                scon.Close();
                Console.WriteLine("tres");
                scmd = new SqlCommand("insert into Chart.OneMonth values(@n,@p,@c,@v)", scon);

                scmd.Parameters.AddWithValue("@n", "Kolkata-IEC");
                scmd.Parameters.AddWithValue("@p", 20000);
                scmd.Parameters.AddWithValue("@c", 20000);
                scmd.Parameters.AddWithValue("@v", -5);

                scon.Open();
                scmd.ExecuteNonQuery();
                scon.Close();
                Console.WriteLine("quattro");
                scmd = new SqlCommand("insert into Chart.OneMonth values(@n,@p,@c,@v)", scon);

                scmd.Parameters.AddWithValue("@n", "507E Michigan");
                scmd.Parameters.AddWithValue("@p", 23000);
                scmd.Parameters.AddWithValue("@c", 26000);
                scmd.Parameters.AddWithValue("@v", 42);

                scon.Open();
                scmd.ExecuteNonQuery();
                scon.Close();

            }
            Console.WriteLine("End of execution");
            Console.ReadLine();
        }
    }
}

//Random rnd = new Random();
//int month = rnd.Next(1, 13);
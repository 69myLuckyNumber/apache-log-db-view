using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApacheDBView
{
	public partial class Form1 : Form
	{
		private readonly string connectionString;

		public Form1()
		{
			connectionString = "Server=.\\SQLEXPRESS;Database=apache-db;Trusted_Connection=True;";
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

			using (var connection = new SqlConnection(connectionString))
			{
				SqlDataAdapter sqlAdapter = new SqlDataAdapter();

				using (var sqlCommand = new SqlCommand(@"SELECT CAST(h.IPAddressBytes as varchar(MAX)) HostIp, 
														h.HostName HostName,
														h.OrgName Org, 
														r.RequestType [Type], 
														r.ResponseStatusCode Code, 
														r.DateTimeRequested [DateTime], 
														f.FilePath [Path], 
														f.FileName [FileName], f.FileSize Size  

														FROM dbo.Hosts h
														JOIN dbo.Requests r
															ON r.RequestorIPAddress = h.IPAddressBytes
														JOIN dbo.Files f
															ON f.RequestId = r.RequestId")) 
				{
					sqlCommand.Connection = connection;
					try
					{
						sqlAdapter.SelectCommand = sqlCommand;
						DataTable dbSet = new DataTable();
						sqlAdapter.Fill(dbSet);
						BindingSource bindingSource = new BindingSource();

						bindingSource.DataSource = dbSet;

						dbGridView.DataSource = bindingSource;
						sqlAdapter.Update(dbSet);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}

				}
			}			
		}
	}
}

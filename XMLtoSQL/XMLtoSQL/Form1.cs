﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using XMLtoSQL.MainDataSetTableAdapters;

namespace XMLtoSQL
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        SqlConnection sqlConn = new SqlConnection(@"Server=PL1\MTCDB;Database=Sandbox;Trusted_Connection=True;");
        DataSet xmlData = new DataSet();
        int chosenID = 0;
        //BindingSource source = new BindingSource();

        private void frmMain_Load(object sender, EventArgs e)
        {
           
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                xmlData.Tables.Clear();
                xmlData.ReadXml(ofdFile.FileName);
                xmlData.Tables[0].Columns.Add("ID");
                dgData.DataSource = xmlData.Tables[0];

            }
        }

        //This also fires if a cell is clicked.
        private void dgData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            chosenID = e.RowIndex + 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            

            SqlCommand newComm = new SqlCommand("sp_InsertXML", sqlConn);
            newComm.CommandType = CommandType.StoredProcedure;

            if(dgData.Rows.Count > 0)
            {
                try
                {
                    sqlConn.Open();
                    foreach (DataGridViewRow row in dgData.Rows)
                    {

                        if (row.Cells["ssn"].Value != null)
                        {
                            newComm.Parameters.AddWithValue("@ID", 0);
                            newComm.Parameters["@ID"].Direction = ParameterDirection.Output;
                            newComm.Parameters.AddWithValue("@FirstName", row.Cells["first_name"].Value);
                            newComm.Parameters.AddWithValue("@LastName", row.Cells["last_name"].Value);
                            newComm.Parameters.AddWithValue("@SSN", row.Cells["ssn"].Value);
                            newComm.Parameters.AddWithValue("@Email", row.Cells["email"].Value);
                            newComm.Parameters.AddWithValue("@Gender", row.Cells["gender"].Value);
                            newComm.ExecuteNonQuery();
                            row.Cells["ID"].Value = newComm.Parameters["@ID"].Value;
                            newComm.Parameters.Clear();
                        }
                    }

                    sqlConn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void btnLoadTable_Click(object sender, EventArgs e)
        {
            try
            {
                MainDataSet.XMLConversionTestDataTable ctdTable = new MainDataSet.XMLConversionTestDataTable();
                XMLConversionTestTableAdapter cttAdapter = new XMLConversionTestTableAdapter();
                cttAdapter.Fill(ctdTable);
                dgData.DataSource = ctdTable;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAltSave_Click(object sender, EventArgs e)
        {
            try
            {
                QueriesTableAdapter cttAdapter = new QueriesTableAdapter();


                
                MainDataSet.XMLConversionTestDataTable newTable = new MainDataSet.XMLConversionTestDataTable();
                DataTable rawTable = (DataTable)dgData.DataSource;

                foreach (DataRow rawRow in rawTable.Rows)
                {
                    int? id = 0;

                    cttAdapter.sp_InsertXML(rawRow["FirstName"].ToString(), rawRow["LastName"].ToString(), rawRow["SSN"].ToString(), rawRow["Email"].ToString(), rawRow["Gender"].ToString(), ref id);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
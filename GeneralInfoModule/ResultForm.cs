using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GeneralInfoModule
{
    public partial class ResultForm : Form
    {
        public ResultForm(DataTable values)
        {
            InitializeComponent();
            alignWidgets();
            dataChart.DataSource = values;
        }

        private void alignWidgets()
        {
            dataChart.Width = this.ClientRectangle.Width - 26;
            dataChart.Height = this.ClientRectangle.Height - 13 - menuStrip1.Height - 9;
        }

        private void ResultForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "Comma Separated Values|*.csv"; // format: "Description1|extension1|Description2|extension1..."
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                string extension = sfd.FileName.Substring(sfd.FileName.IndexOf('.'));
                if (extension == ".csv")
                {
                    using (FileStream saveStream = File.Open(sfd.FileName, FileMode.Create))
                    {
                        foreach (DataGridViewTextBoxColumn column in dataChart.Columns)
                        {
                            string snippet = column.Name + ",";
                            byte[] byteArray = new byte[snippet.Length];
                            for (int index = 0; index < snippet.Length; index++)
                            {
                                byteArray[index] = (byte)snippet[index];
                            }
                            saveStream.Write(byteArray, 0, byteArray.Length);
                        }
                        saveStream.Write(new byte[] { (byte)'\n' }, 0, 1);
                        foreach (DataGridViewRow row in dataChart.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string snippet = cell.Value.ToString() + ",";
                                byte[] byteArray = new byte[snippet.Length];
                                for (int index = 0; index < snippet.Length; index++)
                                {
                                    byteArray[index] = (byte)snippet[index];
                                }
                                saveStream.Write(byteArray, 0, byteArray.Length);
                            }
                            saveStream.Write(new byte[]{ (byte)'\n' }, 0, 1);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid file name.  Please use a .csv or .xlsx file extension");
                }
            }
        }
    }
}

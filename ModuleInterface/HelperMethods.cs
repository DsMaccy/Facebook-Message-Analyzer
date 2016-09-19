using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace ModuleInterface
{
    public static class HelperMethods
    {
        public static bool saveFile(DataTable table)
        {
            DataGridView chart = new DataGridView();
            chart.DataSource = table;
            return saveFile(chart);
        }

        public static bool saveFile(DataGridView dataChart)
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
                            saveStream.Write(new byte[] { (byte)'\n' }, 0, 1);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid file name.  Please use a .csv or .xlsx file extension");
                    return false;
                }
            }
            return true;
        }
    }
}

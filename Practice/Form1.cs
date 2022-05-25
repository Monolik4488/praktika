using System.Drawing.Imaging;

namespace Practice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int column_number = (int)numericUpDown1.Value;
            int rows_number = (int)numericUpDown2.Value;
            update_grid(column_number, rows_number);
        }

        private void update_grid(int c, int r)
        {
            data_grid.Rows.Clear();
            data_grid.Columns.Clear();
            

            for (int i = 0; i < c; i++)
            {
                DataGridViewColumn col = new DataGridViewTextBoxColumn();
                col.HeaderText = i.ToString();
                data_grid.Columns.Add(col);
            }

            for(int i = 0; i < r; i++)
            {
                data_grid.Rows.Add();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int column_number = (int)numericUpDown1.Value;
            int rows_number = (int)numericUpDown2.Value;
            update_grid(column_number, rows_number);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int column_number = (int)numericUpDown1.Value;
            int rows_number = (int)numericUpDown2.Value;
            update_grid(column_number, rows_number);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = "";
                int row_n = 0;
                List<int> maxes = new List<int>();
                foreach (DataGridViewRow row in data_grid.Rows)
                {
                    textBox1.Text += "--�������� " + row_n.ToString() + "\r\n";
                    int max = -99999;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        max = Math.Max(max, Convert.ToInt32(cell.Value));
                    }
                    maxes.Add(max);
                    textBox1.Text += "�������� � ������ " + row_n.ToString() + ": " + max.ToString() + "\r\n";
                    row_n += 1;
                }

                textBox1.Text += "--�������� " + row_n.ToString() + "\r\n";
                textBox1.Text += "����� ������������:\r\n";
                bool place_sign = false;
                int result = 1;
                foreach (int element in maxes)
                {
                    result *= element;
                    if (place_sign)
                    {
                        textBox1.Text += " * ";
                    }
                    textBox1.Text += element.ToString();
                    place_sign = true;
                }
                textBox1.Text += " = " + result.ToString() + "\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("� ����� �� ����� �������� �������");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var fileContent = string.Empty;
                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            int columns = Convert.ToInt32(reader.ReadLine());
                            int rows = Convert.ToInt32(reader.ReadLine());
                            numericUpDown1.Value = columns;
                            numericUpDown2.Value = rows;
                            for (int i = 0; i < rows; i++)
                            {
                                string row_string = reader.ReadLine();
                                var test = row_string.Split(' ');
                                for (int j = 0; j < columns; j++)
                                {
                                    data_grid.Rows[i].Cells[j].Value = test[j];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������� ������ �����");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myStream = saveFileDialog1.OpenFile();
                StreamWriter writer = new StreamWriter(myStream);
                writer.WriteLine(numericUpDown1.Value.ToString());
                writer.WriteLine(numericUpDown2.Value.ToString());
                for(int i = 0; i < numericUpDown1.Value; i++)
                {
                    for(int j = 0; j < numericUpDown2.Value; j++)
                    {
                        writer.Write(data_grid.Rows[i].Cells[j].Value + " ");
                    }
                    writer.WriteLine();
                }

                writer.WriteLine(textBox1.Text);
                writer.Close();
                myStream.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm = Form.ActiveForm;
            using (var bmp = new Bitmap(frm.Width, frm.Height))
            {
                frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    myStream = saveFileDialog1.OpenFile();
                    bmp.Save(myStream, ImageFormat.Png);
                    myStream.Close();
                }
                    
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            List<int> maxes = new List<int>();
            foreach (DataGridViewRow row in data_grid.Rows)
            {
                int max = -99999;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    max = Math.Max(max, Convert.ToInt32(cell.Value));
                }
                maxes.Add(max);
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myStream = saveFileDialog1.OpenFile();
                StreamWriter writer = new StreamWriter(myStream);
                writer.WriteLine(
@"<table>
<caption>
������� ������
</caption>
<thead>
<tr>
<th scope='col'></th>"
);

                for (int i = 0; i < numericUpDown2.Value; i++)
                {
                    writer.WriteLine(String.Format("<th scope='col'>{0}</th>", i));
                }
                writer.WriteLine(
@"</tr>
</thead>
<tbody>"
);
                
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine(String.Format("<th>{0}</th>", i));
                    for (int j = 0; j < numericUpDown2.Value; j++)
                    {
                        if(  Convert.ToInt32(data_grid.Rows[i].Cells[j].Value) == maxes[i])
                            writer.WriteLine(String.Format("<th>{0}</th>", data_grid.Rows[i].Cells[j].Value));
                        else
                            writer.WriteLine(String.Format("<td>{0}</td>", data_grid.Rows[i].Cells[j].Value));

                    }
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine(
@"</tbody>
</table>"
                );
                writer.WriteLine(textBox1.Text.Replace("\n", "<br>"));
                writer.Close();
                myStream.Close();
            }
        }
    }
}
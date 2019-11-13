if ((string) row.Cells[7].Value == "K")
{
  row.DefaultCellStyle.BackColor = Color.Green; // row color 
 }
 else
 {
  dataGridView1.Rows[row.Index].Cells[7].Style.BackColor = Color.Red;// cell color
 }

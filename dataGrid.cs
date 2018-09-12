
            dataGridView1.Rows.Add();
            dataGridView1.Rows[i].Cells[0].Value = "L1"; //Событие
            dataGridView1.Rows[i].Cells[1].Value = normal.Sample(); //таймер мод времени
            dataGridView1.Rows[i].Cells[2].Value = erlang.Sample(); //прибытие следуюшей заявки л1
            dataGridView1.Rows[i].Cells[3].Value = poisson.Sample(); //прибытие следуюшей заявки л2
            dataGridView1.Rows[i].Cells[4].Value = normal.Sample(); //окончание обслуживание текушей заявки
            dataGridView1.Rows[i].Cells[5].Value = "Bussy"; //статус сервера
            dataGridView1.Rows[i].Cells[6].Value = i; //текушия длина очереди
            dataGridView1.Rows[i].Cells[7].Value = i + 10; //кофициент простоя сервера
            dataGridView1.Rows[i].Cells[8].Value = "L1"; //средние время заявки в системе
            dataGridView1.Rows[i].Cells[9].Value = "L1"; //содержимое очереди
            dataGridView1.Refresh();

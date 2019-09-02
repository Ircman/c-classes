//Класс смотрит сколько времени прошло с момента запуска
class TimeWatcher
    {
        Stopwatch stopwatch = new Stopwatch();
       
        public void Start()
        {
            stopwatch.Start();
        }
        public void Stop()
        {
            stopwatch.Stop();
            var time = stopwatch.Elapsed;
            MessageBox.Show(string.Format("Время прошло : {0:hh\\:mm\\:ss}", time));
        }

    }

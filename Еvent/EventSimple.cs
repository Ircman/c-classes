class Person
    {
        internal delegate void HealthEvent();
        public event HealthEvent OnHealthChanged; 
        private int _health;
        public int Health
        {
            set
            {
                if (value != _health)
                {
                    _health = value;
                    if (OnHealthChanged != null) OnHealthChanged();
                }
            }
            get
            {
                return _health;
            }
        }    
    }
   /// после инициализируем и подписаваем на событие метод (СИГНАТУРА МЕТОДА ДОЛЖНА БЫТЬ ТАКАЯ ЖЕ КАК У ДЕЛЕГАТА)
     Person p = new Person();
     p.OnHealthChanged += InformIfHealthChanged;
     
     private void InformIfHealthChanged()
        {
            MessageBox.Show(p.Health.ToString());
        }

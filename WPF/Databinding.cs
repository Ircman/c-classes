            Binding myBinding = new Binding
            {
                Source = _class1,
                Path = new PropertyPath("Asd"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(tx_box, TextBox.TextProperty, myBinding);

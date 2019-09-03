var itemToRemove = _selectedPackages.Packages.Single(r => r.id == Convert.ToInt32(sqLreponse[i][0]));
                        if (itemToRemove != null)
                        _selectedPackages.Packages.Remove(itemToRemove);

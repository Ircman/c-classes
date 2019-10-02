var itemToRemove = _selectedPackages.Packages.Single(r => r.id == Convert.ToInt32(sqLreponse[i][0]));
                        if (itemToRemove != null)
                        _selectedPackages.Packages.Remove(itemToRemove);


// A function to search for people
Person FindPerson(int id) {
   var people = DbContext.GetPeople(); // Returns List<Person>

   return (from person in people
          where person.ID == id
          select person).ToList().FirstOrDefault();
}

// Then do this
var person = FindPerson(123);


Person FindPerson(int id) {
   var people = DbContext.GetPeople(); // Returns List<Person>

   return people.FirstOrDefault(x => x.ID == id);
}

 public void MyComponentLegacyMethod(List<int> masterCollection)
        {
            //Without the ToList this linq query will be executed twice because of the following usage
            var result = masterCollection.Where(i => i > 100).ToList();
 
            Console.WriteLine(result.Count());
            Console.WriteLine(result.Average());
        }

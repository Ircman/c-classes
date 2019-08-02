//deserilization
internal class ODataResponse<T>
 {
    public List<T> Value { get; set; }
 }
 
 using (var client = new HttpClient())
 {
     HttpResponseMessage response = await client.GetAsync(new Uri(url));
     var json = await response.Content.ReadAsStringAsync();
     var result = JsonConvert.DeserializeObject<ODataResponse<Product>>(json);
     var products = result.Value;
 }
 ///end of deserilezation
    public class DataForExport
    {
            public List<DataFromServer> data = new List<DataFromServer>();
            public struct Summary
            {
                public int Max;

                /// <summary>
                /// Min - minimālā value vērtība
                /// </summary>
                public int Min;

                /// <summary>
                /// Average - vidējā value vērtība, noapaļojot līdz veselam skaitlim
                /// </summary>
                public int Average;

                /// <summary>
                /// Max drop - maksimālā krituma vērtība, iecirkņa nosaukums un grupa
                /// </summary>
                public int Max_drop;

                /// <summary>
                /// Max increase - maksimālā pieauguma vērtība, iecirkņa nosaukums un grupa
                /// </summary>
                public int Max_increase;

                public string group;
            }
            public Summary summary = new Summary();

//serilization
        public void Export(DataForExport inputdata)
        {
            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = @"JSON files (*.JSON)|*.JSON";
                if (sf.ShowDialog()==DialogResult.OK)
                {
                    string path = sf.FileName;
                    JsonSerializer serializer = new JsonSerializer();
                     serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    
                    using (StreamWriter sw = new StreamWriter(path))
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer, inputdata);

                        }


                }
            }
        }

    }

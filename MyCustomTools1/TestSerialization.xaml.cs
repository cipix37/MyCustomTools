using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using static System.Environment;
using static System.IO.Path;

namespace MyCustomTools1
{
    /// <summary>
    /// Interaction logic for TestSerialization.xaml
    /// </summary>
    public class Person
    {
        public Person(decimal initialSalary)
        {
            Salary = initialSalary;
        }
        public Person() { }

        [XmlAttribute("fname")]
        public string FirstName { get; set; }

        [XmlAttribute("lname")]
        public string LastName { get; set; }

        [XmlAttribute("dob")]
        public DateTime DateOfBirth { get; set; }
        public HashSet<Person> Children { get; set; }


        protected decimal Salary { get; set; }
    }

    public partial class TestSerialization : Window
    {
        private List<Person> people;
        public TestSerialization()
        {
            InitializeComponent();
        }


        //write
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            // create object that will format a List of Persons as XML
            var xs = new XmlSerializer(typeof(List<Person>));
            // create a file to write to
            string path = Combine(CurrentDirectory, "people.xml");

            using (FileStream stream = File.Create(path))
            {
                // serialize the object graph to the stream
                xs.Serialize(stream, people);
            }
            tb_output.Text = $"Written {new FileInfo(path).Length} bytes of XML to {path}";
            // Display the serialized object graph
            tb_output.Text += File.ReadAllText(path);
        }

        //read
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // create object that will format a List of Persons as XML
            var xs = new XmlSerializer(typeof(List<Person>));
            // create a file to write to
            string path = Combine(CurrentDirectory, "people.xml");

            using (FileStream xmlLoad = File.Open(path, FileMode.Open))
            {
                // deserialize and cast the object graph into a List of Person
                var loadedPeople = (List<Person>)xs.Deserialize(xmlLoad);
                tb_input.Text = "";
                foreach (var item in loadedPeople)
                {
                    tb_input.Text += $"{item.LastName} has {item.Children.Count} children.\n";
                }
            }
        }

        //set
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            people = new List<Person>
            {
                new Person(30000M) { FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateTime(1974, 3, 14) },
                new Person(40000M) { FirstName = "Bob",
                LastName = "Jones",
                DateOfBirth = new DateTime(1969, 11, 23) },
                new Person(20000M) { FirstName = "Charlie",
                LastName = "Cox",
                DateOfBirth = new DateTime(1984, 5, 4),
                Children = new HashSet<Person>
                { new Person(0M) { FirstName = "Sally",
                LastName = "Cox",
                DateOfBirth = new DateTime(2000, 7, 12) } } }
            };
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("closed");
            using (StreamWriter OutputFile = new StreamWriter("closed.txt", false))
            {
                OutputFile.Write("Closed");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("closing");
            using (StreamWriter OutputFile = new StreamWriter("closing.txt", false))
            {
                OutputFile.Write("closing");
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("unloaded");
            using (StreamWriter OutputFile = new StreamWriter("unloaded.txt", false))
            {
                OutputFile.Write("unloaded");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Linq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }


        List<Student> studentList = new List<Student>() {
    new Student() { StudentID = 1, StudentName = "John", Age = 18, StandardID = 1 } ,
    new Student() { StudentID = 2, StudentName = "Steve",  Age = 21, StandardID = 1 } ,
    new Student() { StudentID = 3, StudentName = "Bill",  Age = 18, StandardID = 2 } ,
    new Student() { StudentID = 4, StudentName = "Ram" , Age = 20, StandardID = 2 } ,
    new Student() { StudentID = 5, StudentName = "Ron" , Age = 21 , StandardID = 2 }};

        List<Standard> standardList = new List<Standard>() {
    new Standard(){ StandardID = 1, StandardName="Standard 1"},
    new Standard(){ StandardID = 2, StandardName="Standard 2"},
    new Standard(){ StandardID = 3, StandardName="Standard 3"}};

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var studentNames = studentList.Where(s => s.Age > 18)
                                .Select(s => s)
                                .Where(st => st.StandardID > 0)
                                .Select(s => s.StudentName);

            int c = studentNames.Count(); // get count of list

            // SORT - OrderBy
            var sortedStudents = from s in studentList
                                 orderby s.StandardID, s.Age
                                 select new
                                 {
                                     StudentName = s.StudentName,
                                     Age = s.Age,
                                     StandardID = s.StandardID
                                 };

            // Left Outer Join
            var studentWithStandard = from s in studentList
                                      join stad in standardList
                                      on s.StandardID equals stad.StandardID
                                      select new
                                      {
                                          StudentName = s.StudentName,
                                          StandardName = stad.StandardName
                                      };

            // Left Outer Join
            var studentsWithStandard = from stad in standardList
                                       join s in studentList
                                       on stad.StandardID equals s.StandardID
                                       into sg
                                       from std_grp in sg
                                       orderby stad.StandardName, std_grp.StudentName
                                       select new
                                       {
                                           StudentName = std_grp.StudentName,
                                           StandardName = stad.StandardName
                                       };
            // Inner join
            var studentWithStandardInner = from s in studentList
                                           join stad in standardList
                                           on s.StandardID equals stad.StandardID
                                           select new
                                           {
                                               StudentName = s.StudentName,
                                               StandardName = stad.StandardName
                                           };
            // Nested Query
            var nestedQueries = from s in studentList
                                where s.Age > 18 && s.StandardID ==
                                    (from std in standardList
                                     where std.StandardName == "Standard 1"
                                     select std.StandardID).FirstOrDefault()
                                select s;

            // ThenBy
            var thenByResult = studentList.OrderBy(s => s.StudentName).ThenBy(s => s.Age);

            // ThenByDescending
            var thenByDescResult = studentList.OrderBy(s => s.StudentName).ThenByDescending(s => s.Age);

            // OrderBy 
            var orderByResult = from s in studentList
                                orderby s.StudentName
                                select s;

            //  orderby descending
            var orderByDescendingResult = from s in studentList
                                          orderby s.StudentName descending
                                          select s;
            // GroupBy 
            var groupedResult = from s in studentList
                                group s by s.Age;



            // list inner join
            var innerJoin = studentList.Join(// outer sequence 
                        standardList,  // inner sequence 
                        student => student.StandardID,    // outerKeySelector
                        standard => standard.StandardID,  // innerKeySelector
                        (student, standard) => new  // result selector
                        {
                            StudentName = student.StudentName,
                            StandardName = standard.StandardName
                        });

            // list group join
            var groupJoin = standardList.GroupJoin(studentList,  //inner sequence
                                std => std.StandardID, //outerKeySelector 
                                s => s.StandardID,     //innerKeySelector
                                (std, studentsGroup) => new // resultSelector 
                                {
                                    Students = studentsGroup,
                                    StandarFulldName = std.StandardName
                                });

            // checks whether all the students are teenagers    
            bool areAllStudentsTeenAger = studentList.All(s => s.Age > 12 && s.Age < 20);

            // Checks if any of the elements in a sequence satisfies the specified condition   
            bool isAnyStudentTeenAger = studentList.Any(s => s.Age > 12 && s.Age < 20);

            List<int> intList = new List<int>() { 1, 2, 3, 4, 5 };

            // checks if list contains given value
            bool result = intList.Contains(10);

            // The Max() method returns the largest numeric element from a collection.
            var oldest = studentList.Max(s => s.Age);            

            // Average extension method calculates the average of the numeric items in the collection.
            var avg = intList.Average();

            // Returns the element at a specified index in a collection
            Console.WriteLine("1st Element in intList: {0}", intList.ElementAt(0));

            List<string> collection1 = new List<string>() { "One", "Two", "Three" };
            List<string> collection2 = new List<string>() { "Five", "Six" };

            // combines 2 collections into a new collection
            var collection3 = collection1.Concat(collection2);




            List<string> strList = new List<string>() { "One", "Two", "Three", "Two", "Three" };   

            // Returns distinct values from a collection.
            var distinctList1 = strList.Distinct();

            List<string> strList1 = new List<string>() { "One", "Two", "Three", "Four", "Five" };
            List<string> strList2 = new List<string>() { "Four", "Five", "Six", "Seven", "Eight" };
            // The Except() method requires two collections. It returns a new collection with elements from the first collection 
            // which do not exist in the second collection (parameter collection).
            var aresult = strList1.Except(strList2);

            // The Intersect extension method requires two collections. It returns a new collection that includes common elements 
            // that exists in both the collection. Consider the following example.
            var eresult = strList1.Intersect(strList2);

            // The Union extension method requires two collections and returns a new collection that includes distinct elements 
            // from both the collections. Consider the following example.
            var fresult = strList1.Union(strList2);

            // Skips elements up to a specified position starting from the first element in a sequence.
            var newList = strList.Skip(2);

            // Skips elements based on a condition until an element does not satisfy the condition. If the first element itself doesn't 
            // satisfy the condition, it then skips 0 elements and returns all the elements in the sequence.
            var resultList = strList.SkipWhile(s => s.Length < 4);

            // The Take() extension method returns the specified number of elements starting from the first element.
            var newListe = strList.Take(2);

            // The TakeWhile() extension method returns elements from the given collection until the specified condition is true. 
            // If the first element itself doesn't satisfy the condition then returns an empty collection.
            var resulta = strList.TakeWhile(s => s.Length > 4);



            /* 
            
            Conversion Operators

             AsEnumerable:	Returns the input sequence as IEnumerable<t>
             AsQueryable :	Converts IEnumerable to IQueryable, to simulate a remote query provider
             Cast        :	Coverts a non-generic collection to a generic collection (IEnumerable to IEnumerable<T>)
             OfType      :	Filters a collection based on a specified type
             ToArray	 :  Converts a collection to an array
             ToDictionary:	Puts elements into a Dictionary based on key selector function
             ToList      :	Converts collection to List
             ToLookup    :	Groups elements into an Lookup<TKey,TElement>
             */


        }



    }
}

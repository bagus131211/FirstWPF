using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UX.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestPropertyChanged();
            Console.WriteLine("----------------------------");
            TestPropertyChangedFody();
            Console.WriteLine("----------------------------");
            Console.ReadKey();
        }

        static void TestPropertyChanged()
        {
            var test = new Test();
            test.PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"Property {e.PropertyName} has changed. New value is [{test.SomeProperty}].");
            };
            test.SomeProperty = "Hello World";
            test.SomeProperty = "hello world";
            test.SomeProperty = "hello world";
            test.SomeProperty = "hello World again";

        }

        static void TestPropertyChangedFody()
        {
            var test = new TestFody();
            test.PropertyChanged += (s, e) =>
            {
                Console.WriteLine($"Property {e.PropertyName} has changed.");
            };
            test.SomeProperty = "Hello World";
            test.SomeProperty = "hello world";
            test.SomeProperty = "hello world";
            test.SomeProperty = "hello World again";
            test.SecretProperty = true;
            test.SecretProperty = false;
        }
    }
}

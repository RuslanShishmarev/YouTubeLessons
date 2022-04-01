using System;

namespace Lesson_3_DataType
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass test1 = new TestClass();

            test1.Age = 10;

            TestClass test2 = test1;

            test2.Age = 20;

            TestStruct testStruct1 = new TestStruct();
            testStruct1.Age = 30;

            //запаковка
            object packed = (object)testStruct1;

            testStruct1.Age = 100;

            TestStruct unpacked = (TestStruct)packed;

            unpacked.Age = 200;

            Console.WriteLine(unpacked.Age);
        }
    }

    class TestClass
    {
        public int Age { get; set; } = 50;

        public void ToDo()
        {
            int number = 60;
        }
    }

    class TestClass1
    {
        public int Level { get; set; } = 50;
    }

    struct TestStruct
    {
        public int Age { get; set; }
    }

    struct TestStruct1
    {
        public int Level { get; set; }
    }
}

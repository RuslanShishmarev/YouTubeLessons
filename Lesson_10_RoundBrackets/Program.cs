/* Дано число N, 
нужно сгенерировать все правильные скобочные последовательности
из N открывающих и N закрывающих скобок.*/

int n = Convert.ToInt32(Console.ReadLine());
var result = Generate(n);

foreach (var brackets in result)
{
    Console.WriteLine(brackets);
}

List<string> Generate(int n)
{
    var result = new List<string>();

    GenerateCurrent(n, n, "", result);

    return result;
}

void GenerateCurrent(int left, int right, string current, List<string> resultCommon)
{
    if (left == 0 && right == 0)
    {
        resultCommon.Add(current);
        return;
    }

    if (left > 0)
    {
        GenerateCurrent(left - 1, right, current + '(', resultCommon);
    }

    if (left < right)
    {
        GenerateCurrent(left, right - 1, current + ')', resultCommon);
    }
}

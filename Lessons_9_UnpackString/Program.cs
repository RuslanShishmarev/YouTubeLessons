using System.Text;

/*Рассмотрим простой алгоритм сжатия строки. 
Если в строке есть несколько подряд идущих одинаковых подстрок, можно заменить их на группу. 
Например, строку aabaabaab можно записать как 3(aab). 
Можно алгоритм применить к сжатой строке, получив вложенные группы: 3(2ab). 
Дана сжатая строка, требуется её распаковать. 
Например, для строки a2(a2(bc))3db ответ будет aabcbcabcbcdddb.*/

string input1 = "a2(a2(bc))3db";
string check1 = "aabcbcabcbcdddb";

string input2 = "aa2(bc)db";
string check2 = "aabcbcdb";

string input3 = "4w2o3(3(q))";
string check3 = "wwwwooqqqqqqqqq";

var result1 = GetUnpackStr(new StringBuilder(input1));
var result2 = GetUnpackStr(new StringBuilder(input2));
var result3 = GetUnpackStr(new StringBuilder(input3));

Console.WriteLine(result1);
Console.WriteLine(check1 == result1.ToString());

Console.WriteLine(result2);
Console.WriteLine(check2 == result2.ToString());

Console.WriteLine(result3);
Console.WriteLine(check3 == result3.ToString());

StringBuilder GetUnpackStr(StringBuilder input)
{
    var resultStr = new StringBuilder();
    for (int i = 0; i < input.Length; i++)
    {
        char sb = input[i];

        int sbAsInt;
        if (int.TryParse(sb.ToString(), out sbAsInt))
        {
            StringBuilder result = new StringBuilder();

            char nextSb = input[i + 1];
            if (nextSb == '(')
            {
                var forBrace = input.ToString()[(i + 1)..(input.Length - 1)];
                var innerBrace = GetInnerBraceStr(forBrace);

                result = GetUnpackStr(innerBrace);

                i += innerBrace.Length + 2;
            }
            else
            {
                result = result.Append(nextSb);
                i++;
            }
            for (int charCount = 0; charCount < sbAsInt; charCount++)
            {
                resultStr.Append(result);
            }
        }
        else
        {
            resultStr.Append(sb);
        }
    }

    return resultStr;
}

StringBuilder GetInnerBraceStr(string input)
{
    var resultStr = new StringBuilder();
    int openCount = 1;
    int closeCount = 0;
    for (int i = 1; i < input.Length; i++)
    {
        char sb = input[i];

        if (sb == '(')
        {
            openCount++;
        }
        if (sb == ')')
        {
            closeCount++;
        }

        if (closeCount < openCount)
        {
            resultStr.Append(sb);
        }

        if (openCount == closeCount)
        {
            return resultStr;
        }
    }
    return resultStr;
}

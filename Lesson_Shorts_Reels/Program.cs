// See https://aka.ms/new-console-template for more information

//Перевернуть каждое слово в предложении,
//сохранив порядок без использования сторонних библиотек
//съешь же ещё этих мягких французских булок, да выпей чаю

string input = "съешь же ещё этих мягких французских булок, да выпей чаю";

var reverse = (string text) =>
{
    string result = string.Empty;

	for (int i = text.Length - 1; i >= 0; i--)
	{
        result += text[i];
    }

    return result;
};

var splitText = (string text) =>
{
    text += " ";
    string wordHelp = string.Empty;
    var result = new List<string>();

    for (int i = 0; i < text.Length; i++)
    {
        wordHelp += text[i];
        if (text[i] == ' ')
        {
            result.Add(wordHelp);
            wordHelp = string.Empty;
        }
    }

    return result;
};

string resultProg = string.Empty;

var textAsArray = splitText(input);

foreach (string word in textAsArray)
{
    resultProg += reverse(word); 
}


Console.WriteLine(resultProg);
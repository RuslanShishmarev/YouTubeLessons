// See https://aka.ms/new-console-template for more information

using SimpleWebFramework.Main.WebFramework;

Console.WriteLine("Hello, Web Framework!");

string host = "http://localhost:8080/";

HttpServerListener httpServerListener = new(host);
await httpServerListener.Start();

Console.WriteLine("Сервер запущен...");

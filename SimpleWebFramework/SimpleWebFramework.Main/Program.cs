// See https://aka.ms/new-console-template for more information

using SimpleWebFramework.Main.Services;
using SimpleWebFramework.Main.Services.Interfaces;
using SimpleWebFramework.Lib;
using SimpleWebFramework.Lib.Models;

Console.WriteLine("Hello, Web Framework!");

string host = "http://localhost:8080/";

HttpServerListener httpServerListener = new(host, "MainPage.html");
httpServerListener.Add<IUserService, UserService>(LifeTime.Scoped);
httpServerListener.Add<ILoggingTest, LoggingTest>(LifeTime.Transient);
await httpServerListener.Start();

Console.WriteLine("Сервер запущен...");

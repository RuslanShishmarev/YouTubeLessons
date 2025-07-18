using SimpleWebFramework.Main.Models;
using SimpleWebFramework.Lib.Models;
using SimpleWebFramework.Lib.Models.Attributes;

namespace SimpleWebFramework.Main.Controllers;

[ApiController("Test")]
public class TestController : BaseController
{
    public string GetUserTest() => "{" +
        "\"name\": \"Ruslan\"," +
        "\"age\": 29" +
        "}";

    [HttpGet("Job")]
    public ActionResult GetJobTest() => new ActionResult(
        ResponseType: HttpResponseType.JSON,
        Result: "{" +
        "\"position\": \"developer\"," +
        "\"salary\": 100" +
        "}",
        Status: 200);

    [HttpGet]
    public ActionResult GetTestPage()
    {
        var pageContent = GetHTMLPage("TestData.html", new Dictionary<string, object>()
        {
            { "title", "Test Page" },
            { "description", "This is a test page for the SimpleWebFramework." },
            { "list", new string[]{"Пока", "Привет", "До свидания" } }
        });
        return new ActionResult(HttpResponseType.HTML, pageContent, 200);
    }

    [HttpPost("TestPost")]
    public ActionResult PostTestPage(string data)
    {
        var responseContent = $"Received data: {data}";
        return new ActionResult(HttpResponseType.JSON, responseContent, 200);
    }

    [HttpPost("TestPostWithBody")]
    public ActionResult PostTestWithBody([FromBody]string data)
    {
        var responseContent = $"\"Received data\": \"{data}\"";
        return new ActionResult(HttpResponseType.JSON, responseContent, 200);
    }

    [HttpPost("TestPostWithBodyAndParam")]
    public ActionResult PostTestWithBody([FromBody] User data, int i, double second)
    {
        var responseContent = $"\"Received data\": \"{data}\", \"i\": {i} \"second\": {second}";
        return new ActionResult(HttpResponseType.JSON, responseContent, 200);
    }

    [HttpGet("Pattern/{id}")]
    public ActionResult GetByPattern1(int id)
    {
        var responseContent = $"\"Received data\": \"{id}\"";
        return new ActionResult(HttpResponseType.JSON, responseContent, 200);
    }

    [HttpGet("Pattern/{id}/Name/{name}")]
    public ActionResult GetByPattern3(int id, string name, string data)
    {
        var responseContent = $"\"Received data\": \"{id}\", \"{name}\", {data}";
        return new ActionResult(HttpResponseType.JSON, responseContent, 200);
    }
}

namespace SimpleWebFramework.Main.WebFramework.Models;

public record ActionResult(
    HttpResponseType ResponseType,
    object Result,
    int Status);

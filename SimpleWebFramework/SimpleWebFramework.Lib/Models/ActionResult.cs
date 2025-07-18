namespace SimpleWebFramework.Lib.Models;

public record ActionResult(
    HttpResponseType ResponseType,
    object Result,
    int Status);

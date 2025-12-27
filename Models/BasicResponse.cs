using System;

namespace eMeterApi.Models;

public class BasicResponse<T>
{
    public string? Title {get;set;}
    public T? Value {get;set;}
    public string? Message {get;set;}
}

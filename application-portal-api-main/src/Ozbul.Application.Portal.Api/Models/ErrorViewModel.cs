using System;
namespace Ozbul.Application.Portal.Api.Models;

public class ErrorViewModel
{
    public bool Error = true;

    public string Message { get; set; }

    public ErrorViewModel(string message)
    {
        Message = message;
    }

    public override string ToString() => $"{nameof(Error)}: {Error}, {nameof(Message)}: {Message}";
}

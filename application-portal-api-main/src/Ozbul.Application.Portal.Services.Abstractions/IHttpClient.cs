using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozbul.Application.Portal.Services.Abstractions
{
    public interface IHttpClient
    {
        Task<System.Net.HttpStatusCode> ReturnResponseAsync(string username);
    }
}

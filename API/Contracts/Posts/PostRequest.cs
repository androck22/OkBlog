using System;

namespace API.Contracts
{
    public class PostRequest
    {
        public string Title { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string Author { get; set; } = String.Empty;
    }
}

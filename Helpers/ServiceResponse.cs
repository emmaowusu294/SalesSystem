namespace SalesSystem.Helpers
{
    // This is your OLD class. It's fine.
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation successful.";
    }

    // --- ADD THIS NEW CLASS ---
    // This is a "generic" version that lets us also
    // return an object (like our new Sale)
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }
    }
}
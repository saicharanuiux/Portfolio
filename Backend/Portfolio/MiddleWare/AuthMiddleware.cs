public class AuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Inspect the Incoming Header
        var authHeader = context.Request.Headers["Authorization"].ToString();
        Console.WriteLine($"--- Incoming Request to: {context.Request.Path} ---");
        Console.WriteLine($"Auth Header: {(string.IsNullOrEmpty(authHeader) ? "MISSING" : authHeader)}");

        await next(context);

        // 2. Inspect the Resulting User (After Authentication middleware has run)
        var user = context.User;
        if (user.Identity?.IsAuthenticated ?? false)
        {
            Console.WriteLine($"User authenticated: {user.Identity.Name}");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
        }
        else
        {
            Console.WriteLine("User NOT authenticated after processing middleware.");
            if (context.Response.StatusCode == 401)
            {
                Console.WriteLine("Response returned 401 Unauthorized.");
            }
        }
        Console.WriteLine("--------------------------------------------");
    }
}
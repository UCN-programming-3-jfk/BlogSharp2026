using BlogSharp2026.DataAccess.Model;
using RestSharp2026.ApiClient;

namespace BlogSharp2026.ApiClientConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║   BlogSharp2026 API Client Test Suite  ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();

        const string authorsApiUrl = "https://localhost:7113/api/v1/authors";
        const string blogPostsApiUrl = "https://localhost:7113/api/v1/blogposts";

        bool authorsTestsPassed = false;
        bool blogPostsTestsPassed = false;

        try
        {
            // Run Authors API tests
            var authorsTests = new AuthorsTests(authorsApiUrl);
            authorsTestsPassed = authorsTests.RunTests();

            // Run Blog Posts API tests
            var blogPostsTests = new BlogPostsTests(blogPostsApiUrl, authorsApiUrl);
            blogPostsTestsPassed = blogPostsTests.RunTests();

            // Final Summary
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║         TEST SUITE SUMMARY              ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine($"Authors API Tests:    {(authorsTestsPassed ? "✓ PASSED" : "✗ FAILED")}");
            Console.WriteLine($"Blog Posts API Tests: {(blogPostsTestsPassed ? "✓ PASSED" : "✗ FAILED")}");
            Console.WriteLine();

            if (authorsTestsPassed && blogPostsTestsPassed)
            {
                Console.WriteLine("🎉 All tests passed successfully!");
            }
            else
            {
                Console.WriteLine("⚠ Some tests failed. Review the output above.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[FATAL ERROR] Test suite crashed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadLine();
    }
}


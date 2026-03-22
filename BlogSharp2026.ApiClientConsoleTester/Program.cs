using BlogSharp2026.DataAccess.Model;
using RestSharp2026.ApiClient;

namespace BlogSharp2026.ApiClientConsoleTester;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("AuthorsApiClient Test Suite");
        Console.WriteLine("========================================\n");

        AuthorsApiClient client = new AuthorsApiClient("https://localhost:7113/api/v1/authors");

        try
        {
            // Test 1: Get All Authors
            TestGetAllAuthors(client);

            // Test 2: Insert New Author
            int newAuthorId = TestInsertAuthor(client);

            // Test 3: Get Author By ID
            TestGetAuthorById(client, newAuthorId);

            // Test 4: Update Author
            TestUpdateAuthor(client, newAuthorId);

            // Test 5: Get Updated Author
            TestGetAuthorById(client, newAuthorId);

            // Test 6: Delete Author
            TestDeleteAuthor(client, newAuthorId);

            // Test 7: Verify Deletion (should return null)
            TestGetAuthorByIdAfterDeletion(client, newAuthorId);

            Console.WriteLine("\n========================================");
            Console.WriteLine("All tests completed successfully!");
            Console.WriteLine("========================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR] Test suite failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadLine();
    }

    static void TestGetAllAuthors(AuthorsApiClient client)
    {
        Console.WriteLine("[TEST 1] Get All Authors");
        Console.WriteLine("----------------------------------------");

        var authors = client.GetAllAuthors();
        var authorsList = authors.ToList();

        Console.WriteLine($"Found {authorsList.Count} authors:");
        foreach (var author in authorsList)
        {
            Console.WriteLine($"  - {author}");
        }

        Console.WriteLine($"✓ Test passed\n");
    }

    static int TestInsertAuthor(AuthorsApiClient client)
    {
        Console.WriteLine("[TEST 2] Insert New Author");
        Console.WriteLine("----------------------------------------");

        var newAuthor = new Author
        {
            Email = $"testuser_{Guid.NewGuid().ToString().Substring(0, 8)}@example.com",
            BlogTitle = $"Test Blog {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            PasswordHash = "TestPassword123!"
        };

        Console.WriteLine($"Inserting author: {newAuthor.Email}, {newAuthor.BlogTitle}");

        int newId = client.InsertAuthor(newAuthor);

        Console.WriteLine($"✓ Author inserted successfully with ID: {newId}\n");

        return newId;
    }

    static void TestGetAuthorById(AuthorsApiClient client, int id)
    {
        Console.WriteLine($"[TEST 3] Get Author By ID ({id})");
        Console.WriteLine("----------------------------------------");

        var author = client.GetAuthorById(id);

        if (author != null)
        {
            Console.WriteLine($"Found author: {author}");
            Console.WriteLine($"✓ Test passed\n");
        }
        else
        {
            throw new Exception($"Author with ID {id} not found!");
        }
    }

    static void TestUpdateAuthor(AuthorsApiClient client, int id)
    {
        Console.WriteLine($"[TEST 4] Update Author ({id})");
        Console.WriteLine("----------------------------------------");

        var author = client.GetAuthorById(id);

        if (author == null)
        {
            throw new Exception($"Cannot update - author with ID {id} not found!");
        }

        Console.WriteLine($"Before update: {author}");

        author.BlogTitle = $"Updated Blog Title {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        author.Email = $"updated_{author.Email}";

        bool result = client.UpdateAuthor(author);

        if (result)
        {
            Console.WriteLine($"After update: Email={author.Email}, BlogTitle={author.BlogTitle}");
            Console.WriteLine($"✓ Author updated successfully\n");
        }
        else
        {
            throw new Exception("Update failed!");
        }
    }

    static void TestDeleteAuthor(AuthorsApiClient client, int id)
    {
        Console.WriteLine($"[TEST 6] Delete Author ({id})");
        Console.WriteLine("----------------------------------------");

        bool result = client.DeleteAuthor(id);

        if (result)
        {
            Console.WriteLine($"✓ Author with ID {id} deleted successfully\n");
        }
        else
        {
            throw new Exception($"Failed to delete author with ID {id}");
        }
    }

    static void TestGetAuthorByIdAfterDeletion(AuthorsApiClient client, int id)
    {
        Console.WriteLine($"[TEST 7] Verify Author Deletion ({id})");
        Console.WriteLine("----------------------------------------");

        var author = client.GetAuthorById(id);

        if (author == null)
        {
            Console.WriteLine($"✓ Confirmed: Author with ID {id} no longer exists\n");
        }
        else
        {
            throw new Exception($"Author with ID {id} still exists after deletion!");
        }
    }
}

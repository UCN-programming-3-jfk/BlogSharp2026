using BlogSharp2026.DataAccess.Model;
using RestSharp2026.ApiClient;

namespace BlogSharp2026.ApiClientConsoleTester;

public class AuthorsTests
{
    private readonly AuthorsApiClient _client;

    public AuthorsTests(string apiUrl)
    {
        _client = new AuthorsApiClient(apiUrl);
    }

    public bool RunTests()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("AUTHORS API CLIENT TESTS");
        Console.WriteLine("========================================\n");

        try
        {
            // Test 1: Get All Authors
            TestGetAllAuthors();

            // Test 2: Insert New Author
            int newAuthorId = TestInsertAuthor();

            // Test 3: Get Author By ID
            TestGetAuthorById(newAuthorId);

            // Test 4: Update Author
            TestUpdateAuthor(newAuthorId);

            // Test 5: Get Updated Author
            TestGetAuthorById(newAuthorId);

            // Test 6: Delete Author
            TestDeleteAuthor(newAuthorId);

            // Test 7: Verify Deletion (should return null)
            TestGetAuthorByIdAfterDeletion(newAuthorId);

            Console.WriteLine("========================================");
            Console.WriteLine("✓ All Authors API tests passed!");
            Console.WriteLine("========================================\n");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR] Authors API test failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("========================================\n");
            return false;
        }
    }

    private void TestGetAllAuthors()
    {
        Console.WriteLine("[TEST 1] Get All Authors");
        Console.WriteLine("----------------------------------------");

        var authors = _client.GetAllAuthors();
        var authorsList = authors.ToList();

        Console.WriteLine($"Found {authorsList.Count} authors:");
        foreach (var author in authorsList.Take(3))
        {
            Console.WriteLine($"  - {author}");
        }
        if (authorsList.Count > 3)
        {
            Console.WriteLine($"  ... and {authorsList.Count - 3} more");
        }

        Console.WriteLine($"✓ Test passed\n");
    }

    private int TestInsertAuthor()
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

        int newId = _client.InsertAuthor(newAuthor);

        Console.WriteLine($"✓ Author inserted successfully with ID: {newId}\n");

        return newId;
    }

    private void TestGetAuthorById(int id)
    {
        Console.WriteLine($"[TEST 3] Get Author By ID ({id})");
        Console.WriteLine("----------------------------------------");

        var author = _client.GetAuthorById(id);

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

    private void TestUpdateAuthor(int id)
    {
        Console.WriteLine($"[TEST 4] Update Author ({id})");
        Console.WriteLine("----------------------------------------");

        var author = _client.GetAuthorById(id);

        if (author == null)
        {
            throw new Exception($"Cannot update - author with ID {id} not found!");
        }

        Console.WriteLine($"Before update: {author}");

        author.BlogTitle = $"Updated Blog Title {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        author.Email = $"updated_{author.Email}";

        bool result = _client.UpdateAuthor(author);

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

    private void TestDeleteAuthor(int id)
    {
        Console.WriteLine($"[TEST 6] Delete Author ({id})");
        Console.WriteLine("----------------------------------------");

        bool result = _client.DeleteAuthor(id);

        if (result)
        {
            Console.WriteLine($"✓ Author with ID {id} deleted successfully\n");
        }
        else
        {
            throw new Exception($"Failed to delete author with ID {id}");
        }
    }

    private void TestGetAuthorByIdAfterDeletion(int id)
    {
        Console.WriteLine($"[TEST 7] Verify Author Deletion ({id})");
        Console.WriteLine("----------------------------------------");

        var author = _client.GetAuthorById(id);

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

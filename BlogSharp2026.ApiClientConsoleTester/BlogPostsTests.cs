using BlogSharp2026.DataAccess.Model;
using RestSharp2026.ApiClient;

namespace BlogSharp2026.ApiClientConsoleTester;

public class BlogPostsTests
{
    private readonly BlogPostsApiClient _client;
    private readonly AuthorsApiClient _authorsClient;
    private int _testAuthorId;

    public BlogPostsTests(string apiUrl, string authorsApiUrl)
    {
        _client = new BlogPostsApiClient(apiUrl);
        _authorsClient = new AuthorsApiClient(authorsApiUrl);
    }

    public bool RunTests()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("BLOG POSTS API CLIENT TESTS");
        Console.WriteLine("========================================\n");

        try
        {
            // Setup: Create a test author for blog posts
            _testAuthorId = SetupTestAuthor();

            // Test 1: Get All Blog Posts
            TestGetAllBlogPosts();

            // Test 2: Insert New Blog Post
            int newBlogPostId = TestInsertBlogPost(_testAuthorId);

            // Test 3: Get Blog Post By ID
            TestGetBlogPostById(newBlogPostId);

            // Test 4: Get Blog Posts By Author ID
            TestGetBlogPostsByAuthorId(_testAuthorId);

            // Test 5: Insert Multiple Blog Posts (for latest test)
            var blogPostIds = TestInsertMultipleBlogPosts(_testAuthorId);

            // Test 6: Get Latest Blog Posts (across all authors)
            TestGetLatestBlogPosts(3);

            // Test 7: Update Blog Post
            TestUpdateBlogPost(newBlogPostId);

            // Test 8: Delete Blog Posts
            foreach (var id in blogPostIds)
            {
                TestDeleteBlogPost(id);
            }
            TestDeleteBlogPost(newBlogPostId);

            // Test 9: Verify Deletion
            TestGetBlogPostByIdAfterDeletion(newBlogPostId);

            // Cleanup: Delete test author
            CleanupTestAuthor();

            Console.WriteLine("========================================");
            Console.WriteLine("✓ All Blog Posts API tests passed!");
            Console.WriteLine("========================================\n");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR] Blog Posts API test failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("========================================\n");

            // Attempt cleanup even on failure
            try
            {
                if (_testAuthorId > 0)
                {
                    CleanupTestAuthor();
                }
            }
            catch { }

            return false;
        }
    }

    private int SetupTestAuthor()
    {
        Console.WriteLine("[SETUP] Creating test author for blog posts tests");
        Console.WriteLine("----------------------------------------");

        var testAuthor = new Author
        {
            Email = $"blogtest_{Guid.NewGuid().ToString().Substring(0, 8)}@example.com",
            BlogTitle = $"Blog Test Author {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            PasswordHash = "TestPassword123!"
        };

        int authorId = _authorsClient.InsertAuthor(testAuthor);
        Console.WriteLine($"✓ Test author created with ID: {authorId}\n");

        return authorId;
    }

    private void CleanupTestAuthor()
    {
        Console.WriteLine("[CLEANUP] Deleting test author");
        _authorsClient.DeleteAuthor(_testAuthorId);
        Console.WriteLine("✓ Test author deleted\n");
    }

    private void TestGetAllBlogPosts()
    {
        Console.WriteLine("[TEST 1] Get All Blog Posts");
        Console.WriteLine("----------------------------------------");

        var blogPosts = _client.GetAllBlogPosts();
        var blogPostsList = blogPosts.ToList();

        Console.WriteLine($"Found {blogPostsList.Count} blog posts:");
        foreach (var post in blogPostsList.Take(3))
        {
            Console.WriteLine($"  - [{post.Id}] {post.PostTitle}");
        }
        if (blogPostsList.Count > 3)
        {
            Console.WriteLine($"  ... and {blogPostsList.Count - 3} more");
        }

        Console.WriteLine($"✓ Test passed\n");
    }

    private int TestInsertBlogPost(int authorId)
    {
        Console.WriteLine("[TEST 2] Insert New Blog Post");
        Console.WriteLine("----------------------------------------");

        var newBlogPost = new BlogPost
        {
            FK_Author_Id = authorId,
            PostTitle = $"Test Post {Guid.NewGuid().ToString().Substring(0, 8)}",
            PostContent = "This is a test blog post content created by the API client test suite.",
            CreationDate = DateTime.Now
        };

        Console.WriteLine($"Inserting blog post: {newBlogPost.PostTitle}");

        int newId = _client.InsertBlogPost(newBlogPost);

        Console.WriteLine($"✓ Blog post inserted successfully with ID: {newId}\n");

        return newId;
    }

    private List<int> TestInsertMultipleBlogPosts(int authorId)
    {
        Console.WriteLine("[TEST 5] Insert Multiple Blog Posts");
        Console.WriteLine("----------------------------------------");

        var ids = new List<int>();
        var timestamps = new[] { -60, -30, -10, -2 }; // days ago

        foreach (var daysAgo in timestamps)
        {
            var blogPost = new BlogPost
            {
                FK_Author_Id = authorId,
                PostTitle = $"Test Post {Guid.NewGuid().ToString().Substring(0, 6)} ({Math.Abs(daysAgo)}d ago)",
                PostContent = $"Content created {Math.Abs(daysAgo)} days ago.",
                CreationDate = DateTime.Now.AddDays(daysAgo)
            };

            int newId = _client.InsertBlogPost(blogPost);
            ids.Add(newId);
            Console.WriteLine($"  - Inserted post ID {newId} with date {blogPost.CreationDate:yyyy-MM-dd}");
        }

        Console.WriteLine($"✓ Inserted {ids.Count} blog posts\n");

        return ids;
    }

    private void TestGetBlogPostById(int id)
    {
        Console.WriteLine($"[TEST 3] Get Blog Post By ID ({id})");
        Console.WriteLine("----------------------------------------");

        var blogPost = _client.GetBlogPostById(id);

        if (blogPost != null)
        {
            Console.WriteLine($"Found blog post: [{blogPost.Id}] {blogPost.PostTitle}");
            Console.WriteLine($"  Author ID: {blogPost.FK_Author_Id}, Created: {blogPost.CreationDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"✓ Test passed\n");
        }
        else
        {
            throw new Exception($"Blog post with ID {id} not found!");
        }
    }

    private void TestGetBlogPostsByAuthorId(int authorId)
    {
        Console.WriteLine($"[TEST 4] Get Blog Posts By Author ID ({authorId})");
        Console.WriteLine("----------------------------------------");

        var blogPosts = _client.GetBlogPostsByAuthorId(authorId);
        var blogPostsList = blogPosts.ToList();

        Console.WriteLine($"Found {blogPostsList.Count} blog posts for author {authorId}:");
        foreach (var post in blogPostsList)
        {
            Console.WriteLine($"  - [{post.Id}] {post.PostTitle} ({post.CreationDate:yyyy-MM-dd})");
        }

        if (blogPostsList.Count == 0)
        {
            throw new Exception($"Expected at least 1 blog post for author {authorId}");
        }

        Console.WriteLine($"✓ Test passed\n");
    }

    private void TestGetLatestBlogPosts(int count)
    {
        Console.WriteLine($"[TEST 6] Get Latest {count} Blog Posts (Across All Authors)");
        Console.WriteLine("----------------------------------------");

        // Use GetAllBlogPosts and manually filter/order since we want latest across all authors
        var allBlogPosts = _client.GetAllBlogPosts();
        var latestBlogPosts = allBlogPosts
            .OrderByDescending(bp => bp.CreationDate)
            .Take(count)
            .ToList();

        Console.WriteLine($"Retrieved {latestBlogPosts.Count} latest blog posts:");

        DateTime? previousDate = null;
        foreach (var post in latestBlogPosts)
        {
            Console.WriteLine($"  - [{post.Id}] {post.PostTitle} (Author: {post.FK_Author_Id}, Date: {post.CreationDate:yyyy-MM-dd HH:mm:ss})");

            // Verify ordering (most recent first)
            if (previousDate.HasValue && post.CreationDate > previousDate.Value)
            {
                throw new Exception($"Blog posts are not ordered by date (descending)!");
            }
            previousDate = post.CreationDate;
        }

        if (latestBlogPosts.Count > count)
        {
            throw new Exception($"Expected {count} posts but got {latestBlogPosts.Count}");
        }

        Console.WriteLine($"✓ Posts are correctly ordered by date (most recent first)");
        Console.WriteLine($"✓ Test passed\n");
    }

    private void TestUpdateBlogPost(int id)
    {
        Console.WriteLine($"[TEST 7] Update Blog Post ({id})");
        Console.WriteLine("----------------------------------------");

        var blogPost = _client.GetBlogPostById(id);

        if (blogPost == null)
        {
            throw new Exception($"Cannot update - blog post with ID {id} not found!");
        }

        Console.WriteLine($"Before update: {blogPost.PostTitle}");

        blogPost.PostTitle = $"Updated Title {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        blogPost.PostContent = "This content has been updated by the test suite.";

        bool result = _client.UpdateBlogPost(blogPost);

        if (result)
        {
            Console.WriteLine($"After update: {blogPost.PostTitle}");
            Console.WriteLine($"✓ Blog post updated successfully\n");
        }
        else
        {
            throw new Exception("Update failed!");
        }
    }

    private void TestDeleteBlogPost(int id)
    {
        Console.WriteLine($"[TEST 8] Delete Blog Post ({id})");
        Console.WriteLine("----------------------------------------");

        bool result = _client.DeleteBlogPost(id);

        if (result)
        {
            Console.WriteLine($"✓ Blog post with ID {id} deleted successfully\n");
        }
        else
        {
            throw new Exception($"Failed to delete blog post with ID {id}");
        }
    }

    private void TestGetBlogPostByIdAfterDeletion(int id)
    {
        Console.WriteLine($"[TEST 9] Verify Blog Post Deletion ({id})");
        Console.WriteLine("----------------------------------------");

        var blogPost = _client.GetBlogPostById(id);

        if (blogPost == null)
        {
            Console.WriteLine($"✓ Confirmed: Blog post with ID {id} no longer exists\n");
        }
        else
        {
            throw new Exception($"Blog post with ID {id} still exists after deletion!");
        }
    }
}

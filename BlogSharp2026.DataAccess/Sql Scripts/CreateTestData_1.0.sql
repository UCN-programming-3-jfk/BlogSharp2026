USE [BlogSharp2026]
GO

-- =======================================================
-- CreateTestData_1.0.sql
-- Creates 5 authors with distinct blog themes
-- Each author has 5-8 blog posts distributed over 3 years
-- =======================================================

-- Clear existing data (if any)
DELETE FROM [dbo].[BlogPost];
DELETE FROM [dbo].[Author];
DBCC CHECKIDENT ('Author', RESEED, 0);
DBCC CHECKIDENT ('BlogPost', RESEED, 0);
GO

-- =======================================================
-- INSERT AUTHORS
-- Note: Password for all test users is "Password123!"
-- Hash generated using BCrypt with default work factor
-- =======================================================

INSERT INTO [dbo].[Author] ([Email], [BlogTitle], [PasswordHash])
VALUES 
    ('alex.techguru@example.com', 'Tech Talk with Alex', '$2a$11$ZvXQJhqP0FQYGmF8qQ6F.OLQmxH9rKVxJQmQJPKYqGJ5FvxYqJNdW'),
    ('sarah.chef@example.com', 'Sarahs Kitchen Chronicles', '$2a$11$ZvXQJhqP0FQYGmF8qQ6F.OLQmxH9rKVxJQmQJPKYqGJ5FvxYqJNdW'),
    ('mike.traveler@example.com', 'Wanderlust with Mike', '$2a$11$ZvXQJhqP0FQYGmF8qQ6F.OLQmxH9rKVxJQmQJPKYqGJ5FvxYqJNdW'),
    ('emma.fitness@example.com', 'Fit Life by Emma', '$2a$11$ZvXQJhqP0FQYGmF8qQ6F.OLQmxH9rKVxJQmQJPKYqGJ5FvxYqJNdW'),
    ('david.finance@example.com', 'Money Matters with David', '$2a$11$ZvXQJhqP0FQYGmF8qQ6F.OLQmxH9rKVxJQmQJPKYqGJ5FvxYqJNdW');
GO

-- =======================================================
-- TECH TALK WITH ALEX - Blog Posts
-- =======================================================

-- Author ID: 1 (Alex - Tech Blog)
INSERT INTO [dbo].[BlogPost] ([FK_Author_Id], [PostTitle], [PostContent], [CreationDate])
VALUES
    (1, 'Getting Started with .NET 8', 
     'The release of .NET 8 brings exciting new features including improved performance, Native AOT compilation, and enhanced cloud-native capabilities. In this post, we''ll explore the key highlights and how to migrate your existing projects to take advantage of these improvements.',
     DATEADD(MONTH, -36, GETDATE())),
    
    (1, 'Understanding Async Await in C#',
     'Asynchronous programming can be confusing for beginners, but it''s essential for building responsive applications. This comprehensive guide breaks down async/await patterns, common pitfalls, and best practices for writing efficient asynchronous code in C#.',
     DATEADD(MONTH, -28, GETDATE())),
    
    (1, 'Microservices Architecture Best Practices',
     'Microservices have revolutionized how we build scalable applications. Learn about service decomposition, API gateway patterns, distributed tracing, and how to implement resilient communication between services using modern .NET technologies.',
     DATEADD(MONTH, -20, GETDATE())),
    
    (1, 'Building RESTful APIs with ASP.NET Core',
     'RESTful APIs are the backbone of modern web applications. This tutorial covers everything from controller design, proper HTTP status codes, versioning strategies, to implementing authentication and authorization in your ASP.NET Core APIs.',
     DATEADD(MONTH, -14, GETDATE())),
    
    (1, 'Docker Containers for .NET Developers',
     'Containerization has become essential for modern application deployment. This guide walks through creating Dockerfiles for .NET applications, multi-stage builds, docker-compose for local development, and deploying to Kubernetes clusters.',
     DATEADD(MONTH, -9, GETDATE())),
    
    (1, 'Entity Framework Core Performance Tips',
     'Entity Framework Core is powerful but can be slow if not used correctly. Discover techniques like query splitting, compiled queries, no-tracking queries, and proper indexing strategies to make your EF Core applications blazingly fast.',
     DATEADD(MONTH, -5, GETDATE())),
    
    (1, 'CI CD Pipelines with GitHub Actions',
     'Automating your build and deployment process is crucial for modern development. Learn how to set up GitHub Actions workflows for .NET projects, including automated testing, code coverage reports, and deployment to Azure App Service.',
     DATEADD(MONTH, -2, GETDATE()));
GO

-- =======================================================
-- SARAH'S KITCHEN CHRONICLES - Blog Posts
-- =======================================================

-- Author ID: 2 (Sarah - Recipe Blog)
INSERT INTO [dbo].[BlogPost] ([FK_Author_Id], [PostTitle], [PostContent], [CreationDate])
VALUES
    (2, 'Classic Italian Carbonara Recipe',
     'Authentic carbonara is simple yet requires perfect technique. Using only eggs, pecorino romano, guanciale, and black pepper, this Roman classic delivers creamy perfection without cream. Learn the secrets to emulsifying the eggs perfectly without scrambling them.',
     DATEADD(MONTH, -34, GETDATE())),
    
    (2, 'Mastering Sourdough Bread at Home',
     'Baking sourdough bread is both an art and a science. From creating your starter to achieving that perfect crispy crust and airy crumb, this comprehensive guide will turn you into a home baker extraordinaire. Includes troubleshooting tips for common issues.',
     DATEADD(MONTH, -30, GETDATE())),
    
    (2, 'Thai Green Curry From Scratch',
     'Skip the store-bought paste and make authentic Thai green curry from scratch. Fresh herbs, spices, and coconut milk create a vibrant, aromatic dish that''s both spicy and comforting. Includes vegetarian and seafood variations.',
     DATEADD(MONTH, -24, GETDATE())),
    
    (2, 'Perfect Chocolate Chip Cookies Every Time',
     'After baking hundreds of batches, I''ve perfected the ultimate chocolate chip cookie recipe. Brown butter, a mix of sugars, and the right ratio of ingredients create cookies that are crispy on the edges and chewy in the middle.',
     DATEADD(MONTH, -18, GETDATE())),
    
    (2, 'Summer BBQ Essentials and Marinades',
     'Take your grilling game to the next level with these tested marinades and rubs. From Korean-inspired bulgogi to classic American BBQ, these recipes will make you the star of any summer cookout. Includes timing and temperature guides.',
     DATEADD(MONTH, -12, GETDATE())),
    
    (2, 'Homemade Pasta Making for Beginners',
     'Fresh pasta is easier to make than you think. With just flour, eggs, and a rolling pin (or pasta machine), you can create restaurant-quality pasta at home. Includes recipes for fettuccine, ravioli, and pappardelle.',
     DATEADD(MONTH, -7, GETDATE())),
    
    (2, 'Fall Comfort Food: Butternut Squash Risotto',
     'As temperatures drop, this creamy butternut squash risotto brings warmth and comfort. The key is patience and constant stirring, but the result is worth every minute. Topped with crispy sage and parmesan for the perfect fall meal.',
     DATEADD(MONTH, -4, GETDATE())),
    
    (2, 'Holiday Baking: Traditional Gingerbread House',
     'Create a stunning gingerbread house from scratch with this detailed guide. Includes templates, royal icing recipe, and decorating ideas. Perfect family activity for the holiday season that results in an edible centerpiece.',
     DATEADD(MONTH, -1, GETDATE()));
GO

-- =======================================================
-- WANDERLUST WITH MIKE - Blog Posts
-- =======================================================

-- Author ID: 3 (Mike - Travel Blog)
INSERT INTO [dbo].[BlogPost] ([FK_Author_Id], [PostTitle], [PostContent], [CreationDate])
VALUES
    (3, 'Hidden Gems of Southeast Asia',
     'Beyond the tourist hotspots of Bangkok and Bali lie incredible destinations waiting to be discovered. From the emerald pools of Laos to the ancient temples of Myanmar, this guide reveals lesser-known destinations that offer authentic cultural experiences.',
     DATEADD(MONTH, -35, GETDATE())),
    
    (3, 'Budget Backpacking Through Europe',
     'You don''t need a fortune to explore Europe. This comprehensive guide covers budget accommodation, transportation hacks, free activities, and how to eat well without breaking the bank. Includes a 3-week itinerary under $2000.',
     DATEADD(MONTH, -29, GETDATE())),
    
    (3, 'Solo Travel Safety Tips for First Timers',
     'Traveling alone can be intimidating but incredibly rewarding. Learn essential safety practices, how to meet fellow travelers, staying connected, and building confidence for your first solo adventure. Personal insights from visiting 40 countries alone.',
     DATEADD(MONTH, -22, GETDATE())),
    
    (3, 'Ultimate Guide to Japanese Rail Pass',
     'Navigating Japan''s train system is easier than you think. The JR Pass can save you hundreds of dollars, but only if used strategically. Complete guide to routes, reservations, and maximizing value during your Japan adventure.',
     DATEADD(MONTH, -16, GETDATE())),
    
    (3, 'Adventure Travel in New Zealand',
     'New Zealand is an adrenaline junkie''s paradise. From bungee jumping in Queenstown to exploring glowworm caves and hiking the Tongariro Crossing, this guide covers the best adventure activities across both islands. Includes practical tips and costs.',
     DATEADD(MONTH, -11, GETDATE())),
    
    (3, 'Digital Nomad Life in Lisbon',
     'Lisbon has become a hub for remote workers. Discover the best coworking spaces, neighborhoods to live, visa requirements, cost of living, and why this vibrant European city is perfect for combining work and exploration.',
     DATEADD(MONTH, -6, GETDATE()));
GO

-- =======================================================
-- FIT LIFE BY EMMA - Blog Posts
-- =======================================================

-- Author ID: 4 (Emma - Fitness Blog)
INSERT INTO [dbo].[BlogPost] ([FK_Author_Id], [PostTitle], [PostContent], [CreationDate])
VALUES
    (4, 'Beginner Strength Training Guide',
     'Starting strength training can be overwhelming. This guide breaks down fundamental movements like squats, deadlifts, and bench press with proper form cues. Includes a 12-week progressive program for beginners to build a solid foundation.',
     DATEADD(MONTH, -33, GETDATE())),
    
    (4, 'Nutrition 101: Macros and Meal Planning',
     'Understanding macronutrients is key to reaching your fitness goals. Learn how to calculate your protein, carb, and fat needs, plus practical meal planning strategies and simple recipes that fit your macros without feeling restrictive.',
     DATEADD(MONTH, -27, GETDATE())),
    
    (4, 'Home Workouts: No Equipment Needed',
     'You don''t need a gym membership to stay fit. These bodyweight workouts target every muscle group using just your body weight. Perfect for busy schedules or traveling, with modifications for all fitness levels.',
     DATEADD(MONTH, -21, GETDATE())),
    
    (4, 'Marathon Training for First Timers',
     'Running 26.2 miles is achievable with proper training. This 16-week plan gradually builds your endurance, includes recovery protocols, nutrition strategies for long runs, and mental preparation tips to conquer race day.',
     DATEADD(MONTH, -15, GETDATE())),
    
    (4, 'Yoga for Athletes: Flexibility and Recovery',
     'Yoga isn''t just for relaxation—it''s a powerful tool for athletic performance. Learn specific poses for runners, weightlifters, and cyclists that improve flexibility, prevent injury, and speed up recovery between training sessions.',
     DATEADD(MONTH, -10, GETDATE())),
    
    (4, 'Breaking Through Fitness Plateaus',
     'Hit a wall in your progress? Plateaus are frustrating but normal. This post explores progressive overload, deload weeks, changing training variables, and nutrition adjustments to break through and continue making gains.',
     DATEADD(MONTH, -5, GETDATE())),
    
    (4, 'Pre and Post Workout Nutrition Timing',
     'When you eat matters as much as what you eat. Science-backed strategies for timing your meals around workouts to maximize performance, muscle growth, and recovery. Includes quick meal ideas for busy schedules.',
     DATEADD(MONTH, -1, GETDATE()));
GO

-- =======================================================
-- MONEY MATTERS WITH DAVID - Blog Posts
-- =======================================================

-- Author ID: 5 (David - Finance Blog)
INSERT INTO [dbo].[BlogPost] ([FK_Author_Id], [PostTitle], [PostContent], [CreationDate])
VALUES
    (5, 'Building Your First Investment Portfolio',
     'Starting to invest doesn''t have to be complicated. Learn the fundamentals of asset allocation, diversification, and risk tolerance. This guide covers index funds, ETFs, bonds, and how to build a balanced portfolio based on your goals and timeline.',
     DATEADD(MONTH, -32, GETDATE())),
    
    (5, 'Emergency Fund: How Much Do You Need',
     'An emergency fund is your financial safety net. Discover how to calculate the right amount for your situation, where to keep it, and strategies for building it up even on a tight budget. Real examples of when emergency funds save the day.',
     DATEADD(MONTH, -26, GETDATE())),
    
    (5, 'Understanding Credit Scores and How to Improve',
     'Your credit score affects everything from loan rates to apartment applications. Learn what factors impact your score, common myths, and actionable steps to improve your creditworthiness. Includes timeline for seeing results.',
     DATEADD(MONTH, -19, GETDATE())),
    
    (5, 'Retirement Planning in Your 30s',
     'The earlier you start, the better. Compound interest is your best friend for retirement. Explore 401k strategies, Roth vs Traditional IRA decisions, catch-up contributions, and calculating how much you need to retire comfortably.',
     DATEADD(MONTH, -13, GETDATE())),
    
    (5, 'Side Hustles: Diversifying Your Income',
     'Relying on a single income stream is risky. Explore legitimate side hustle ideas that can supplement your income, from freelancing to passive income streams. Includes tax implications and time management strategies.',
     DATEADD(MONTH, -8, GETDATE())),
    
    (5, 'Real Estate Investing for Beginners',
     'Real estate can build wealth but requires education first. Learn about rental properties, REITs, house flipping, and the 1% rule. Includes financing options, tax benefits, and common mistakes to avoid as a new real estate investor.',
     DATEADD(MONTH, -3, GETDATE())),
    
    (5, 'Tax Optimization Strategies for 2025',
     'Pay your fair share but not a penny more. Explore legal tax reduction strategies including retirement contributions, HSA benefits, tax-loss harvesting, and deductions often missed. Includes checklist for year-end tax planning.',
     DATEADD(WEEK, -3, GETDATE())),
    
    (5, 'Cryptocurrency: Should You Invest?',
     'Crypto is volatile but has captured investor attention. Balanced analysis of blockchain technology, different cryptocurrencies, risk factors, and whether crypto deserves a place in your portfolio. Not financial advice, but educational insight.',
     DATEADD(DAY, -10, GETDATE()));
GO

-- =======================================================
-- VERIFICATION QUERY
-- =======================================================

-- Display summary of created data
SELECT 'Authors Created' AS [Summary], COUNT(*) AS [Count] FROM [dbo].[Author]
UNION ALL
SELECT 'Blog Posts Created' AS [Summary], COUNT(*) AS [Count] FROM [dbo].[BlogPost];
GO

-- Show authors with their post counts
SELECT 
    a.[Id],
    a.[BlogTitle],
    a.[Email],
    COUNT(bp.[Id]) AS [PostCount]
FROM [dbo].[Author] a
LEFT JOIN [dbo].[BlogPost] bp ON a.[Id] = bp.[FK_Author_Id]
GROUP BY a.[Id], a.[BlogTitle], a.[Email]
ORDER BY a.[Id];
GO

-- Show distribution of posts over time
SELECT 
    YEAR(CreationDate) AS [Year],
    DATENAME(MONTH, CreationDate) AS [Month],
    COUNT(*) AS [PostCount]
FROM [dbo].[BlogPost]
GROUP BY YEAR(CreationDate), DATENAME(MONTH, CreationDate), MONTH(CreationDate)
ORDER BY YEAR(CreationDate), MONTH(CreationDate);
GO

PRINT 'Test data created successfully!';
PRINT '5 Authors created with distinct blog themes';
PRINT 'Blog posts distributed over past 3 years';
PRINT 'Test password for all users: Password123!';
GO

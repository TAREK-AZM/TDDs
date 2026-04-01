# 🧪 API Integration Testing Roadmap

A step-by-step, reusable guide for setting up and executing integration tests on any .NET Web API project.

---

## Phase 1: Project Setup & Infrastructure

### Step 1 — Create the Test Project
```bash
dotnet new xunit -n YourApi.IntegrationTests
dotnet add YourApi.IntegrationTests reference ../YourApi/YourApi.csproj
```

### Step 2 — Install Required NuGet Packages
| Package | Purpose |
|---------|---------|
| `Microsoft.AspNetCore.Mvc.Testing` | In-memory test server (`WebApplicationFactory`) |
| `Microsoft.EntityFrameworkCore.InMemoryDatabase` | In-memory DB for isolated tests |
| `FluentAssertions` | Readable assertion syntax |
| `Moq` *(optional)* | Mocking external services |
| `Bogus` *(optional)* | Realistic fake data generation |

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package FluentAssertions
```

### Step 3 — Create a Custom `WebApplicationFactory`
This is the **heart** of .NET integration testing — it spins up your API in-memory.

```csharp
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove the real database registration
            var dbDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbDescriptor != null) services.Remove(dbDescriptor);

            // 2. Add an in-memory database
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            // 3. (Optional) Replace external services with fakes
            // services.AddScoped<IEmailService, FakeEmailService>();
        });

        builder.UseEnvironment("Testing");
    }
}
```

> [!TIP]
> Keep this factory in a shared folder so all test classes can reuse it.

---

## Phase 2: Organize Your Tests

### Step 4 — Define a Folder Structure
```
YourApi.IntegrationTests/
├── Fixtures/
│   ├── CustomWebApplicationFactory.cs
│   └── TestDataSeeder.cs
├── Helpers/
│   ├── HttpClientExtensions.cs   // e.g., AuthenticatedClient()
│   └── TestConstants.cs
├── Controllers/
│   ├── ProductsControllerTests.cs
│   ├── OrdersControllerTests.cs
│   └── AuthControllerTests.cs
└── appsettings.Testing.json
```

### Step 5 — Create a Base Test Class *(optional but recommended)*
```csharp
public abstract class IntegrationTestBase
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory<Program> Factory;

    protected IntegrationTestBase(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    // Helper: deserialize JSON responses
    protected async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
```

---

## Phase 3: Write the Tests (CRUD Pattern)

### Step 6 — Test Each Endpoint Systematically

For every controller/resource, cover these **5 core scenarios**:

| # | Scenario | HTTP Method | Expected Status |
|---|----------|-------------|-----------------|
| 1 | Get all items | `GET /api/items` | `200 OK` |
| 2 | Get single item | `GET /api/items/{id}` | `200 OK` / `404 Not Found` |
| 3 | Create a valid item | `POST /api/items` | `201 Created` |
| 4 | Update an existing item | `PUT /api/items/{id}` | `200 OK` / `204 No Content` |
| 5 | Delete an item | `DELETE /api/items/{id}` | `204 No Content` / `404` |

### Step 7 — Example: Full CRUD Test Class
```csharp
public class ProductsControllerTests : IntegrationTestBase
{
    public ProductsControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory) { }

    // ────────── CREATE ──────────
    [Fact]
    public async Task POST_CreateProduct_ReturnsCreated()
    {
        // Arrange
        var newProduct = new { Name = "Widget", Price = 9.99 };
        var content = new StringContent(
            JsonSerializer.Serialize(newProduct),
            Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await DeserializeResponse<ProductDto>(response);
        created!.Name.Should().Be("Widget");
    }

    // ────────── READ (all) ──────────
    [Fact]
    public async Task GET_AllProducts_ReturnsOk()
    {
        var response = await Client.GetAsync("/api/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ────────── READ (single) ──────────
    [Fact]
    public async Task GET_NonExistentProduct_ReturnsNotFound()
    {
        var response = await Client.GetAsync("/api/products/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ────────── UPDATE ──────────
    [Fact]
    public async Task PUT_ExistingProduct_ReturnsOk()
    {
        // Seed data first, then update...
    }

    // ────────── DELETE ──────────
    [Fact]
    public async Task DELETE_ExistingProduct_ReturnsNoContent()
    {
        // Seed data first, then delete...
    }
}
```

---

## Phase 4: Handle Cross-Cutting Concerns

### Step 8 — Test Authentication & Authorization
```csharp
// Helper to create an authenticated client
protected HttpClient CreateAuthenticatedClient(string role = "Admin")
{
    var client = Factory.CreateClient();
    var token = GenerateTestJwt(role);  // create a valid JWT for testing
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    return client;
}

[Fact]
public async Task GET_ProtectedEndpoint_WithoutToken_Returns401()
{
    var response = await Client.GetAsync("/api/admin/dashboard");
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}

[Fact]
public async Task GET_ProtectedEndpoint_WithToken_Returns200()
{
    var authedClient = CreateAuthenticatedClient();
    var response = await authedClient.GetAsync("/api/admin/dashboard");
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

### Step 9 — Test Validation & Error Handling
```csharp
[Theory]
[InlineData("", 10.0)]    // empty name
[InlineData("OK", -5.0)]  // negative price
public async Task POST_InvalidProduct_ReturnsBadRequest(string name, double price)
{
    var payload = new { Name = name, Price = price };
    var content = new StringContent(
        JsonSerializer.Serialize(payload),
        Encoding.UTF8, "application/json");

    var response = await Client.PostAsync("/api/products", content);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}
```

### Step 10 — Test Pagination, Filtering & Sorting
```csharp
[Fact]
public async Task GET_Products_WithPagination_ReturnsCorrectPage()
{
    var response = await Client.GetAsync("/api/products?page=1&pageSize=5");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var result = await DeserializeResponse<PagedResult<ProductDto>>(response);
    result!.Items.Should().HaveCountLessOrEqualTo(5);
}
```

---

## Phase 5: Data Management

### Step 11 — Seed Test Data
```csharp
public static class TestDataSeeder
{
    public static void SeedProducts(AppDbContext db)
    {
        if (db.Products.Any()) return;

        db.Products.AddRange(
            new Product { Id = 1, Name = "Alpha", Price = 10 },
            new Product { Id = 2, Name = "Beta",  Price = 20 },
            new Product { Id = 3, Name = "Gamma", Price = 30 }
        );
        db.SaveChanges();
    }
}
```

> [!IMPORTANT]
> Always ensure **test isolation** — each test should start with a known, clean state. Use `IClassFixture` for shared setup, or re-seed per-test when tests modify data.

### Step 12 — Reset the Database Between Tests
```csharp
// In your factory or base class:
public void ResetDatabase()
{
    using var scope = Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    TestDataSeeder.SeedProducts(db);
}
```

---

## Phase 6: Run, Review & CI/CD

### Step 13 — Run All Tests
```bash
dotnet test --verbosity normal
```

### Step 14 — Naming Convention Checklist
Use a consistent naming pattern so test output is self-documenting:

```
{METHOD}_{Endpoint}_{Scenario}_Returns{StatusCode}
```

**Examples:**
- `POST_Products_ValidPayload_ReturnsCreated`
- `GET_Products_Unauthorized_Returns401`
- `DELETE_Products_NonExistent_ReturnsNotFound`

### Step 15 — Add to CI/CD Pipeline
```yaml
# Example: GitHub Actions
- name: Run Integration Tests
  run: dotnet test ./YourApi.IntegrationTests --no-build --logger trx
```

---

## 📋 Quick Checklist (Copy for Every New Project)

```markdown
- [ ] Create test project & add references
- [ ] Install packages (Mvc.Testing, InMemory, FluentAssertions)
- [ ] Build CustomWebApplicationFactory
- [ ] Set up folder structure & base test class
- [ ] Write CRUD tests for each controller
- [ ] Cover auth/authz (401, 403 scenarios)
- [ ] Cover validation (400 Bad Request scenarios)
- [ ] Cover edge cases (404, empty results, duplicates)
- [ ] Add pagination/filtering/sorting tests (if applicable)
- [ ] Implement data seeding & database reset
- [ ] Integrate into CI/CD pipeline
```

---

## 🧠 Golden Rules

| Rule | Why |
|------|-----|
| **One assertion per concept** | Keeps tests focused and failures clear |
| **Tests must be independent** | No test should depend on another test's side-effects |
| **Use in-memory DB for speed** | Real DB only when testing DB-specific behavior |
| **Mock external services** | Don't call real APIs (email, payment, etc.) in tests |
| **Descriptive test names** | The test name IS the documentation |
| **Arrange → Act → Assert** | Follow the AAA pattern in every test |

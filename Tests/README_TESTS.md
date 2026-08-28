# Unit Tests for Facility Maintenance System

## Overview

This directory contains comprehensive unit tests for the Facility Maintenance System, covering all service layers and business logic.

## Test Framework

- **Testing Framework:** xUnit
- **Mocking Library:** Moq
- **Coverage:** Service Layer & Helpers

## Test Organization

```
Tests/
├── Services/
│   ├── MaintenanceRequestServiceTests.cs      (STEP 1 & 3)
│   ├── ApprovalServiceTests.cs                 (STEP 2)
│   ├── MaintenanceWorkServiceTests.cs          (STEP 6)
│   ├── CompletionVerificationServiceTests.cs   (STEP 7)
│   ├── FeedbackServiceTests.cs                 (STEP 8)
│   └── LocationServiceTests.cs                 (Reference Data)
├── Helpers/
│   ├── ValidationHelperTests.cs
│   └── AuditLogServiceTests.cs
└── FacilityMaintenanceSystem.Tests.csproj
```

## Running Tests

### Using Visual Studio
1. Open Test Explorer (Test > Windows > Test Explorer)
2. Click "Run All"
3. View results in Test Explorer window

### Using Command Line (dotnet CLI)
```bash
dotnet test
```

### Using PowerShell (NUnit Console)
```powershell
.\packages\xunit.runner.console\tools\xunit.console.exe FacilityMaintenanceSystem.Tests.dll
```

## Test Coverage

### Service Tests

#### MaintenanceRequestServiceTests (8 tests)
- ✅ Create request with valid data
- ✅ Create request with null data throws exception
- ✅ Generate tracking IDs are unique
- ✅ Tracking IDs include current year
- ✅ Get requests by status
- ✅ Update request
- ✅ Update null request throws exception

#### ApprovalServiceTests (6 tests)
- ✅ Create approval with valid data
- ✅ Create approval with null data throws exception
- ✅ Approve request updates status
- ✅ Reject request updates status
- ✅ IsRequestApproved returns true for approved
- ✅ IsRequestApproved returns false for pending

#### MaintenanceWorkServiceTests (5 tests)
- ✅ Create work with valid data
- ✅ Create work with null data throws exception
- ✅ Start work sets status to InProgress
- ✅ Complete work calculates labor hours
- ✅ GetTotalLaborHours sums correctly

#### CompletionVerificationServiceTests (6 tests)
- ✅ Create verification with valid data
- ✅ Create verification with null data throws exception
- ✅ Verify completion updates status correctly
- ✅ IsRequestCompleted returns true for verified
- ✅ IsRequestCompleted returns false for unverified
- ✅ Failed verification sends back to rework

#### FeedbackServiceTests (7 tests)
- ✅ Create feedback with valid rating
- ✅ Create feedback with invalid rating throws exception
- ✅ Create feedback with zero rating throws exception
- ✅ Create null feedback throws exception
- ✅ Calculate average satisfaction rating
- ✅ Close request updates status

#### LocationServiceTests (4 tests)
- ✅ Create location with valid data
- ✅ Create location with null data throws exception
- ✅ Get locations by building
- ✅ Get locations by floor

### Helper Tests

#### ValidationHelperTests (8 tests)
- ✅ Validate email addresses
- ✅ Validate phone numbers
- ✅ Validate file types
- ✅ Validate urgency levels
- ✅ Validate priority levels
- ✅ Validate satisfaction ratings (1-5)
- ✅ Get validation messages

#### AuditLogServiceTests (2 tests)
- ✅ Log actions without throwing
- ✅ Log actions with old/new values

## Total Test Count: 48+ Tests

## Test Patterns Used

### Arrange-Act-Assert (AAA)
All tests follow the AAA pattern for clarity and maintainability.

```csharp
[Fact]
public void TestMethod_Scenario_ExpectedResult()
{
    // Arrange - Setup test data
    var input = new TestData();
    
    // Act - Execute the method being tested
    var result = service.MethodUnderTest(input);
    
    // Assert - Verify the result
    Assert.Equal(expected, result);
}
```

### Mocking with Moq
DbContext and dependencies are mocked to isolate unit tests.

```csharp
private Mock<FacilityMaintenanceContext> _mockContext;
private Service _service;

public ServiceTests()
{
    _mockContext = new Mock<FacilityMaintenanceContext>();
    _service = new Service(_mockContext.Object);
}
```

### Theory Tests with InlineData
Data-driven tests use xUnit Theory for multiple scenarios.

```csharp
[Theory]
[InlineData("valid@email.com", true)]
[InlineData("invalid.email", false)]
public void ValidateEmail_WithVariousInputs_ReturnsExpected(string email, bool expected)
{
    var result = _validator.ValidateEmail(email);
    Assert.Equal(expected, result);
}
```

## CI/CD Integration

These tests are designed to run in:
- Visual Studio Test Explorer
- Azure DevOps Pipelines
- GitHub Actions
- Jenkins
- TeamCity

## Future Enhancements

- [ ] Integration tests with real database
- [ ] Controller action tests
- [ ] View model tests
- [ ] End-to-end workflow tests
- [ ] Performance tests
- [ ] Code coverage reports (OpenCover, Codecov)

## Best Practices

1. **Isolation:** Each test is independent
2. **Clarity:** Test names describe what is being tested
3. **Speed:** Unit tests run quickly (< 1 second each)
4. **Determinism:** Tests always produce the same result
5. **Maintainability:** Tests are easy to understand and update

## Troubleshooting

### Tests Won't Run
- Ensure xUnit and Moq NuGet packages are installed
- Check that test project references the main project
- Rebuild solution before running tests

### Mock Setup Issues
- Verify DbSet<T> is properly mocked
- Ensure IQueryable data is configured correctly
- Check that async/await patterns match method signatures

### Assertion Failures
- Review test logic and expected values
- Check if business logic changed
- Verify test data matches business requirements

## Contributing

When adding new features:
1. Write tests first (TDD approach)
2. Ensure tests pass
3. Add to appropriate test class
4. Document complex test scenarios
5. Run full test suite before committing

---

**Last Updated:** 2026-08-28
**Framework Version:** xUnit 2.4.1
**Status:** Active Development

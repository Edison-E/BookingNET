# Testing
In this section of the Testing documentation, we will explain the following sections:
- Tools used.
- Location of the tests.
- Nomenclature of the methods.


### Tools
To perform the tests in this project, we will use the following NuGet:
- xUnit → Allows us to write and run unit tests.
- Moq → Used to simulate dependencies and create mocks in the tests.

### Location
To organize the tests correctly, we follow the following structure:
- 📂 test
  - 📂 BookPro.UnitTest
     - 📂 Application
     - 📂 Repository
     - 📄 Usings.cs

Location rules:
 - If it is a test of a service → It is stored in 📂 Application.
 - If it is a test of a repository → It is saved in 📂 Repository.
 - If we add tests for another layer → We create a new folder with the name of the layer.

### Nomenclatura 
When creating a test, we must follow a clear naming convention for the methods:

``public async Task {MethodThatTests}_{Condition}_{ResultExpected}``


Now that you know what NuGet is used for, the location of the tests and the moneclature, you can perform the corresponding tests, here is an example of how a test should look like:

````
    [Fact]
    public async Task GetUser_UserExists_ShouldReturnUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User { Email = email, Name = "Test User" };
        var userDTO = new UserDTO { Email = email, Name = "Test User" };

        _userRepositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string>() });
        _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessUser.UserFind)).Returns("User found successfully");

        // Act
        var result = await _userService.GetUser(email);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(userDTO, result.UserDTO);
        Assert.Contains("User found successfully", result.Message);
    }
````
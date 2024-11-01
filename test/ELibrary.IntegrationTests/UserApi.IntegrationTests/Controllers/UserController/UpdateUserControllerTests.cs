﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class UpdateUserControllerTests : BaseUserControllerTest
    {
        [Test]
        public async Task UpdateUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "olduser1@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            var updateRequest = new UserUpdateDataRequest
            {
                Email = "updateduser1@example.com",
                OldPassword = "123456;QWERTY",
                Password = "NEW123456;QWERTY"
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/update");
            var accessKey = await GetAccessKeyForUserAsync(new UserAuthenticationRequest
            {
                Login = "olduser1@example.com",
                Password = "123456;QWERTY",
            });
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var loginRequest = new UserAuthenticationRequest
            {
                Login = "updateduser1@example.com",
                Password = "NEW123456;QWERTY"
            };
            using var request2 = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
            request2.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response2 = await client.SendAsync(request2);
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response2.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<UserAuthenticationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(authResponse);
            Assert.That(authResponse.Email, Is.EqualTo("updateduser1@example.com"));
        }
        [Test]
        public async Task UpdateUser_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest
            {
                Email = "updateduser2@example.com",
                OldPassword = "123456;QWERTY",
                Password = "NEW123456;QWERTY"
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/update");
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task UpdateUser_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest
            {
                Email = "",
                OldPassword = "123456;QWERTY",
                Password = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/update");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateUser_ConflictingEmail_ReturnsConflictError()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "conflict@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "conflict2@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            var updateRequest = new UserUpdateDataRequest
            {
                Email = "conflict@example.com",  // Conflicting email with another user
                OldPassword = "123456;QWERTY",
                Password = "123456;QWERTY"
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/update");
            var accessKey = await GetAccessKeyForUserAsync(new UserAuthenticationRequest
            {
                Login = "conflict2@example.com",
                Password = "123456;QWERTY",
            });
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }
    }
}
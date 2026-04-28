using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LoanSphere_Frontend.Models;
using Microsoft.Extensions.Options;

namespace LoanSphere_Frontend.Services
{
    public class LoanSphereApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public LoanSphereApiClient(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        public async Task<ApiResult<AuthResponseViewModel>> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var normalizedRole = string.IsNullOrWhiteSpace(model.Role) ? "Customer" : model.Role.Trim();
                var client = CreateClient();
                var endpoint = normalizedRole switch
                {
                    "Admin" => $"{_apiSettings.AuthBaseUrl}/api/auth/register/admin",
                    "Manager" => $"{_apiSettings.AuthBaseUrl}/api/auth/register/manager",
                    _ => $"{_apiSettings.AuthBaseUrl}/api/auth/register/customer"
                };

                object payload = normalizedRole switch
                {
                    "Admin" => new
                    {
                        model.FullName,
                        model.Email,
                        model.Phone,
                        model.Password,
                        model.ConfirmPassword,
                        AdminSecretKey = model.SecretKey
                    },
                    "Manager" => new
                    {
                        model.FullName,
                        model.Email,
                        model.Phone,
                        model.Password,
                        model.ConfirmPassword,
                        ManagerSecretKey = model.SecretKey
                    },
                    _ => new
                    {
                        model.FullName,
                        model.Email,
                        model.Phone,
                        model.Password,
                        model.ConfirmPassword
                    }
                };

                var response = await client.PostAsJsonAsync(endpoint, payload);
                return await ReadResponseAsync<AuthResponseViewModel>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<AuthResponseViewModel>(ex, "registration");
            }
        }

        public async Task<ApiResult<AuthResponseViewModel>> LoginAsync(LoginViewModel model)
        {
            try
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync($"{_apiSettings.AuthBaseUrl}/api/auth/login", model);
                return await ReadResponseAsync<AuthResponseViewModel>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<AuthResponseViewModel>(ex, "login");
            }
        }

        public async Task<ApiResult<ProfileViewModel>> GetProfileAsync(string userId, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.GetAsync($"{_apiSettings.ProfileBaseUrl}/api/profile/{userId}");
                return await ReadResponseAsync<ProfileViewModel>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<ProfileViewModel>(ex, "loading the profile");
            }
        }

        public async Task<ApiResult<bool>> UpdateProfileAsync(string userId, ProfileEditViewModel model, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.PutAsJsonAsync($"{_apiSettings.ProfileBaseUrl}/api/profile/{userId}", new
                {
                    model.ProfilePictureUrl,
                    model.PanCardNumber,
                    model.AadhaarNumber
                });

                var result = await ReadResponseAsync<string>(response);
                return new ApiResult<bool>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Success
                };
            }
            catch (Exception ex)
            {
                return CreateFailure<bool>(ex, "updating the profile");
            }
        }

        public async Task<ApiResult<List<LoanSummaryViewModel>>> GetLoansByUserAsync(string userId, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.GetAsync($"{_apiSettings.LoanBaseUrl}/loan/user/{userId}");
                return await ReadResponseAsync<List<LoanSummaryViewModel>>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<List<LoanSummaryViewModel>>(ex, "loading user loans");
            }
        }

        public async Task<ApiResult<List<LoanSummaryViewModel>>> GetAllLoansAsync(string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.GetAsync($"{_apiSettings.LoanBaseUrl}/loan/getall");
                return await ReadResponseAsync<List<LoanSummaryViewModel>>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<List<LoanSummaryViewModel>>(ex, "loading all loans");
            }
        }

        public async Task<ApiResult<LoanSummaryViewModel>> GetLoanByIdAsync(int id, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.GetAsync($"{_apiSettings.LoanBaseUrl}/loan/getbyid/{id}");
                return await ReadResponseAsync<LoanSummaryViewModel>(response);
            }
            catch (Exception ex)
            {
                return CreateFailure<LoanSummaryViewModel>(ex, "loading the loan");
            }
        }

        public async Task<ApiResult<bool>> ApplyLoanAsync(AuthenticatedUser user, LoanApplicationViewModel model, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.PostAsJsonAsync($"{_apiSettings.LoanBaseUrl}/loan/create", new
                {
                    UserId = user.UserId,
                    model.Amount,
                    model.LoanType,
                    model.TermInMonths,
                    model.Purpose
                });

                var result = await ReadResponseAsync<object>(response);
                return new ApiResult<bool>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Success
                };
            }
            catch (Exception ex)
            {
                return CreateFailure<bool>(ex, "submitting the loan application");
            }
        }

        public async Task<ApiResult<bool>> UpdateLoanDecisionAsync(int loanId, string reviewerRole, string status, string? reason, string? token = null)
        {
            try
            {
                var client = CreateClient(token);
                var response = await client.PatchAsJsonAsync($"{_apiSettings.LoanBaseUrl}/loan/{loanId}/decision", new
                {
                    ReviewerRole = reviewerRole,
                    Status = status,
                    Reason = reason
                });

                var result = await ReadResponseAsync<object>(response);
                return new ApiResult<bool>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Success
                };
            }
            catch (Exception ex)
            {
                return CreateFailure<bool>(ex, "updating the loan decision");
            }
        }

        private static ApiResult<T> CreateFailure<T>(Exception ex, string action)
        {
            return new ApiResult<T>
            {
                Success = false,
                Message = $"There was a problem while {action}. Make sure the backend services are running. Details: {ex.Message}"
            };
        }

        private HttpClient CreateClient(string? token = null)
        {
            var client = _httpClientFactory.CreateClient("LoanSphereApi");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        private async Task<ApiResult<T>> ReadResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResult<T>
                {
                    Success = false,
                    Message = ExtractMessage(content, response.ReasonPhrase)
                };
            }

            if (typeof(T) == typeof(string))
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Message = string.IsNullOrWhiteSpace(content) ? "Success" : content.Trim('"'),
                    Data = (T)(object)content.Trim('"')
                };
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return new ApiResult<T>
                {
                    Success = true,
                    Message = "Success"
                };
            }

            var data = JsonSerializer.Deserialize<T>(content, _jsonOptions);

            var message = ExtractMessage(content, "Success");

            return new ApiResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        private static string ExtractMessage(string? content, string? fallback)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return fallback ?? "Request completed.";
            }

            try
            {
                using var document = JsonDocument.Parse(content);
                if (document.RootElement.ValueKind == JsonValueKind.Object &&
                    document.RootElement.TryGetProperty("message", out var messageProperty))
                {
                    return messageProperty.GetString() ?? fallback ?? "Request completed.";
                }
            }
            catch (JsonException)
            {
            }

            return content.Trim('"');
        }
    }
}

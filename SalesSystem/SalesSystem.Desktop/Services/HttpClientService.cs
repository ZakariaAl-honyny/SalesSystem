using System.Net.Http.Json;
using System.Text.Json;
using SalesSystem.Common;
using Serilog;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services;

public class HttpClientService
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
    {
        if (element.TryGetProperty(propertyName, out value))
            return true;
        if (element.TryGetProperty(char.ToUpper(propertyName[0]) + propertyName.Substring(1), out value))
            return true;
        return false;
    }

    public HttpClientService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Result<IReadOnlyList<T>>> GetListAsync<T>(string path, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetAsync(path, ct);
            if (!response.IsSuccessStatusCode) return await HandleErrorResponse<IReadOnlyList<T>>(response, ct);

            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            // Handle PagedResult { items: [], totalCount: N }
            if (TryGetPropertyIgnoreCase(doc.RootElement, "items", out var itemsProp))
            {
                var items = itemsProp.Deserialize<List<T>>(_jsonOptions);
                return Result<IReadOnlyList<T>>.Success(items ?? new List<T>());
            }

            // Handle Wrapped Result<T> { isSuccess: true, value: [...] }
            if (TryGetPropertyIgnoreCase(doc.RootElement, "value", out var valueProp))
            {
                if (valueProp.ValueKind == JsonValueKind.Array)
                {
                    var items = valueProp.Deserialize<List<T>>(_jsonOptions);
                    return Result<IReadOnlyList<T>>.Success(items ?? new List<T>());
                }

                // Handle Wrapped PagedResult { isSuccess: true, value: { items: [...] } }
                if (valueProp.ValueKind == JsonValueKind.Object && TryGetPropertyIgnoreCase(valueProp, "items", out var nestedItems))
                {
                    var items = nestedItems.Deserialize<List<T>>(_jsonOptions);
                    return Result<IReadOnlyList<T>>.Success(items ?? new List<T>());
                }
            }

            // Handle raw List [...]
            try
            {
                var result = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
                return result != null ? Result<IReadOnlyList<T>>.Success(result) : Result<IReadOnlyList<T>>.Success(new List<T>());
            }
            catch
            {
                Log.Warning("GetListAsync: Unrecognized JSON format for {Path}. First 500 chars: {Json}", path, json[..Math.Min(json.Length, 500)]);
                return Result<IReadOnlyList<T>>.Success(new List<T>());
            }
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<T>>(ex); }
    }

    public async Task<Result<T>> GetAsync<T>(string path, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetAsync(path, ct);
            if (!response.IsSuccessStatusCode) return await HandleErrorResponse<T>(response, ct);

            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            // Handle Wrapped Result<T> { isSuccess: true, value: {...} }
            if (TryGetPropertyIgnoreCase(doc.RootElement, "value", out var valueProp) && valueProp.ValueKind == JsonValueKind.Object)
            {
                var value = valueProp.Deserialize<T>(_jsonOptions);
                return value != null ? Result<T>.Success(value) : Result<T>.Failure("استجابة فارغة");
            }

            // Handle raw object {...}
            var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);
            return result != null ? Result<T>.Success(result) : Result<T>.Failure("استجابة فارغة");
        }
        catch (Exception ex) { return HandleException<T>(ex); }
    }

    public async Task<Result<T>> PostAsync<T>(string path, object body, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.PostAsJsonAsync(path, body, _jsonOptions, ct);
            return await DeserializeResultOrRaw<T>(response, ct);
        }
        catch (Exception ex) { return HandleException<T>(ex); }
    }

    public async Task<Result> PostAsync(string path, object? body = null, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.PostAsJsonAsync(path, body, _jsonOptions, ct);
            if (response.IsSuccessStatusCode) return Result.Success();
            return await HandleErrorResponse(response, ct);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<Result<T>> PutAsync<T>(string path, object? body = null, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.PutAsJsonAsync(path, body, _jsonOptions, ct);
            return await DeserializeResultOrRaw<T>(response, ct);
        }
        catch (Exception ex) { return HandleException<T>(ex); }
    }

    public async Task<Result> PutAsync(string path, object? body = null, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.PutAsJsonAsync(path, body, _jsonOptions, ct);
            if (response.IsSuccessStatusCode) return Result.Success();
            return await HandleErrorResponse(response, ct);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    private async Task<Result<T>> DeserializeResultOrRaw<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (!response.IsSuccessStatusCode)
            return await HandleErrorResponse<T>(response, ct);

        var json = await response.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrEmpty(json))
            return Result<T>.Failure("استجابة فارغة");

        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind == JsonValueKind.Object && TryGetPropertyIgnoreCase(doc.RootElement, "value", out var valueProp))
        {
            var result = valueProp.Deserialize<T>(_jsonOptions);
            return result != null ? Result<T>.Success(result) : Result<T>.Failure("استجابة فارغة");
        }

        var direct = JsonSerializer.Deserialize<T>(json, _jsonOptions);
        return direct != null ? Result<T>.Success(direct) : Result<T>.Failure("استجابة فارغة");
    }

    public async Task<Result> DeleteAsync(string path, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.DeleteAsync(path, ct);
            if (response.IsSuccessStatusCode) return Result.Success();
            return await HandleErrorResponse(response, ct);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    private async Task<Result<T>> HandleErrorResponse<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var error = await ParseError(response, ct);
        return Result<T>.Failure(error);
    }

    private async Task<Result> HandleErrorResponse(HttpResponseMessage response, CancellationToken ct)
    {
        var error = await ParseError(response, ct);
        return Result.Failure(error);
    }

    private async Task<string> ParseError(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            return "انتهت الجلسة أو غير مصرح لك. يرجى تسجيل الدخول مرة أخرى.";
        
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            return "ليس لديك الصلاحية الكافية لهذه العملية.";

        try
        {
            var content = await response.Content.ReadAsStringAsync(ct);
            if (string.IsNullOrEmpty(content)) return $"خطأ في الخادم: {response.StatusCode}";

            using var doc = JsonDocument.Parse(content);
            if (TryGetPropertyIgnoreCase(doc.RootElement, "error", out var errorProp))
                return errorProp.GetString() ?? content;
            
            if (TryGetPropertyIgnoreCase(doc.RootElement, "errors", out var errorsProp))
            {
                // Handle FluentValidation errors
                var firstError = errorsProp.EnumerateObject().FirstOrDefault();
                return $"{firstError.Value.EnumerateArray().FirstOrDefault()}";
            }
        }
        catch { }

        return $"خطأ غير متوقع: {response.StatusCode}";
    }

    private Result<T> HandleException<T>(Exception ex)
    {
        Log.Error(ex, "HTTP Client Exception");
        string error;
        if (ex is HttpRequestException || ex.InnerException is System.Net.Sockets.SocketException)
        {
            error = "تعذر الاتصال بالخادم. يرجى التأكد من تشغيل نظام الـ API.";
        }
        else
        {
            error = $"خطأ في الاتصال: {ex.Message}";
        }
        return Result<T>.Failure(error);
    }

    private Result HandleException(Exception ex)
    {
        Log.Error(ex, "HTTP Client Exception");
        string error;
        if (ex is HttpRequestException || ex.InnerException is System.Net.Sockets.SocketException)
        {
            error = "تعذر الاتصال بالخادم. يرجى التأكد من تشغيل نظام الـ API.";
        }
        else
        {
            error = $"خطأ في الاتصال: {ex.Message}";
        }
        return Result.Failure(error);
    }
}

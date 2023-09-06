using MudBlazor;
using rmsweb.Client.Components.Common;
using rmsweb.Client.Infrastructure.ApiClient;

namespace rmsweb.Client.Shared;
public static class ApiHelper
{
    public static async Task<T?> ExecuteCallGuardedAsync<T>(
        Func<Task<T>> call,
        ISnackbar snackbar,
        CustomValidation? customValidation = null,
        string? successMessage = null)
    {
        customValidation?.ClearErrors();
        try
        {
            var result = await call();

            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                snackbar.Add(successMessage, Severity.Info);
            }

            return result;
        }
        catch (Exception ex)
        {
            if (ex.Message is not null)
            {
                snackbar.Add(ex.Message, Severity.Error);
               // customValidation?.DisplayErrors(ex.Message);
            }
            else
            {
                snackbar.Add("Something went wrong!", Severity.Error);
            }
        }
 
        //catch (Exception ex)
        //{
        //    snackbar.Add(ex.Message, Severity.Error);
        //}

        return default;
    }

    public static async Task<bool> ExecuteCallGuardedAsync(
        Func<Task> call,
        ISnackbar snackbar,
        CustomValidation? customValidation = null,
        string? successMessage = null)
    {
        customValidation?.ClearErrors();
        try
        {
            await call();

            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                snackbar.Add(successMessage, Severity.Success);
            }

            return true;
        }
       /* catch (Exception<HttpValidationProblemDetails> ex)
        {
            if (ex.Result.Errors is not null)
            {
                customValidation?.DisplayErrors(ex.Result.Errors);
            }
            else
            {
                snackbar.Add("Something went wrong!", Severity.Error);
            }
        }*/
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }

        return false;
    }
}
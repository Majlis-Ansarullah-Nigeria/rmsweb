using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;
using rmsweb.Client.Shared;

namespace rmsweb.Client.Pages.Report;
public class ReportTypeAutoComplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<ReportTypeAutoComplete> L { get; set; } = default!;
    [Inject]
    protected IReportTypesClient ReportTypesClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<rmsweb.Client.Infrastructure.ApiClient.ReportTypeDto> _reportTypes = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["ReportType"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchReportTypes;
        ToStringFunc = GetBrandName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one brand to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await ReportTypesClient.GetReportTypeAsync(_value), Snackbar) is { } reportType)
        {
            _reportTypes.Add(reportType.Data);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchReportTypes(string value)
    {
        //var filter = await ReportTypesClient.GetReportType2Async(value);
        //if (filter != null)
        //{
        //    var e = await ReportTypesClient.GetReportTypesAsync();
        //    if (await ApiHelper.ExecuteCallGuardedAsync(
        //            async () => e.Data, Snackbar)
        //        is IEnumerable<rmsweb.Client.Infrastructure.ApiClient.ReportTypeDto> res)
        //    {
        //        _reportTypes = res.ToList();
        //    }

        //    return _reportTypes.Select(x => x.Id);
        //}
        
        var result = await ReportTypesClient.GetReportTypesAsync();
        if (await ApiHelper.ExecuteCallGuardedAsync(
                async () => result.Data, Snackbar)
            is IEnumerable<rmsweb.Client.Infrastructure.ApiClient.ReportTypeDto> response)
        {
            _reportTypes = response.ToList();
        }

        return _reportTypes.Select(x => x.Id);
    }

    private string GetBrandName(Guid id) =>
        _reportTypes.Find(b => b.Id == id)?.Name ?? string.Empty;
}
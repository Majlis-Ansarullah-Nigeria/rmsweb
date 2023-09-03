using Mapster;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using rmsweb.Client.Components.Dialogs;
using rmsweb.Client.Components.EntityTable;
using rmsweb.Client.Infrastructure.ApiClient;
using rmsweb.Client.Shared;
using System.Security.Cryptography;

namespace rmsweb.Client.Pages.Report;
public partial class ReportTypes
{
    [Inject]
    protected IReportTypesClient ReportTypesClient { get; set; } = default!;

    protected EntityServerTableContext<ReportTypeDto, Guid, ReportTypeViewModel> Context { get; set; } = default!;
    private bool _canViewRoles;

    private EntityTable<ReportTypeDto, Guid, ReportTypeViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["ReportType"],
            entityNamePlural: L["ReportTypes"],
           // entityResource: FSHResource.Products,
            fields: new()
            {
                new(prod => prod.Name, L["Name"], "Name"),
                //new(prod => prod.Description, L["Description"], "Description"),
                new(prod => prod.ReportTag.ToString(), L["Report Tag"], "Report Tag"),
            },
            enableAdvancedSearch: true,
            idFunc: reportType => reportType.Id,
            searchFunc: async filter =>
            {
                var result = await ReportTypesClient.GetReportTypesAsync();
                return result.Adapt<PaginationResponse<ReportTypeDto>>();
            },
            createFunc: async prod =>
            {
                var reportTag = (ReportTypeEnum)Enum.Parse(typeof(ReportTypeEnum), prod.ReportTagString);
                prod.ReportTag = reportTag;
                await ReportTypesClient.CreateReportTypeAsync(prod.Adapt<CreateReportTypeRequest>());
            },
            updateFunc: async (id, prod) =>
            {
                var reportTag = (ReportTypeEnum)Enum.Parse(typeof(ReportTypeEnum), prod.ReportTagString);
                prod.ReportTag = reportTag;
                await ReportTypesClient.UpdateReportTypeAsync(id, prod.Adapt<UpdateReportTypeRequest>());
            });

    // Advanced Search

    private Guid _searchBrandId;
    private Guid SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private void ViewReport(in Guid reportTypeId) =>
       Navigation.NavigateTo($"/reportTypes/{reportTypeId}");

    private void ManageReportSections(in Guid reportTypeId) =>
        Navigation.NavigateTo($"/reportTypes/{reportTypeId}/sections");

    private async Task View(ReportTypeDto entity)
    {
        _ = Context.IdFunc ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = L[entity.Description];
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, Context.EntityName) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
        var dialog = DialogService.Show<DeleteConfirmation>(L["test"], parameters, options);
    }
}

public class ReportTypeViewModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ReportTagString { get; set; }
    public ReportTypeEnum ReportTag { get; set; }
}

public class ReportTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ReportTypeEnum ReportTag { get; set; }
}
public enum ReportTypeEnum
{
    MuqamReportType = 1,

    DilaReportType,

    ZoneReportType,

    QaidReportType
}